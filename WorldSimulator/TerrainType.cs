using Microsoft.Xna.Framework;

using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator;
internal class TerrainType
{
    public Color Color { get; init; }
    /// <summary>
    /// Determine if structures can be builded on this terrain.
    /// </summary>
    public bool Buildable { get; init; }
    /// <summary>
    /// Determine if entities could walk on this terrain.
    /// </summary>
    public bool Walkable { get; init; }
    /// <summary>
    /// Resource which can spawn on this terrain or null if no resource can spawn on this terrain.
    /// </summary>
    public ResourceType Resource { get; init; }
    /// <summary>
    /// Chance that <see cref="Resource"/> will be spawned at specific pixel of this terrain.
    /// </summary>
    public float ResourceSpawnChance { get; init; }
}

/// <summary>
/// Contains definitions of each terrain type.
/// </summary>
internal static class TerrainTypes
{
    public readonly static TerrainType Border = new()
    {
        Color = new Color(30, 30, 30),
    };

    public readonly static TerrainType DeepWater = new()
    {
        Color = new Color(48, 62, 255),
    };

    public readonly static TerrainType ShallowWater = new()
    {
        Color = new Color(71, 185, 255),
    };

    public readonly static TerrainType Beach = new()
    {
        Color = new Color(255, 253, 158),
        Buildable = true,
        Walkable = true,
    };

    public readonly static TerrainType Plain = new()
    {
        Color = new Color(85, 201, 90),
        Buildable = true,
        Walkable = true,
        Resource = ResourceTypes.Deer,
        ResourceSpawnChance = 0.0001f,
    };
    
    public readonly static TerrainType Forest = new()
    {
        Color = new Color(25, 133, 30),
        Buildable = true,
        Walkable = true,
        Resource = ResourceTypes.Tree,
        ResourceSpawnChance = 0.0002f
    };
    
    public readonly static TerrainType Mountain = new()
    {
        Color = new Color(143, 143, 143),
        Buildable = true,
        Walkable = true,
        Resource = ResourceTypes.Rock,
        ResourceSpawnChance = 0.001f
    };

    public readonly static TerrainType HighMountain = new()
    {
        Color = Color.White,
        Buildable = false,
        Walkable = true,
        Resource = ResourceTypes.Deposite,
        ResourceSpawnChance = 0.002f,
    };

    public static IEnumerable<TerrainType> GetAllTypes()
        => typeof(TerrainTypes).GetFields().Select(p => (TerrainType)p.GetValue(null));
}