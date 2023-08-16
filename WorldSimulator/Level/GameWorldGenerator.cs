using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WorldSimulator.Level;
internal class GameWorldGenerator
{
    private readonly Effect terrainGenShader;
    private readonly GraphicsDevice graphicsDevice;
    private readonly LevelFactory levelFactory;

    private int[] terrains;
    private Vector2[] resourcePositions;

    public GameWorldGenerator(Game game, LevelFactory levelFactory)
    {
        this.levelFactory = levelFactory;

        terrainGenShader = game.GetResourceManager<Effect>()[GameWorld.TerrainGenShader];
        graphicsDevice = game.GraphicsDevice;
    }

    public GameWorld Generate()
    {
        GenerateTerrain();
        SpawnResources();

        return new GameWorld();
    }

    private void GenerateTerrain()
    {
        int totalWorldSize = GameWorld.Size.X * GameWorld.Size.Y;

        StructuredBuffer terrainBuffer = new
        (
            graphicsDevice,
            typeof(int),
            totalWorldSize / GameWorldGrid.Distance,
            BufferUsage.None,
            ShaderAccess.None
        );
        StructuredBuffer resourceBuffer = new
        (
            graphicsDevice,
            typeof(Vector2),
            totalWorldSize,
            BufferUsage.None,
            ShaderAccess.None
        );
        StructuredBuffer sizeBuffer = new
        (
            graphicsDevice,
            typeof(int),
            2,
            BufferUsage.None,
            ShaderAccess.None
        );

        terrainGenShader.Parameters["worldSize"].SetValue(GameWorld.Size.ToVector2());
        terrainGenShader.Parameters["gridDistance"].SetValue(GameWorldGrid.Distance);
        terrainGenShader.Parameters["terrainBuffer"].SetValue(terrainBuffer);
        terrainGenShader.Parameters["resourceBuffer"].SetValue(resourceBuffer);
        terrainGenShader.Parameters["sizeBuffer"].SetValue(sizeBuffer);

        terrainGenShader.CurrentTechnique.Passes[0].ApplyCompute();
        graphicsDevice.DispatchCompute(totalWorldSize / GameWorldGrid.Distance, 1, 1);

        terrains = new int[terrainBuffer.ElementCount];
        terrainBuffer.GetData(terrains);

        int[] size = new int[sizeBuffer.ElementCount];
        sizeBuffer.GetData(size);

        resourcePositions = new Vector2[size[0]];
        resourceBuffer.GetData(resourcePositions);
    }

    private void SpawnResources()
    {
        foreach (var position in resourcePositions)
        {
            int index = ((int)position.Y * GameWorld.Size.X + (int)position.X) / GameWorldGrid.Distance;

            ResourceType type = TerrainTypes.GetTerrainType(terrains[index]).ResourceType;
            if (type != null)
                levelFactory.CreateResource(type, position);
        }
    }
}
