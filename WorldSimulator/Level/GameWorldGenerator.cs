using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Level;
internal class GameWorldGenerator
{
    /// <summary>
    /// Number of threads in one thread group from compute shader.
    /// </summary>
    private const int shaderThreadGroupSize = 1024;
    private const int villageJitterSize = 2048;

    /// <summary>
    /// Global world data which can be reused by different instances of <see cref="Game"/>.
    /// </summary>
    private static GameWorldData globalWorldData;

    private readonly Effect terrainGenShader;
    private readonly GraphicsDevice graphicsDevice;
    private readonly Game game;
    private readonly LevelFactory levelFactory;
    private readonly bool useGlobalWorldData;

    /// <summary>
    /// Positions where resources will be spawned.
    /// </summary>
    private Vector2[] resourcePositions;
        
    private GameWorld gameWorld;

    /// <param name="useGlobalWorldData">
    /// Determine if generator should use global world data. If no global world data yet exist, generator will generate
    /// them.
    /// </param>
    public GameWorldGenerator(Game game, LevelFactory levelFactory, bool useGlobalWorldData)
    {
        this.levelFactory = levelFactory;
        this.game = game;
        this.useGlobalWorldData = useGlobalWorldData;

        terrainGenShader = game.GetResourceManager<Effect>()[GameWorld.TerrainGenShader];
        graphicsDevice = game.GraphicsDevice;
    }

    public GameWorld Generate()
    {
        GenerateTerrain();
        SpawnResources();
        SpawnVillages();

        GameWorld gameWorld = this.gameWorld;
        this.gameWorld = null;

        return gameWorld;
    }

    private void GenerateTerrain()
    {
        if (globalWorldData != null && useGlobalWorldData) 
        {
            gameWorld = new GameWorld(globalWorldData.TerrainData);
            resourcePositions = globalWorldData.ResourcePositions;
            return;
        }

        StructuredBuffer terrainBuffer = new
        (
            graphicsDevice,
            typeof(int),
            GameWorld.TotalSize / GameWorldGrid.Distance,
            BufferUsage.None,
            ShaderAccess.None
        );
        StructuredBuffer resourceBuffer = new
        (
            graphicsDevice,
            typeof(Vector2),
            GameWorld.TotalSize,
            BufferUsage.None,
            ShaderAccess.None
        );
        StructuredBuffer sizeBuffer = new
        (
            graphicsDevice,
            typeof(int),
            1,
            BufferUsage.None,
            ShaderAccess.None
        );

        terrainGenShader.Parameters["worldSize"].SetValue(GameWorld.Size.ToVector2());
        terrainGenShader.Parameters["gridDistance"].SetValue(GameWorldGrid.Distance);
        terrainGenShader.Parameters["terrainBuffer"].SetValue(terrainBuffer);
        terrainGenShader.Parameters["resourceBuffer"].SetValue(resourceBuffer);
        terrainGenShader.Parameters["sizeBuffer"].SetValue(sizeBuffer);

        terrainGenShader.CurrentTechnique.Passes[0].ApplyCompute();
        graphicsDevice.DispatchCompute((GameWorld.TotalSize / GameWorldGrid.Distance) / shaderThreadGroupSize, 1, 1);

        int[] terrains = new int[terrainBuffer.ElementCount];
        terrainBuffer.GetData(terrains);
        gameWorld = new GameWorld(terrains);

        int[] size = new int[sizeBuffer.ElementCount];
        sizeBuffer.GetData(size);

        resourcePositions = new Vector2[size[0]];
        resourceBuffer.GetData(resourcePositions);

        globalWorldData = new GameWorldData(terrains, resourcePositions);
    }

    private void SpawnResources()
    {
        foreach (var position in resourcePositions)
        {
            TerrainType terrainType = gameWorld.GetTerrain(position);
            ResourceType resourceType = terrainType.ResourceType;

            // Because positions were calculated on the GPU, resources at the edges can end up in a different biome.
            if (resourceType == null)
                continue;

            IEntity entity = null;
            if (resourceType == ResourceType.Tree)
                entity = levelFactory.CreateTree(position);
            else if (resourceType == ResourceType.Rock)
                entity = levelFactory.CreateRock(position);
            else if (resourceType == ResourceType.Deposit)
                entity = levelFactory.CreateDeposit(position);
            else if (resourceType == ResourceType.Deer)
                entity = levelFactory.CreateDeer(position);
            else
                throw new InvalidOperationException("Resource type not supported!");

            gameWorld.AddResource(resourceType, entity, position);
        }
        resourcePositions = null;
    }

    private void SpawnVillages()
    {
        // factory.CreateVillage(GameWorld.Size.ToVector2() / 2.0f); return;

        Random random = new(game.GenerateSeed());

        for (int y = 0; y < GameWorld.Size.Y; y += villageJitterSize)
        {
            for (int x = 0; x < GameWorld.Size.X; x += villageJitterSize)
            {
                Vector2 position;

                while (true)
                {
                    position = new
                    (
                        random.NextSingle(x, x + villageJitterSize),
                        random.NextSingle(y, y + villageJitterSize)
                    );

                    if (gameWorld.GetTerrain(position).CanBuild)
                        break;
                }

                levelFactory.CreateVillage(position);
            }
        }
    }
}
