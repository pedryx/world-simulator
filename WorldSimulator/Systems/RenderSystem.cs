using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Systems;
internal readonly struct RenderSystem : IEntityProcessor<Location, Appearance>
{
    /// <summary>
    /// The squared size threshold for considering entities not visible.
    /// </summary>
    private const float sizeThresholdSquared = 100.0f;
    /// <summary>
    /// The camera view bounds offset, used for layer computation.
    /// </summary>
    private const float layerViewOffset = 512.0f;

    private readonly Game game;
    private readonly SpriteBatch spriteBatch;
    private readonly Camera camera;
    /// <summary>
    /// The camera view transform matrix.
    /// </summary>
    private readonly RefWrapper<Matrix> transform = new();
    /// <summary>
    /// The camera view bounds size used for layer computation.
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

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Location location, ref Appearance appearance, float deltaTime)
    {
        // Calculate bounds for camera view and entity.
        Rectangle viewBounds = camera.ViewBounds;
        Vector2 entitySize = appearance.Texture.GetSize() * appearance.Scale;
        Rectangle entityBounds = new
        (
            (location.Position - entitySize * appearance.Origin).ToPoint(),
            entitySize.ToPoint()
        );

        // Discard entities that are outside the camera view.
        if (!(viewBounds.Contains(location.Position) || viewBounds.Intersects(entityBounds)))
            return;
        // Discard entities that are too small.
        if ((entitySize * camera.Scale).LengthSquared() < sizeThresholdSquared)
            return;
        
        // Calculate the layer for the entity.
        Vector2 screenPosition = Vector2.Transform(location.Position, transform) 
            + new Vector2(layerViewOffset * camera.Scale);
        float layer = (screenPosition.Y * layerViewSize.Value.X + screenPosition.X) 
            / (layerViewSize.Value.X * layerViewSize.Value.Y);

        spriteBatch.Draw
        (
            appearance.Texture,
            location.Position,
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