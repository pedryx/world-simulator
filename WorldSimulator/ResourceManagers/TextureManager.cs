﻿using Microsoft.Xna.Framework.Graphics;

namespace WorldSimulator.ResourceManagers;
internal class TextureManager : ResourceManager<Texture2D>
{
    private readonly GraphicsDevice graphics;

    public TextureManager(GraphicsDevice graphics) 
        : base("png", "Textures")
    {
        this.graphics = graphics;
    }

    public override Texture2D Load(string file, string name)
    {
        var texture = Texture2D.FromFile(graphics, file);
        texture.Name = name;

        return texture;
    }
}
