namespace WorldSimulator;
/// <summary>
/// Represent an item.
/// </summary>
/// <param name="ProcessedType">Type of item into which this item can be processed.</param>
internal sealed class Item : GlobalInstances<Item>
{
    /// <summary>
    /// Item into which current item can be processed or null if this item cannot be processed.
    /// </summary>
    public Item ProcessedItem { get; init; }

    private Item() : base() { }

    public readonly static Item Plank = new();
    public readonly static Item Brick = new();
    public readonly static Item Iron = new();
    public readonly static Item Food = new();
    public readonly static Item Wood = new() { ProcessedItem = Plank };
    public readonly static Item Stone = new() { ProcessedItem = Brick };
    public readonly static Item Ore = new() { ProcessedItem = Iron };
    public readonly static Item Meat = new() { ProcessedItem = Food };
}