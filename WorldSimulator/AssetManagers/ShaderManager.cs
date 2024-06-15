using Microsoft.Xna.Framework.Graphics;

using System.IO;

namespace WorldSimulator.AssetManagers;
internal class ShaderManager : AssetManager<Effect>
{
    private readonly GraphicsDevice graphics;

    public ShaderManager(GraphicsDevice graphics)
        : base("mgfx", "Shaders")
    {
        this.graphics = graphics;
    }

    public override Effect Load(string file, string name)
    {
        return new Effect(graphics, File.ReadAllBytes(file))
        {
            Name = name,
        };
    }
}
