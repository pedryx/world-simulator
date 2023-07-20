using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Systems;
internal class RenderSystem : EntityProcessor<Transform, Appearance>
{
    private readonly SpriteBatch spriteBatch;

    public RenderSystem(Game game, GameState gameState) 
        : base(game, gameState)
    {
        spriteBatch = game.SpriteBatch;
    }

    public override void PreUpdate(float deltaTime)
    {
        spriteBatch.Begin
        (
            sortMode: SpriteSortMode.FrontToBack,
            transformMatrix: GameState.Camera.GetTransformMatrix()
        );

        base.PreUpdate(deltaTime);
    }

    public override void Process
    (
        ref Transform transform,
        ref Appearance appearance,
        float deltaTime
    )
    {
        spriteBatch.Draw(appearance.Sprite, transform.Position, transform.Scale, transform.Rotation);
    }

    public override void PostUpdate(float deltaTime)
    {
        spriteBatch.End();

        base.PostUpdate(deltaTime);
    }
}