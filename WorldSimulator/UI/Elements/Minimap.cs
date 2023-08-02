﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Linq;

using WorldSimulator.Components;
using WorldSimulator.Extensions;
using WorldSimulator.Level;

namespace WorldSimulator.UI.Elements;
internal class Minimap : UIElement
{
    private const int viewFrameThickness = 3;

    private readonly Color viewFrameColor = Color.White;

    private readonly GameWorld gameWorld;
    private readonly Camera camera;
    private readonly Vector2 scale;

    private SpriteBatch spriteBatch;
    private Texture2D viewFrameTexture;

    public override Rectangle Bounds => new(Offset.ToPoint(), Size.ToPoint());

    public Vector2 Size { get; private set; }

    public Minimap(LevelState levelState, Vector2 size)
    {
        gameWorld = levelState.GameWorld;
        camera = levelState.Camera;
        Size = size;
        scale = Size / new Vector2(GameWorld.Size);
    }

    protected override void Initialize()
    {
        spriteBatch = Game.SpriteBatch;

        Vector2 viewFrameSize = Game.DefaultResolution * scale;
        viewFrameTexture = new Texture2D(Game.GraphicsDevice, (int)viewFrameSize.X, (int)viewFrameSize.Y);
        Color[] pixels = new Color[viewFrameTexture.Width * viewFrameTexture.Height];
        for (int i = 0; i < pixels.Length; i++)
        {
            int x = i % viewFrameTexture.Width;
            int y = i / viewFrameTexture.Width;

            if (x < viewFrameThickness || x >= viewFrameTexture.Width - viewFrameThickness 
                || y < viewFrameThickness || y >= viewFrameTexture.Height - viewFrameThickness)
            {
                pixels[i] = viewFrameColor;
            }
            else
            {
                pixels[i] = Color.Transparent;
            }
        }
        viewFrameTexture.SetData(pixels);

        base.Initialize();
    }

    public override void Draw(Vector2 position, float deltaTime)
    {
        var original = Game.GraphicsDevice.ScissorRectangle;
        Game.GraphicsDevice.ScissorRectangle = new Rectangle
        (
            (position * Game.ResolutionScale).ToPoint(),
            (Size * Game.ResolutionScale).ToPoint()
        );
        spriteBatch.Begin
        (
            rasterizerState: new RasterizerState() { ScissorTestEnable = true },
            transformMatrix: Matrix.CreateScale(Game.ResolutionScale.X, Game.ResolutionScale.Y, 1.0f)
        );

        // render chunks
        foreach (var chunk in gameWorld.Chunks.SelectMany(c => c))
        {
            spriteBatch.Draw
            (
                chunk.GetComponent<Appearance>().Texture,
                chunk.GetComponent<Position>().Coordinates * scale + position,
                null,
                Color.White,
                0.0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0.0f
            );
        }

        // render view frame
        spriteBatch.Draw
        (
            viewFrameTexture,
            camera.Position * scale + position,
            null,
            Color.White,
            0.0f,
            viewFrameTexture.GetSize() / 2.0f,
            1.0f / camera.Scale,
            SpriteEffects.None,
            0.0f
        );

        spriteBatch.End();
        Game.GraphicsDevice.ScissorRectangle = original;

        base.Draw(position, deltaTime);
    }
}
