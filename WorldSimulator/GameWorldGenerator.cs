using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Threading.Tasks;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator;
internal class GameWorldGenerator
{
    /// <summary>
    /// Size of world width and height in pixels.
    /// </summary>
    private const int worldSize = 4096;

    private readonly object spawnResourceLock = new();
    private readonly Game game;
    /// <summary>
    /// Noise used for terrain generation.
    /// </summary>
    private readonly Noise terrainNoise;
    /// <summary>
    /// Each layer prepresent different biome, biomes are placed based on generated height.
    /// </summary>
    private readonly BiomeLayer[] layers = new BiomeLayer[]
    {
        new BiomeLayer(0.30f, Terrains.DeepWater),
        new BiomeLayer(0.35f, Terrains.ShallowWater),
        new BiomeLayer(0.40f, Terrains.Beach),
        new BiomeLayer(0.50f, Terrains.Plains),
        new BiomeLayer(0.70f, Terrains.Forest),
        new BiomeLayer(1.00f, Terrains.Mountain),
    };
    private readonly LevelFactory factory;

    public GameWorldGenerator(Game game, LevelFactory factory)
    {
        this.game = game;
        this.factory = factory;

        // Noise parameters are fine-tuned.
        terrainNoise = new Noise(game.GenerateSeed(), 0.0008f, 0.0016f, 0.0032f);
    }

    public void Generate()
    {
        GenerateTerrain();
    }

    /// <summary>
    /// Generate terrain entity.
    /// </summary>
    private IEntity GenerateTerrain()
    {
        /*
         * 1. for each pixel:
         * 2.     generate random height value (based on terrain noise)
         * 3.     select biome based on height value
         * 4.     if resource can spawn at biome
         * 5.         try to spawn it
         * 
         * Biome-height pair is represented by Layer structure.
         */

        Color[] pixels = new Color[worldSize * worldSize];
        Texture2D terrainTexture = new(game.GraphicsDevice, worldSize, worldSize);
        IEntity terrain = factory.CreateTerrain(terrainTexture);

        Random random = new(game.GenerateSeed());
        float[] chances = new float[worldSize * worldSize];
        for (int i = 0; i < chances.Length; i++)
        {
            chances[i] = random.NextSingle();
        }

        Parallel.For(0, pixels.Length, i =>
        {
            int x = i % worldSize;
            int y = i / worldSize;
            float height = terrainNoise.CalculateValue(x, y);

            foreach (var layer in layers)
            {
                if (height < layer.Height)
                {
                    pixels[i] = layer.Terrain.Color;

                    if (layer.Terrain.Resource != null)
                    {
                        if (chances[i] < layer.Terrain.ResourceSpawnChance)
                        {
                            lock (spawnResourceLock)
                            {
                                factory.CreateResource
                                (
                                    layer.Terrain.Resource,
                                    new Vector2(x - worldSize / 2, y - worldSize / 2)
                                );
                            }
                        }
                    }

                    break;
                }
            }
        });

        terrainTexture.SetData(pixels);
        return terrain;
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
