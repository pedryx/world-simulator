using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldSimulator.Extensions;
/// <summary>
/// Contains extension methods for <see cref="SpriteBatch"/>.
/// </summary>
public static class SpriteBatchExtensions
{
    public static void Draw
    (
        this SpriteBatch spriteBatch,
        Sprite sprite,
        Vector2 positionOffset = default,
        float scaleOffset = 1.0f,
        float rotationOffset = 0.0f
    )
    {
        spriteBatch.Draw
        (
            sprite.Texture,
            sprite.Position + positionOffset,
            sprite.SourceRectangle,
            sprite.Color,
            sprite.Rotation + rotationOffset,
            sprite.Origin,
            sprite.Scale * scaleOffset,
            sprite.Effects,
            sprite.LayerDepth
        );
    }
}