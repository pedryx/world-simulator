using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldSimulator;
internal class TextureFactory
{
    private readonly GraphicsDevice graphicsDevice;

    public TextureFactory(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
    }

    /// <summary>
    /// Create rectangular frame.
    /// </summary>
    /// <param name="size">Width and height of the frame.</param>
    /// <param name="thickness">Size of frame border.</param>
    /// <param name="color">Frame color.</param>
    /// <returns>Texture of created frame.</returns>
    public Texture2D CreateRectangleFrame(Vector2 size, int thickness, Color color)
    {
        Texture2D texture = new(graphicsDevice, (int)size.X, (int)size.Y);
        Color[] pixels = new Color[texture.Width * texture.Height];

        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % texture.Width;
            int y = i / texture.Width;

            if (x < thickness || x >= texture.Width - thickness || y < thickness || y >= texture.Height - thickness)
                pixels[i] = color;
        }

        texture.SetData(pixels);
        return texture;
    }
}
