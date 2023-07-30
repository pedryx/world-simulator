using Microsoft.Xna.Framework;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
/// <summary>
/// Marks all entities visible on screen as visible (sets <see cref="Appearance.Visible"/> to true).
/// </summary>
internal readonly struct RenderClipSystem : IEntityProcessor<Transform, Appearance>
{
    /// <summary>
    /// If entity's height is smaller than this threshold than entity is considered not visible.
    /// </summary>
    private const float sizeThreshold = 5.0f;

    private readonly Game game;
    private readonly Camera camera;

    public RenderClipSystem(Game game, Camera camera)
    {
        this.game = game;
        this.camera = camera;
    }

    public void Process(ref Transform transform, ref Appearance appearance, float deltaTime)
    {
        float maxWindowSize = game.Resolution.Length() * (1.0f / camera.Scale);
        float maxSpriteSize = (appearance.Sprite.GetSize() * transform.Scale).Length() * camera.Scale;

        if (maxSpriteSize < sizeThreshold)
        {
            appearance.Visible = false;
            return;
        }

        float distance = Vector2.Distance(camera.Position, transform.Position + appearance.Sprite.Position);

        appearance.Visible = (distance <= (maxWindowSize + maxSpriteSize));
    }
}
