using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldSimulator.Extensions;
/// <summary>
/// Contains extension methods for the <see cref="Texture2D"/>.
/// </summary>
internal static class Texture2DExtensions
{
    /// <summary>
    /// Get width and height of the texture as <see cref="Vector2"/>.
    /// </summary>
    public static Vector2 GetSize(this Texture2D texture)
        => new(texture.Width, texture.Height);
}
