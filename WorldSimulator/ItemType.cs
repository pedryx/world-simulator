namespace WorldSimulator;
/// <summary>
/// Represent a type of item.
/// </summary>
/// <param name="ProcessedType">The type of item into which this item can be processed.</param>
internal sealed class ItemType : GlobalInstances<ItemType>
{
    /// <summary>
    /// The item into which the current item can be processed or null if this item cannot be processed.
    /// </summary>
    public ItemType ProcessedItem { get; init; }
    /// <summary>
    /// The time takes until the resource gets processed. Only affects items that can be processed.
    /// </summary>
    public float TimeToProcess { get; init; }

    private ItemType() : base() { }

    public readonly static ItemType Plank = new();
    public readonly static ItemType Brick = new();
    public readonly static ItemType Iron = new();
    public readonly static ItemType Food = new();
    public readonly static ItemType Wood = new() { ProcessedItem = Plank, TimeToProcess = 1.0f };
    public readonly static ItemType Stone = new() { ProcessedItem = Brick, TimeToProcess = 3.0f };
    public readonly static ItemType Ore = new() { ProcessedItem = Iron, TimeToProcess = 5.0f };
    public readonly static ItemType Meat = new() { ProcessedItem = Food, TimeToProcess = 7.0f };
}