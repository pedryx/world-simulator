﻿using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator;
internal class ResourceType
{
    /// <summary>
    /// Item obtained by harvesting resource.
    /// </summary>
    public ItemType HarvestItem { get; init; }

    public int HarvestQuantity { get; init; }
}

internal static class ResourceTypes
{
    public readonly static ResourceType Tree = new()
    {
        HarvestItem = ItemTypes.Wood,
        HarvestQuantity = 1,
    };
    public readonly static ResourceType Rock = new()
    {
        HarvestItem = ItemTypes.Stone,
        HarvestQuantity = 1,
    };
    public readonly static ResourceType Deposite = new()
    {
        HarvestItem = ItemTypes.Iron,
        HarvestQuantity = 1,
    };
    public readonly static ResourceType Deer = new()
    {
        HarvestItem = ItemTypes.Meat,
        HarvestQuantity = 1,
    };

    public static IEnumerable<ResourceType> GetAllTypes()
        => typeof(ResourceTypes).GetFields().Select(p => (ResourceType)p.GetValue(null));
}