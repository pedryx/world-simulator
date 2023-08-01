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

    private readonly Game game;
    /// <summary>
    /// Each layer prepresent different biome, biomes are placed based on generated height.
    /// </summary>
    private readonly BiomeLayer[] layers = new BiomeLayer[]
    {
        new BiomeLayer(0.30f, Terrains.DeepWater),
        new BiomeLayer(0.35f, Terrains.ShallowWater),
        new BiomeLayer(0.40f, Terrains.Beach),
        new BiomeLayer(0.50f, Terrains.Plain),
        new BiomeLayer(0.70f, Terrains.Forest),
        new BiomeLayer(0.80f, Terrains.Mountain),
        new BiomeLayer(1.00f, Terrains.HighMountain),
    };
    /// <summary>
    /// Values generated from noise are mapped to this array.
    /// </summary>
    private readonly Terrain[] layersArray;
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
    private Terrain[][] terrainMap = new Terrain[GameWorld.Size][];

    public GameWorldGenerator(Game game, LevelFactory factory)
    {
        this.game = game;
        this.factory = factory;

        // create mapping of noise values to terrains
        layersArray = new Terrain[20];
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
        // Noise parameters are fine-tuned.
        terrainNoise = new Noise(game.GenerateSeed(), 0.0008f, 0.0016f, 0.0032f);
        graph = new Graph();
        terrainMap = new Terrain[GameWorld.Size][];
        for (int i = 0; i < terrainMap.Length; i++)
        {
            terrainMap[i] = new Terrain[GameWorld.Size];
        }

        GenerateTerrain();
        CreateGraph();

        //factory.CreateVillager(new Vector2(WorldSize) / 2.0f);

        return new(terrainMap, graph);
    }

    private void GenerateTerrain()
    {
        int chunkCount = GameWorld.Size / chunkSize;

        for (int y = 0; y < chunkCount; y++)
        {
            for (int x = 0; x < chunkCount; x++)
            {
                GenerateChunk(new Point(x * chunkSize, y * chunkSize));
            }
        }
    }

    /// <summary>
    /// Generate one chunk of terrain.
    /// </summary>
    private void GenerateChunk(Point offset)
    {
        Color[] pixels = new Color[chunkSize * chunkSize];
        Texture2D terrainTexture = new(game.GraphicsDevice, chunkSize, chunkSize);
        IEntity terrain = factory.CreateTerrain(terrainTexture, offset.ToVector2());

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
            int x = i % chunkSize + offset.X;
            int y = i / chunkSize + offset.Y;

            // set border biome, GridDistance is also size of border
            if (x < GridDistance || x >= GameWorld.Size - GridDistance 
                || y < GridDistance || y >= GameWorld.Size - GridDistance)
            {
                pixels[i] = Terrains.Border.Color;
                terrainMap[y][x] = Terrains.Border;
                return;
            }

            // set terrain according to generated noise value
            Terrain terrain = layersArray[(int)(terrainNoise.CalculateValue(x, y) * layersArray.Length)];
            pixels[i] = terrain.Color;
            terrainMap[y][x] = terrain;
            
            // try to spawn resource
            if (terrain.Resource != null && chances[i] < terrain.ResourceSpawnChance)
            {
                lock (factoryLock)
                {
                    factory.CreateResource(terrain.Resource, new Vector2(x, y));
                }
            }
        });

        terrainTexture.SetData(pixels);
    }

    /// <summary>
    /// Generate grid graph for path-finding for movement around the map.
    /// </summary>
    private void CreateGraph()
    {
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
        public Terrain Terrain { get; private set; }

        public BiomeLayer(float height, Terrain terrain)
        {
            Height = height;
            Terrain = terrain;
        }
    }
}
