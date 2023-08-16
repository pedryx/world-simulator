using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator;
internal class TerrainType
{
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
    public ResourceType ResourceType { get; init; }
}

/// <summary>
/// Contains definitions of each terrain type.
/// </summary>
internal static class TerrainTypes
{
    public readonly static TerrainType ShallowWater = new();

    public readonly static TerrainType Beach = new()
    {
        Buildable = true,
        Walkable = true,
    };

    public readonly static TerrainType Plain = new()
    {
        Buildable = true,
        Walkable = true,
        ResourceType = ResourceTypes.Deer,
    };
    
    public readonly static TerrainType Forest = new()
    {
        Buildable = true,
        Walkable = true,
        ResourceType = ResourceTypes.Tree,
    };
    
    public readonly static TerrainType Mountain = new()
    {
        Buildable = true,
        Walkable = true,
        ResourceType = ResourceTypes.Rock,
    };

    public readonly static TerrainType HighMountain = new()
    {
        Buildable = false,
        Walkable = true,
        ResourceType = ResourceTypes.Deposite,
    };

    public static TerrainType GetTerrainType(int id)
    {
        return id switch
        {
            0 => ShallowWater,
            1 => Beach,
            2 => Plain,
            3 => Forest,
            4 => Mountain,
            5 => HighMountain,

            _ => throw new InvalidOperationException("Invalid terrain id!"),
        };
    }

    public static IEnumerable<TerrainType> GetAllTypes()
        => typeof(TerrainTypes).GetFields().Select(p => (TerrainType)p.GetValue(null));
}