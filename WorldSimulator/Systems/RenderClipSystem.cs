using Microsoft.Xna.Framework;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
/// <summary>
/// Marks all entities visible on screen as visible (sets <see cref="Appearance.Visible"/> to true).
/// </summary>
public class RenderClipSystem : EntityProcessor<Transform, Appearance>
{
    /// <summary>
    /// If entity's height is smaller than this threshold than entity is considered not visible.
    /// </summary>
    private const float sizeThreshold = 5.0f;

    public RenderClipSystem(Game game, GameState gameState) 
        : base(game, gameState) { }

    public override void Process(ref Transform transform, ref Appearance appearance, float deltaTime)
    {
        float maxWindowSize = Game.Resolution.Length();
        float maxSpriteSize = (appearance.Sprite.GetSize() * transform.Scale).Length() 
            * GameState.Camera.Scale;

        if (maxSpriteSize < sizeThreshold)
        {
            appearance.Visible = false;
            return;
        }

        float distance = Vector2.Distance
        (
            GameState.Camera.Position,
            transform.Position + appearance.Sprite.Position
        );

        appearance.Visible = (distance <= (maxWindowSize + maxSpriteSize));
    }
}
