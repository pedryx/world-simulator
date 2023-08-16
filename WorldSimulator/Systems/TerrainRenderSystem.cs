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
    private readonly Effect terrainShader;
    private readonly SpriteBatch spriteBatch;
    private readonly Texture2D blankTexture;
    private readonly Game game;
    private readonly Camera camera;

    public TerrainRenderSystem(Game game, Camera camera)
    {
        terrainShader = game.GetResourceManager<Effect>()[GameWorld.TerrainDrawShader];
        spriteBatch = game.SpriteBatch;
        blankTexture = game.BlankTexture;

        this.game = game;
        this.camera = camera;

        terrainShader.Parameters["worldSize"].SetValue(GameWorld.Size.ToVector2());
        terrainShader.Parameters["resolution"].SetValue(Game.DefaultResolution);
    }

    public void Initialize(IECSWorld world) { }

    public void Update(float deltaTime)
    {
        terrainShader.Parameters["resolutionScale"].SetValue(game.ResolutionScale);
        terrainShader.Parameters["cameraPos"].SetValue(camera.Position);
        terrainShader.Parameters["cameraScale"].SetValue(camera.Scale);

        spriteBatch.Begin(effect: terrainShader);
        spriteBatch.Draw(blankTexture, game.GraphicsDevice.Viewport.Bounds, Color.White);
        spriteBatch.End();
    }
}
