using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Systems;
public readonly struct RenderSystem : IEntityProcessor<Transform, Appearance>
{
    private readonly SpriteBatch spriteBatch;
    private readonly Camera camera;

    public RenderSystem(SpriteBatch spriteBatch, Camera camera)
    {
        this.spriteBatch = spriteBatch;
        this.camera = camera;
    }

    void IEntityProcessor.PreUpdate(float deltaTime)
    {
        spriteBatch.Begin
        (
            sortMode: SpriteSortMode.FrontToBack,
            transformMatrix: camera.GetTransformMatrix()
        );
    }

    public void Process(ref Transform transform, ref Appearance appearance, float deltaTime)
    {
        if (!appearance.Visible)
            return;

        spriteBatch.Draw(appearance.Sprite, transform.Position, transform.Scale, transform.Rotation);
    }

    void IEntityProcessor.PostUpdate(float deltaTime)
    {
        spriteBatch.End();
    }
}