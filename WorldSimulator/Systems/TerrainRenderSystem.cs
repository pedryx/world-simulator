using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
/// <summary>
/// System responsible for redering terrain.
/// </summary>
internal readonly struct TerrainRenderSystem : IECSSystem
{
    private readonly Effect terrainDrawShader;
    private readonly SpriteBatch spriteBatch;
    private readonly Texture2D blankTexture;
    private readonly Game game;
    private readonly Camera camera;

    public TerrainRenderSystem(Game game, Camera camera)
    {
        terrainDrawShader = game.GetResourceManager<Effect>()[GameWorld.TerrainDrawShader];
        spriteBatch = game.SpriteBatch;
        blankTexture = game.BlankTexture;

        this.game = game;
        this.camera = camera;

        terrainDrawShader.Parameters["worldSize"].SetValue(GameWorld.Size.ToVector2());
    }

    public void Initialize(IECSWorld world) { }

    public void Update(float deltaTime)
    {
        terrainDrawShader.Parameters["resolutionScale"].SetValue(game.ResolutionScale);
        terrainDrawShader.Parameters["resolution"].SetValue(game.Resolution);

        terrainDrawShader.Parameters["texOffset"].SetValue(Vector2.Zero);
        terrainDrawShader.Parameters["texOrigin"].SetValue(Game.DefaultResolution / 2.0f);

        terrainDrawShader.Parameters["scale"].SetValue(camera.Scale);
        terrainDrawShader.Parameters["offset"].SetValue(camera.Position);

        spriteBatch.Begin
        (
            effect: terrainDrawShader
        );
        spriteBatch.Draw(blankTexture, game.GraphicsDevice.Viewport.Bounds, Color.White);
        spriteBatch.End();
    }
}
