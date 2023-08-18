using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator;
internal class ResourceType
{
    /// <summary>
    /// Item obtained by harvesting resource.
    /// </summary>
    public ItemType HarvestItem { get; init; }
    public int HarvestQuantity { get; init; }
    public float HarvestTime { get; init; }
}

internal static class ResourceTypes
{
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

    public static IEnumerable<ResourceType> GetAllTypes()
        => typeof(ResourceTypes).GetFields().Select(p => (ResourceType)p.GetValue(null));
}