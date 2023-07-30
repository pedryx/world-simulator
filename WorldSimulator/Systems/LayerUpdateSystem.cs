using Microsoft.Xna.Framework;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
/// <summary>
/// Dynamically changes layer depth of entities based on their vertical position.
/// </summary>
internal readonly struct LayerUpdateSystem : IEntityProcessor<Transform, Appearance, LayerUpdate>
{
    private readonly Game game;
    private readonly Camera camera;

    private readonly RefWrapper<Matrix> cameraTransform = new();

    public LayerUpdateSystem(Game game, Camera camera)
    {
        this.game = game;
        this.camera = camera;
    }

    void IEntityProcessor.PreUpdate(float deltaTime)
    {
        cameraTransform.Value = camera.GetTransformMatrix();
    }

    public void Process(ref Transform transform, ref Appearance appearance, ref LayerUpdate _, float deltaTime)
    {
        if (!appearance.Visible)
            return;

        Vector2 position = Vector2.Transform(transform.Position + appearance.Sprite.Position, cameraTransform);

        if (game.GraphicsDevice.Viewport.Bounds.Contains(position))
        {
            appearance.Sprite.LayerDepth = (position.Y * game.Resolution.Y + position.X)
                / (game.Resolution.X * game.Resolution.Y);
        }
    }
}
