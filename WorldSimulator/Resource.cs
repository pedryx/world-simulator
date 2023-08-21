namespace WorldSimulator;
/// <summary>
/// Represent a natural resource, which can be harvested to obtain resources.
/// </summary>
internal sealed class ResourceType : GlobalInstances<ResourceType>
{
    /// <summary>
    /// The item obtained upon harvesting resource.
    /// </summary>
    public ItemType HarvestItem { get; init; }
    /// <summary>
    /// The number of items obtained upon harvesting the resource.
    /// </summary>
    public int HarvestQuantity { get; init; }
    /// <summary>
    /// Time it takes to harvest the resource in seconds.
    /// </summary>
    public float HarvestTime { get; init; }

    private ResourceType() : base() { }

    public readonly static ResourceType Tree = new()
    {
        HarvestItem = ItemType.Wood,
        HarvestQuantity = 1,
        HarvestTime = 3.0f,
    };
    public readonly static ResourceType Rock = new()
    {
        HarvestItem = ItemType.Stone,
        HarvestQuantity = 1,
        HarvestTime = 5.0f,
    };
    public readonly static ResourceType Deposit = new()
    {
        HarvestItem = ItemType.Ore,
        HarvestQuantity = 1,
        HarvestTime = 7.0f,
    };
    public readonly static ResourceType Deer = new()
    {
        HarvestItem = ItemType.Meat,
        HarvestQuantity = 1,
        HarvestTime = 9.0f,
    };
}