using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Linq;

using WorldSimulator.Components;
using WorldSimulator.Level;

namespace WorldSimulator.UI.Elements;
/// <summary>
/// Represent a minimap of game world.
/// </summary>
internal class Minimap : UIElement
{
    private const int viewFrameThickness = 3;
    private const int borderSize = 15;

    private readonly Color viewFrameColor = Color.White;
    private readonly Color borderColor = Color.Black;

    private readonly Camera camera;
    /// <summary>
    /// Scale factor for chunks which ensures that they fit the minimap.
    /// </summary>
    private readonly Vector2 scale;
    private readonly Texture2D borderTexture;
    private readonly Game game;

    /// <summary>
    /// Custom spritebatch instance, which does not affect rendering of other UI elements.
    /// </summary>
    private SpriteBatch spriteBatch;
    private Effect terrainDrawShader;

    public override Rectangle Bounds => new(Offset.ToPoint(), Size.ToPoint());

    public Vector2 Size { get; private set; }

    public Minimap(LevelState levelState, Vector2 size, Texture2D borderTexture)
    {
        camera = levelState.Camera;
        game = levelState.Game;

        Size = size;
        scale = (Size - new Vector2(borderSize * 2.0f)) / GameWorld.Size.ToVector2();

        this.borderTexture = borderTexture;
    }

    protected override void Initialize()
    {
        spriteBatch = new SpriteBatch(Game.GraphicsDevice);
        terrainDrawShader = Game.GetResourceManager<Effect>()[GameWorld.TerrainDrawShader];

        base.Initialize();
    }

    public override void Draw(Vector2 position, float deltaTime)
    {
        DrawTerrain(position);
        DrawViewFrame(position);
        DrawBorder(position);

        base.Draw(position, deltaTime);
    }

    private void DrawTerrain(Vector2 position)
    {
        terrainDrawShader.Parameters["resolutionScale"].SetValue(game.ResolutionScale);
        terrainDrawShader.Parameters["resolution"].SetValue(game.Resolution);

        Vector2 textureOffset = (position + new Vector2(borderSize)) * game.ResolutionScale;
        Vector2 size = Size - new Vector2(2 * borderSize);
        Vector2 origin = Vector2.Zero;

        terrainDrawShader.Parameters["texOffset"].SetValue(textureOffset);
        terrainDrawShader.Parameters["texOrigin"].SetValue(origin);

        terrainDrawShader.Parameters["scale"].SetValue(size.X / GameWorld.Size.X);
        terrainDrawShader.Parameters["offset"].SetValue(Vector2.Zero);

        size *= game.ResolutionScale;

        spriteBatch.Begin
        (
            effect: terrainDrawShader
        );
        spriteBatch.Draw(game.BlankTexture, new Rectangle(textureOffset.ToPoint(), size.ToPoint()), Color.White);
        spriteBatch.End();
    }

    private void DrawViewFrame(Vector2 position)
    {
        var original = Game.GraphicsDevice.ScissorRectangle;
        Game.GraphicsDevice.ScissorRectangle = new Rectangle
        (
            (position * Game.ResolutionScale + new Vector2(borderSize)).ToPoint(),
            (Size * Game.ResolutionScale - new Vector2(2.0f * borderSize)).ToPoint()
        );
        spriteBatch.Begin
        (
            rasterizerState: new RasterizerState() { ScissorTestEnable = true },
            transformMatrix: Matrix.CreateScale(Game.ResolutionScale.X, Game.ResolutionScale.Y, 1.0f)
        );

        Vector2 size = (Game.DefaultResolution * scale) / camera.Scale;
        Vector2 offset = position + camera.Position * scale - size / 2.0f + new Vector2(borderSize);

        // top
        spriteBatch.Draw
        (
            Game.BlankTexture,
            new Rectangle
            (
                offset.ToPoint(),
                new Point((int)size.X, viewFrameThickness)
            ),
            viewFrameColor
        );
        // bottom
        spriteBatch.Draw
        (
            Game.BlankTexture,
            new Rectangle
            (
                offset.ToPoint() + new Point(0, (int)size.Y - viewFrameThickness),
                new Point((int)size.X, viewFrameThickness)
            ),
            viewFrameColor
        );
        // left
        spriteBatch.Draw
        (
            Game.BlankTexture,
            new Rectangle
            (
                offset.ToPoint(),
                new Point(viewFrameThickness, (int)size.Y)
            ),
            viewFrameColor
        );
        // right
        spriteBatch.Draw
        (
            Game.BlankTexture,
            new Rectangle
            (
                offset.ToPoint() + new Point((int)size.X - viewFrameThickness, 0),
                new Point(viewFrameThickness, (int)size.Y)
            ),
            viewFrameColor
        );

        spriteBatch.End();
        Game.GraphicsDevice.ScissorRectangle = original;
    }

    private void DrawBorder(Vector2 position)
    {
        spriteBatch.Begin
        (
            transformMatrix: Matrix.CreateScale(Game.ResolutionScale.X, Game.ResolutionScale.Y, 1.0f)
        );
        spriteBatch.Draw(borderTexture, new Rectangle(position.ToPoint(), Size.ToPoint()), borderColor);
        spriteBatch.End();
    }
}
