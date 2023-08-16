using KdTree;
using KdTree.Math;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Villages;

namespace WorldSimulator.Level;
internal class GameWorldGenerator
{
    private const int shaderThreadGroupSize = 1024;
    private const int villageJitterSize = 2048;

    private readonly Effect terrainGenShader;
    private readonly GraphicsDevice graphicsDevice;
    private readonly LevelFactory levelFactory;
    private readonly Game game;

    private Vector2[] resourcePositions;
        
    private int[] terrains;
    private IDictionary<ResourceType, KdTree<float, IEntity>> resources;
    private List<Village> villages;

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

        return new GameWorld(terrains, resources, villages);
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

        terrains = new int[terrainBuffer.ElementCount];
        terrainBuffer.GetData(terrains);

        int[] size = new int[sizeBuffer.ElementCount];
        sizeBuffer.GetData(size);

        resourcePositions = new Vector2[size[0]];
        resourceBuffer.GetData(resourcePositions);
    }

    private void SpawnResources()
    {
        resources = ResourceTypes.GetAllTypes().ToDictionary
        (
            type => type,
            type => new KdTree<float, IEntity>(2, new FloatMath())
        );

        foreach (var position in resourcePositions)
        {
            Vector2 point = GameWorldGrid.GetClosestPoint(position);
            int index = ((int)point.Y * GameWorld.Size.X + (int)point.X) / GameWorldGrid.Distance;

            TerrainType terrainType = TerrainTypes.GetTerrainType(terrains[index]);
            ResourceType resourceType = terrainType.ResourceType;

            /* 
             * because [psitions were calculated on graphics card, on biome transitions they could result onto
             * neighbor biome
             */
            if (resourceType == null)
                continue;

            IEntity entity = levelFactory.CreateResource(resourceType, position);
            resources[resourceType].Add(position.ToFloat(), entity);
        }
        resourcePositions = null;
    }

    private void SpawnVillages()
    {
        villages = new List<Village>();
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

                    Vector2 point = GameWorldGrid.GetClosestPoint(position);
                    int index = ((int)point.Y * GameWorld.Size.X + (int)point.X) / GameWorldGrid.Distance;
                    TerrainType terrainType = TerrainTypes.GetTerrainType(terrains[index]);

                    if (terrainType.Buildable)
                        break;
                }
       

                SpawnVillage(position);
            }
        }
    }

    private void SpawnVillage(Vector2 position)
    {
        villages.Add(new Village());
        levelFactory.CreateMainBuilding(position);

        for (int i = 0; i < 3; i++)
        {
            IEntity villager = levelFactory.CreateVillager(position, villages.Count - 1);
            villages.Last().AddVillager(villager);
        }
    }
}
