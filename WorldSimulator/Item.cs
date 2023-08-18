using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator;
internal class ItemType
{
    /// <summary>
    /// Number of item types.
    /// </summary>
    public static int Count;

    public readonly int ID = Count++;

    /// <summary>
    /// Type of item into which current item can be processed, or null if current item cannot be processed.
    /// </summary>
    public ItemType Processed { get; init; }
}

internal static class ItemTypes
{
    public readonly static ItemType Wood = new() { Processed = Plank };
    public readonly static ItemType Stone = new() { Processed = Brick };
    public readonly static ItemType Ore = new() { Processed = Iron };
    public readonly static ItemType Meat = new() { Processed = Food };
    public readonly static ItemType Plank = new();
    public readonly static ItemType Brick = new();
    public readonly static ItemType Iron = new();
    public readonly static ItemType Food = new();

    public static IEnumerable<ResourceType> GetAllTypes()
        => typeof(ItemTypes).GetFields().Select(p => (ResourceType)p.GetValue(null));
}