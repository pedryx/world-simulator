using KdTree;
using KdTree.Math;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Villages;

namespace WorldSimulator.Level;
internal class GameWorldGenerator
{
    /// <summary>
    /// Number of threads in one thread group from compute shader.
    /// </summary>
    private const int shaderThreadGroupSize = 1024;
    private const int villageJitterSize = 2048;

    private readonly Effect terrainGenShader;
    private readonly GraphicsDevice graphicsDevice;
    private readonly LevelFactory levelFactory;
    private readonly Game game;

    /// <summary>
    /// Positions where resources will be spawned.
    /// </summary>
    private Vector2[] resourcePositions;
        
    private GameWorld gameWorld;

    public GameWorldGenerator(Game game, LevelFactory levelFactory)
    {
        this.levelFactory = levelFactory;
        this.game = game;

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
    }

    private void SpawnResources()
    {
        foreach (var position in resourcePositions)
        {
            Terrain terrainType = gameWorld.GetTerrain(position);
            ResourceType resourceType = terrainType.ResourceType;

            // Because positions were calculated on the GPU, resources at the edges can end up in a different biome.
            if (resourceType == null)
                continue;

            IEntity entity = levelFactory.CreateResource(resourceType, position);
            gameWorld.AddResource(resourceType, entity, position);
        }
        resourcePositions = null;
    }

    private void SpawnVillages()
    {
        // SpawnVillage(GameWorld.Size.ToVector2() / 2.0f); return;

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

                    if (gameWorld.GetTerrain(position).Buildable)
                        break;
                }

                SpawnVillage(position);
            }
        }
    }

    private void SpawnVillage(Vector2 position)
    {
        Debug.Assert(GameWorld.Bounds.Contains(position));

        Village village = new(game, gameWorld);
        int id = gameWorld.AddVillage(village);

        IEntity mainBuilding = levelFactory.CreateMainBuilding(position);
        village.AddBuilding(mainBuilding);

        IEntity stockpile = levelFactory.CreateStockpile(village.GetNextBuildingPosition());
        village.AddStockpile(stockpile);

        IEntity woodcutterHut = levelFactory.CreateWoodcutterHut(village.GetNextBuildingPosition());
        village.AddResourceProcessingBuilding(ResourceType.Tree, woodcutterHut);

        IEntity villager = levelFactory.CreateVillager(position, id);
        village.AddVillager(villager);
    }
}
