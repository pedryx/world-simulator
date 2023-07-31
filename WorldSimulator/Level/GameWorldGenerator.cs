using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Threading.Tasks;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Level;
internal class GameWorldGenerator
{
    /// <summary>
    /// Size of world width and height in pixels.
    /// </summary>
    public const int WorldSize = 4096;
    public const int BorderSize = 16;
    /// <summary>
    /// Distance between nodes in movement grid.
    /// </summary>
    public const int GridDistance = 16;

    private readonly object spawnResourceLock = new();
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
    private readonly LevelFactory factory;

    /// <summary>
    /// Noise used for terrain generation.
    /// </summary>
    private Noise terrainNoise;
    private Graph graph;
    private Terrain[][] terrainMap = new Terrain[WorldSize][];

    public GameWorldGenerator(Game game, LevelFactory factory)
    {
        this.game = game;
        this.factory = factory;
    }

    public GameWorld Generate()
    {
        // Noise parameters are fine-tuned.
        terrainNoise = new Noise(game.GenerateSeed(), 0.0008f, 0.0016f, 0.0032f);
        graph = new Graph();
        terrainMap = new Terrain[WorldSize][];
        for (int i = 0; i < terrainMap.Length; i++)
        {
            terrainMap[i] = new Terrain[WorldSize];
        }

        GenerateTerrain();
        GenerateGraph();

        return new(terrainMap, graph);
    }

    /// <summary>
    /// Generate terrain entity.
    /// </summary>
    private IEntity GenerateTerrain()
    {
        Color[] pixels = new Color[WorldSize * WorldSize];
        Texture2D terrainTexture = new(game.GraphicsDevice, WorldSize, WorldSize);
        IEntity terrain = factory.CreateTerrain(terrainTexture);

        /*
         * pregenerate random value for each pixel, later this value will be used to determine if resource should be
         * spawned
        */
        Random random = new(game.GenerateSeed());
        float[] chances = new float[WorldSize * WorldSize];
        for (int i = 0; i < chances.Length; i++)
        {
            chances[i] = random.NextSingle();
        }

        // process each pixel in parallel
        Parallel.For(0, pixels.Length, i =>
        {
            int x = i % WorldSize;
            int y = i / WorldSize;

            // set border biome
            if (x < BorderSize || x >= WorldSize - BorderSize || y < BorderSize || y >= WorldSize - BorderSize)
            {
                pixels[i] = Terrains.Border.Color;
                terrainMap[y][x] = Terrains.Border;
                return;
            }

            float height = terrainNoise.CalculateValue(x, y);

            // determine biome of pixel
            foreach (var layer in layers)
            {
                if (height < layer.Height)
                {
                    // set biome to pixel
                    pixels[i] = layer.Terrain.Color;
                    terrainMap[y][x] = layer.Terrain;

                    // try to spawn resource
                    if (layer.Terrain.Resource != null && chances[i] < layer.Terrain.ResourceSpawnChance)
                    {
                        lock (spawnResourceLock)
                        {
                            factory.CreateResource(layer.Terrain.Resource, new Vector2(x, y));
                        }
                    }

                    break;
                }
            }
        });

        terrainTexture.SetData(pixels);
        return terrain;
    }

    /// <summary>
    /// Generate grid graph for path-finding for movement around the map.
    /// </summary>
    private void GenerateGraph()
    {
        for (int x = BorderSize; x < WorldSize - BorderSize; x += GridDistance)
        {
            for (int y = BorderSize; y < WorldSize - BorderSize; y += GridDistance)
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
