using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Threading.Tasks;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Level;
internal class GameWorldGenerator
{
    /// <summary>
    /// Distance between nodes in movement grid. Also represent size of border.
    /// </summary>
    private const int GridDistance = 16;
    /// <summary>
    /// Width and height of one chunk in pixels.
    /// </summary>
    private const int chunkSize = 512;
    /// <summary>
    /// Chance that village will be spawned at specific pixel.
    /// </summary>
    private const float spawnVillageChance = 0.000001f;

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
    /// Values generated from noise are mapped to this array.
    /// </summary>
    private readonly TerrainType[] layersArray;
    private readonly object factoryLock = new();
    private readonly LevelFactory factory;

    /// <summary>
    /// Noise used for terrain generation.
    /// </summary>
    private Noise terrainNoise;
    /// <summary>
    /// Graph for path-finding for movement.
    /// </summary>
    private Graph graph;
    /// <summary>
    /// Maps pixels to terrains.
    /// </summary>
    private TerrainType[][] terrainMap;
    private IEntity[][] chunks;

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
        CreateGraph();

        return new(chunks, terrainMap, graph);
    }

    private void GenerateTerrain()
    {
        int chunkCount = GameWorld.Size / chunkSize;

        // Noise parameters are fine-tuned.
        terrainNoise = new Noise(game.GenerateSeed(), 0.0008f, 0.0016f, 0.0032f);
        terrainMap = new TerrainType[GameWorld.Size][];
        for (int i = 0; i < terrainMap.Length; i++)
        {
            terrainMap[i] = new TerrainType[GameWorld.Size];
        }
        chunks = new IEntity[chunkCount][];
        for (int i = 0; i < chunks.Length; i++)
        {
            chunks[i] = new IEntity[chunkCount];
        }

        for (int y = 0; y < chunkCount; y++)
        {
            for (int x = 0; x < chunkCount; x++)
            {
                chunks[y][x] = GenerateChunk(new Vector2(x, y) * chunkSize);

            }
        }
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
            if (x < GridDistance || x >= GameWorld.Size - GridDistance 
                || y < GridDistance || y >= GameWorld.Size - GridDistance)
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
            if (terrain.Buildable && chances[i] < spawnVillageChance)
            {
                lock(factoryLock)
                {
                    SpawnVillage(new Vector2(x, y));
                }
            }
            // try to spawn resource
            else if (terrain.Resource != null && chances[i] < terrain.ResourceSpawnChance)
            {
                lock (factoryLock)
                {
                    factory.CreateResource(terrain.Resource, new Vector2(x, y));
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

    /// <summary>
    /// Generate grid graph for path-finding for movement around the map.
    /// </summary>
    private void CreateGraph()
    {
        graph = new Graph();

        // its a grid so we just need to create edges for all adject grid points, GridDistance is also size of border
        for (int y = GridDistance; y < GameWorld.Size - GridDistance; y += GridDistance)
            {
            for (int x = GridDistance; x < GameWorld.Size - GridDistance; x += GridDistance)
            {
                if (!terrainMap[y][x].Walkable)
                    continue;

                if (terrainMap[x - GridDistance][y].Walkable)
                    graph.AddEdge(new Vector2(x, y), new Vector2(x, y - GridDistance));
                if (terrainMap[y + GridDistance][x].Walkable)
                    graph.AddEdge(new Vector2(x, y), new Vector2(x, y + GridDistance));
                if (terrainMap[y][x - GridDistance].Walkable)
                    graph.AddEdge(new Vector2(x, y), new Vector2(x - GridDistance, y));
                if (terrainMap[y][x + GridDistance].Walkable)
                    graph.AddEdge(new Vector2(x, y), new Vector2(x + GridDistance, y));
            }
        }
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
