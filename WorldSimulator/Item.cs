using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator;
internal class ItemType
{
}

internal static class ItemTypes
{
    public readonly static ItemType Wood = new();
    public readonly static ItemType Stone = new();
    public readonly static ItemType Iron = new();
    public readonly static ItemType Meat = new();

    public static IEnumerable<ResourceType> GetAllTypes()
        => typeof(ItemTypes).GetFields().Select(p => (ResourceType)p.GetValue(null));
}