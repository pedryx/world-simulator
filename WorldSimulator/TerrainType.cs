// Ignore Spelling: Buildable

namespace WorldSimulator;
/// <summary>
/// Represent a terrain type.
/// </summary>
internal sealed class TerrainType : GlobalInstances<TerrainType>
{
    /// <summary>
    /// Determine if structures can be built on this terrain.
    /// </summary>
    public bool CanBuild { get; init; }
    /// <summary>
    /// Determine if entities could walk on this terrain.
    /// </summary>
    public bool CanWalk { get; init; }
    /// <summary>
    /// The resource which can spawn on this terrain or null if no resource can spawn on this terrain.
    /// </summary>
    public ResourceType ResourceType { get; init; }

    private TerrainType() : base() { }

    // The order of declaration of terrains must correspond to IDs in terrain.fx shader file.

    public readonly static TerrainType Water = new();
    public readonly static TerrainType Beach = new()
    {
        CanBuild = true,
        CanWalk = true,
    };
    public readonly static TerrainType Plain = new()
    {
        CanBuild = true,
        CanWalk = true,
        ResourceType = ResourceType.Deer,
    };
    public readonly static TerrainType Forest = new()
    {
        CanBuild = true,
        CanWalk = true,
        ResourceType = ResourceType.Tree,
    };
    public readonly static TerrainType Mountain = new()
    {
        CanBuild = true,
        CanWalk = true,
        ResourceType = ResourceType.Rock,
    };
    public readonly static TerrainType HighMountain = new()
    {
        CanBuild = false,
        CanWalk = true,
        ResourceType = ResourceType.Deposit,
    };
}