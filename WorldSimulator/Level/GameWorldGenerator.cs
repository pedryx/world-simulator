using KdTree;
using KdTree.Math;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Level;
internal class GameWorldGenerator
{
    /// <summary>
    /// Size of game world border.
    /// </summary>
    private const int border = 16;
    /// <summary>
    /// Width and height of one chunk.
    /// </summary>
    private const int chunkSize = 512;
    /// <summary>
    /// Chance that village will be spawned at specific pixel.
    /// </summary>
    private const float villageSpawnChance = 0.000001f;

    private readonly Game game;
    /// <summary>
    /// Each layer prepresent different biome, biomes are placed based on generated height.
    /// </summary>
    private readonly BiomeLayer[] layers = new BiomeLayer[]
    {
        new BiomeLayer(0.30f, TerrainTypes.DeepWater),
        new BiomeLayer(0.35f, TerrainTypes.ShallowWater),
        new BiomeLayer(0.40f, TerrainTypes.Beach),
        new BiomeLayer(0.50f, TerrainTypes.Plain),
        new BiomeLayer(0.70f, TerrainTypes.Forest),
        new BiomeLayer(0.80f, TerrainTypes.Mountain),
        new BiomeLayer(1.00f, TerrainTypes.HighMountain),
    };
    /// <summary>
    /// Maps noise values into terrain types.
    /// </summary>
    private readonly TerrainType[] layersArray;
    private readonly LevelFactory factory;
    private readonly object factoryLock = new();

    /// <summary>
    /// Noise used for terrain generation.
    /// </summary>
    private Noise terrainNoise;
    /// <summary>
    /// Maps pixels to terrains.
    /// </summary>
    private TerrainType[][] terrainMap;
    private IEnumerable<IEntity> chunks;
    private IDictionary<ResourceType, KdTree<float, IEntity>> resources;

    public GameWorldGenerator(Game game, LevelFactory factory)
    {
        this.game = game;
        this.factory = factory;

        // create mapping of noise values to terrains
        layersArray = new TerrainType[20];
        for (int i = 0; i < layersArray.Length; i++)
        {
            foreach (var layer in layers)
            {
                if ((float)(i + 1) / layersArray.Length <= layer.Height)
                {
                    layersArray[i] = layer.Terrain;
                    break;
                }
            }
        }
    }

    public GameWorld Generate()
    {
        GenerateTerrain();

        return new(chunks, terrainMap, resources);
    }

    private void GenerateTerrain()
    {
        int chunkCount = GameWorld.Size / chunkSize;

        // Prepare data structures for game world.
        // Noise parameters are fine-tuned.
        terrainNoise = new Noise(game.GenerateSeed(), 0.0008f, 0.0016f, 0.0032f);
        terrainMap = new TerrainType[GameWorld.Size][];
        for (int i = 0; i < terrainMap.Length; i++)
        {
            terrainMap[i] = new TerrainType[GameWorld.Size];
        }
        List<IEntity> chunks = new();
        resources = ResourceTypes.GetAllTypes().ToDictionary
        (
            type => type,
            type => new KdTree<float, IEntity>(2, new FloatMath())
        );

        // Generate game world chunks.
        for (int y = 0; y < chunkCount; y++)
        {
            for (int x = 0; x < chunkCount; x++)
            {
                chunks.Add(GenerateChunk(new Vector2(x, y) * chunkSize));
            }
        }

        this.chunks = chunks; 
    }

    /// <summary>
    /// Generate one chunk of terrain.
    /// </summary>
    private IEntity GenerateChunk(Vector2 offset)
    {
        Color[] pixels = new Color[chunkSize * chunkSize];
        Texture2D terrainTexture = new(game.GraphicsDevice, chunkSize, chunkSize);
        IEntity entity = factory.CreateTerrain(terrainTexture, offset);

        /*
         * Pregenerate random value for each pixel, later this value will be used to determine if resource should be
         * spawned at pixel i.
        */
        Random random = new(game.GenerateSeed());
        float[] chances = new float[chunkSize * chunkSize];
        for (int i = 0; i < chances.Length; i++)
        {
            chances[i] = random.NextSingle();
        }

        // process each pixel in parallel
        terrainNoise.Begin();
        Parallel.For(0, pixels.Length, i =>
        {
            int x = i % chunkSize + (int)offset.X;
            int y = i / chunkSize + (int)offset.Y;

            // set border biome, GridDistance is also size of border
            if (x < border || x >= GameWorld.Size - border || y < border || y >= GameWorld.Size - border)
            {
                pixels[i] = TerrainTypes.Border.Color;
                terrainMap[y][x] = TerrainTypes.Border;
                return;
            }

            // set terrain according to generated noise value
            TerrainType terrain = layersArray[(int)(terrainNoise.CalculateValue(x, y) * layersArray.Length)];
            pixels[i] = terrain.Color;
            terrainMap[y][x] = terrain;
            
            // try to spawn village
            if (terrain.Buildable && chances[i] < villageSpawnChance)
            {
                lock(factoryLock)
                {
                    SpawnVillage(new Vector2(x, y));
                }
            }
            // try to spawn resource
            else if (terrain.ResourceType != null && chances[i] < terrain.ResourceSpawnChance)
            {
                lock (factoryLock)
                {
                    IEntity resource = factory.CreateResource(terrain.ResourceType, new Vector2(x, y));
                    resources[terrain.ResourceType].Add(new float[] { x, y }, resource);
                }
            }
        });

        terrainTexture.SetData(pixels);
        return entity;
    }

    private void SpawnVillage(Vector2 position)
    {
        factory.CreateMainBuilding(position);

        factory.CreateVillager(position);
        factory.CreateVillager(position);
        factory.CreateVillager(position);
    }

    private struct BiomeLayer
    {
        public float Height { get; private set; }
        public TerrainType Terrain { get; private set; }

        public BiomeLayer(float height, TerrainType terrain)
        {
            Height = height;
            Terrain = terrain;
        }
    }
}
