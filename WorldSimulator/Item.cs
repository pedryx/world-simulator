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
    /// <summary>
    /// How long it takes if resource will be processed. Only affects items which can be processed.
    /// </summary>
    public float TimeToProcess { get; init; }

    private Item() : base() { }

    public readonly static Item Plank = new();
    public readonly static Item Brick = new();
    public readonly static Item Iron = new();
    public readonly static Item Food = new();
    public readonly static Item Wood = new() { ProcessedItem = Plank, TimeToProcess = 1.0f };
    public readonly static Item Stone = new() { ProcessedItem = Brick, TimeToProcess = 3.0f };
    public readonly static Item Ore = new() { ProcessedItem = Iron, TimeToProcess = 5.0f };
    public readonly static Item Meat = new() { ProcessedItem = Food, TimeToProcess = 7.0f };
}