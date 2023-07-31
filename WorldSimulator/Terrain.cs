using Microsoft.Xna.Framework;

namespace WorldSimulator;
internal class Terrain
{
    public Color Color { get; private set; }
    /// <summary>
    /// Determine if structures can be builded on this terrain.
    /// </summary>
    public bool Buildable { get; private set; }
    /// <summary>
    /// Determine if entities could walk on this terrain.
    /// </summary>
    public bool Walkable { get; set; }
    /// <summary>
    /// Resource which can spawn on this terrain or null if no resource can spawn on this terrain.
    /// </summary>
    public Resource Resource { get; private set; }
    /// <summary>
    /// Chance that <see cref="Resource"/> will be spawned at specific pixel of this terrain.
    /// </summary>
    public float ResourceSpawnChance { get; private set; }

    /// <param name="buildable">Determine if structures can be builded on this terrain.</param>
    /// <param name="movable">Determine if entities could walk on this terrain.</param>
    /// <param name="resource">Resource which can spawn on this terrain or null if no resource can spawn on this terrain.</param>
    /// <param name="resourceSpawnChance">Chance that <see cref="Resource"/> will be spawned at specific pixel of this terrain.</param>
    public Terrain(Color color, bool buildable, bool movable, Resource resource = null, float resourceSpawnChance = 0.0f)
    {
        Color = color;
        Buildable = buildable;
        Walkable = movable;
        Resource = resource;
        ResourceSpawnChance = resourceSpawnChance;
    }
}

/// <summary>
/// Contains definitions of each terrain type.
/// </summary>
internal static class Terrains
{
    public readonly static Terrain Border = new(new Color(30, 30, 30), false, false);
    public readonly static Terrain DeepWater = new(new Color(48, 62, 255), false, false);
    public readonly static Terrain ShallowWater = new(new Color(71, 185, 255), false, false);
    public readonly static Terrain Beach = new(new Color(255, 253, 158), true, true);
    public readonly static Terrain Plain = new(new Color(85, 201, 90), true, true, Resources.Deer, 0.0001f);
    public readonly static Terrain Forest = new(new Color(25, 133, 30), true, true, Resources.Tree, 0.0002f);
    public readonly static Terrain Mountain = new(new Color(143, 143, 143), true, true, Resources.Rock, 0.001f);
    public readonly static Terrain HighMountain = new(new Color(255, 255, 255), false, true, Resources.Deposite, 0.002f);
}