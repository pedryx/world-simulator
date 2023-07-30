using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.Extensions;

namespace WorldSimulator;
/// <summary>
/// Represent renderable sprite.
/// </summary>
internal struct Sprite
{
    public Texture2D Texture;
    public Vector2 Position;
    public Rectangle? SourceRectangle;
    public Color Color = Color.White;
    public float Rotation;
    public Vector2 Origin;
    public float Scale = 1.0f;
    public SpriteEffects Effects;
    public float LayerDepth;

    public Vector2 GetSize()
    {
        if (Texture == null)
            return Vector2.Zero;

        Vector2 size = SourceRectangle.HasValue ?
            SourceRectangle.Value.Size.ToVector2() :
            Texture.GetSize();
        return size * Scale;
    }

    public Sprite() { }

    public Sprite(Texture2D texture, float scale = 1.0f)
    {
        Texture = texture;
        Scale = scale;
        Origin = texture.GetSize() / 2.0f;
    }
}
