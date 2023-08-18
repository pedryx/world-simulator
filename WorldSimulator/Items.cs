using System;
using System.Runtime.CompilerServices;

namespace WorldSimulator;
/// <summary>
/// Represent an item.
/// </summary>
/// <param name="ProcessedType">Type of item into which this item can be processed.</param>
internal readonly record struct Item(ItemType ProcessedType);

internal enum ItemType
{
    None = -1,
    Wood,
    Stone,
    Ore,
    Meat,
    Plank,
    Brick,
    Iron,
    Food,
}

internal static class Items
{
    private readonly static Item[] items = new Item[Enum.GetValues<ItemType>().Length - 1];

    [ModuleInitializer]
    internal static void Initialize()
    {
        items[(int)ItemType.Wood] = new Item() { ProcessedType = ItemType.Plank };
        items[(int)ItemType.Stone] = new Item() { ProcessedType = ItemType.Brick };
        items[(int)ItemType.Ore] = new Item() { ProcessedType = ItemType.Iron };
        items[(int)ItemType.Meat] = new Item() { ProcessedType = ItemType.Food };
        items[(int)ItemType.Plank] = new Item();
        items[(int)ItemType.Brick] = new Item();
        items[(int)ItemType.Iron] = new Item();
        items[(int)ItemType.Food] = new Item();
    }

    public static int Count => items.Length;

    public static Item Get(ItemType type)
        => items[(int)type];
}