using Microsoft.Xna.Framework.Graphics;

using System.IO;

namespace WorldSimulator.ContentResources;
internal class ShaderManager : ResourceManager<Effect>
{
    private readonly GraphicsDevice graphics;

    public ShaderManager(GraphicsDevice graphics) 
        : base("mgfx", "Shaders")
    {
        this.graphics = graphics;
    }

    public override Effect Load(string file)
    {
        return new Effect(graphics, File.ReadAllBytes(file));
    }
}
