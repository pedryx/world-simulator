﻿// Ignore Spelling: Buildable

using System.Runtime.CompilerServices;

namespace WorldSimulator;
/// <summary>
/// Represent a terrain.
/// </summary>
internal sealed class Terrain : GlobalInstances<Terrain>
{
    /// <summary>
    /// Determine if structures can be built on this terrain.
    /// </summary>
    public bool Buildable { get; init; }
    /// <summary>
    /// Determine if entities could walk on this terrain.
    /// </summary>
    public bool Walkable { get; init; }
    /// <summary>
    /// The resource which can spawn on this terrain or null if no resource can spawn on this terrain.
    /// </summary>
    public ResourceType ResourceType { get; init; }

    private Terrain() : base() { }

    // The order of declaration of terrains must correspond to IDs in terrain.fx shader file.

    public readonly static Terrain Water = new();
    public readonly static Terrain Beach = new()
    {
        Buildable = true,
        Walkable = true,
    };
    public readonly static Terrain Plain = new()
    {
        Buildable = true,
        Walkable = true,
        ResourceType = ResourceType.Deer,
    };
    public readonly static Terrain Forest = new()
    {
        Buildable = true,
        Walkable = true,
        ResourceType = ResourceType.Tree,
    };
    public readonly static Terrain Mountain = new()
    {
        Buildable = true,
        Walkable = true,
        ResourceType = ResourceType.Rock,
    };
    public readonly static Terrain HighMountain = new()
    {
        Buildable = false,
        Walkable = true,
        ResourceType = ResourceType.Deposit,
    };
}