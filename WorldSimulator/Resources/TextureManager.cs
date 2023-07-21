using Microsoft.Xna.Framework.Graphics;

namespace WorldSimulator.Resources;
internal class TextureManager : ResourceManager<Texture2D>
{
    private readonly GraphicsDevice graphics;

    public TextureManager(GraphicsDevice graphics) 
        : base("png", "Textures")
    {
        this.graphics = graphics;
    }

    public override Texture2D Load(string file)
    {
        return Texture2D.FromFile(graphics, file);
    }
}
