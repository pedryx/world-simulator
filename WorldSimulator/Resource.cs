namespace WorldSimulator;
/// <summary>
/// Represent a natural resource, which can be harvested to obtain resources.
/// </summary>
internal sealed class Resource : GlobalInstances<Resource>
{
    /// <summary>
    /// Item obtained upon harvesting resource.
    /// </summary>
    public Item HarvestItem { get; init; }
    /// <summary>
    /// Number of items obtained upon harvesting resource.
    /// </summary>
    public int HarvestQuantity { get; init; }
    /// <summary>
    /// How long it takes to harvest resource in seconds.
    /// </summary>
    public float HarvestTime { get; init; }

    private Resource() : base() { }

    public readonly static Resource Tree = new()
    {
        HarvestItem = Item.Wood,
        HarvestQuantity = 1,
        HarvestTime = 3.0f,
    };
    public readonly static Resource Rock = new()
    {
        HarvestItem = Item.Stone,
        HarvestQuantity = 1,
        HarvestTime = 5.0f,
    };
    public readonly static Resource Deposit = new()
    {
        HarvestItem = Item.Ore,
        HarvestQuantity = 1,
        HarvestTime = 7.0f,
    };
    public readonly static Resource Deer = new()
    {
        HarvestItem = Item.Meat,
        HarvestQuantity = 1,
        HarvestTime = 9.0f,
    };
}