using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Threading.Tasks;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.GameStates.Level;

namespace WorldSimulator;
internal class WorldGenerator
{
    /// <summary>
    /// Size of world width and height in pixels.
    /// </summary>
    private const int worldSize = 4096;

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
        new BiomeLayer(0.3f, new Color(48, 62, 255)),   // deep water
        new BiomeLayer(0.35f, new Color(71, 185, 255)), // shallow water
        new BiomeLayer(0.4f, new Color(255, 253, 158)), // beaches
        new BiomeLayer(0.5f, new Color(85, 201, 90)),   // plains
        new BiomeLayer(0.7f, new Color(25, 133, 30)),   // forests
        new BiomeLayer(1.0f, new Color(143, 143, 143)), // mountains
    };
    private readonly LevelFactory factory;

    public WorldGenerator(Game game, LevelFactory factory)
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
         * 
         * Biome-height pair is represented by Layer structure. Each biome is represented by its color.
         */

        Color[] pixels = new Color[worldSize * worldSize];

        Parallel.For(0, pixels.Length, i =>
        {
            int x = i % worldSize;
            int y = i / worldSize;
            float height = terrainNoise.CalculateValue(x, y);

            for (int j = 0; j < layers.Length; j++)
            {
                if (height < layers[j].Height)
                {
                    pixels[i] = layers[j].Color;
                    break;
                }
            }
        });

        Texture2D texture = new(game.GraphicsDevice, worldSize, worldSize);
        texture.SetData(pixels);

        return factory.CreateBasicEntity(texture);
    }

    private struct BiomeLayer
    {
        public float Height { get; private set; }
        public Color Color { get; private set; }

        public BiomeLayer(float height, Color color)
        {
            Height = height;
            Color = color;
        }
    }
}
