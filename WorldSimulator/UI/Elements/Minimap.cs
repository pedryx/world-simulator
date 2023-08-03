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
    private const int borderSize = 10;

    private readonly Color viewFrameColor = Color.White;
    private readonly Color borderColor = new(230, 255, 186);

    private readonly GameWorld gameWorld;
    private readonly Camera camera;
    /// <summary>
    /// Scale factor for chunks which ensures that they fit the minimap.
    /// </summary>
    private readonly Vector2 scale;

    /// <summary>
    /// Custom spritebatch instance, which does not affect rendering of other UI elements.
    /// </summary>
    private SpriteBatch spriteBatch;

    public override Rectangle Bounds => new(Offset.ToPoint(), Size.ToPoint());

    public Vector2 Size { get; private set; }

    public Minimap(LevelState levelState, Vector2 size)
    {
        gameWorld = levelState.GameWorld;
        camera = levelState.Camera;
        Size = size;
        scale = (Size - new Vector2(borderSize * 2.0f)) / new Vector2(GameWorld.Size);

    }

    protected override void Initialize()
    {
        spriteBatch = new SpriteBatch(Game.GraphicsDevice);

        base.Initialize();
    }

    public override void Draw(Vector2 position, float deltaTime)
    {
        // render border
        spriteBatch.Begin
        (
            transformMatrix: Matrix.CreateScale(Game.ResolutionScale.X, Game.ResolutionScale.Y, 1.0f)
        );
        spriteBatch.Draw(Game.BlankTexture, new Rectangle(position.ToPoint(), Size.ToPoint()), borderColor);
        spriteBatch.End();

        // set scissor
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

        // render chunks
        foreach (var chunk in gameWorld.Chunks.SelectMany(c => c))
        {
            spriteBatch.Draw
            (
                chunk.GetComponent<Appearance>().Texture,
                chunk.GetComponent<Position>().Coordinates * scale + position + new Vector2(borderSize),
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
        DrawViewFrame(position);

        spriteBatch.End();
        // restore original scissor
        Game.GraphicsDevice.ScissorRectangle = original;

        base.Draw(position, deltaTime);
    }

    private void DrawViewFrame(Vector2 position)
    {
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
    }
}
