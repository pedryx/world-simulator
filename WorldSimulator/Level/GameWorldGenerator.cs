using KdTree;
using KdTree.Math;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.Linq;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Level;
internal class GameWorldGenerator
{
    private const int shaderThreadGroupSize = 1024;

    private readonly Effect terrainGenShader;
    private readonly GraphicsDevice graphicsDevice;
    private readonly LevelFactory levelFactory;

    private Vector2[] resourcePositions;

    private int[] terrains;
    private IDictionary<ResourceType, KdTree<float, IEntity>> resources;

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

        return new GameWorld(terrains, resources);
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
}
