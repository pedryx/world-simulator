using Microsoft.Xna.Framework;

namespace WorldSimulator;
internal class Terrain
{
    public Color Color { get; private set; }
    public bool Buildable { get; private set; }
    public Resource Resource { get; private set; }
    public float ResourceSpawnChance { get; private set; }

    public Terrain(Color color, bool buildable, Resource resource = null, float resourceSpawnChance = 0.0f)
    {
        Color = color;
        Buildable = buildable;
        Resource = resource;
        ResourceSpawnChance = resourceSpawnChance;
    }
}

internal static class Terrains
{
    public readonly static Terrain DeepWater = new(new Color(48, 62, 255), false);
    public readonly static Terrain ShallowWater = new(new Color(71, 185, 255), false);
    public readonly static Terrain Beach = new(new Color(255, 253, 158), true);
    public readonly static Terrain Plains = new(new Color(85, 201, 90), true);
    public readonly static Terrain Forest = new(new Color(25, 133, 30), true, Resources.Tree, 0.0003f);
    public readonly static Terrain Mountain = new(new Color(143, 143, 143), true, Resources.Rock, 0.002f);
    public readonly static Terrain HighMountain = new(new Color(255, 255, 255), true, Resources.Deposite, 0.004f);
}