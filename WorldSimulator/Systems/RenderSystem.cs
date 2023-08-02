using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Systems;
internal readonly struct RenderSystem : IEntityProcessor<Position, Appearance>
{
    /// <summary>
    /// If entity is smaller than this threshold than entity is considered not visible.
    /// </summary>
    private const float sizeThresholdSquared = 100.0f;
    /// <summary>
    /// Camera view bounds offset for layer computation.
    /// </summary>
    private const float layerViewOffset = 512.0f;

    private readonly Game game;
    private readonly SpriteBatch spriteBatch;
    private readonly Camera camera;
    /// <summary>
    /// Camera view transform matrix.
    /// </summary>
    private readonly RefWrapper<Matrix> transform = new();
    /// <summary>
    /// Camera view bounds size for layer computation.
    /// </summary>
    private readonly RefWrapper<Vector2> layerViewSize = new();

    public RenderSystem(Game game, Camera camera)
    {
        this.game = game;
        spriteBatch = game.SpriteBatch;
        this.camera = camera;
    }

    void IEntityProcessor.PreUpdate(float deltaTime)
    {
        transform.Value = camera.GetTransformMatrix() 
            * Matrix.CreateScale(game.ResolutionScale.X, game.ResolutionScale.Y, 1.0f);
        layerViewSize.Value = game.Resolution + new Vector2(2.0f * layerViewOffset * camera.Scale);

        spriteBatch.Begin
        (
            sortMode: SpriteSortMode.FrontToBack,
            transformMatrix: transform
        );
    }

    public void Process(ref Position position, ref Appearance appearance, float deltaTime)
    {
        // Calculate bounds for camera view and entity.
        Rectangle viewBounds = camera.ViewBounds;
        Vector2 entitySize = appearance.Texture.GetSize() * appearance.Scale;
        Rectangle entityBounds = new
        (
            (position.Coordinates - entitySize * appearance.Origin).ToPoint(),
            entitySize.ToPoint()
        );

        // Discard entities which are outside came view.
        if (!(viewBounds.Contains(position.Coordinates) || viewBounds.Intersects(entityBounds)))
            return;
        // Discard entities which are too small.
        if ((entitySize * camera.Scale).LengthSquared() < sizeThresholdSquared)
            return;
        
        // Calculate layer for the entity.
        Vector2 screenPosition = Vector2.Transform(position.Coordinates, transform) 
            + new Vector2(layerViewOffset * camera.Scale);
        float layer = (screenPosition.Y * layerViewSize.Value.X + screenPosition.X) 
            / (layerViewSize.Value.X * layerViewSize.Value.Y);

        spriteBatch.Draw
        (
            appearance.Texture,
            position.Coordinates,
            null,
            Color.White,
            0.0f,
            appearance.Origin * appearance.Texture.GetSize(),
            appearance.Scale,
            appearance.Effects,
            layer
        );
    }

    void IEntityProcessor.PostUpdate(float deltaTime)
    {
        spriteBatch.End();
    }
}