using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds capability to own items to an entity.
/// </summary>
[Component]
internal struct Inventory
{
    /// <summary>
    /// The array of slots, where each slot corresponds to a specific item type ID and holds the quantity of that item
    /// in the inventory.
    /// </summary>
    public int[] Slots;

    public Inventory()
    {
        Slots = new int[ItemType.Count];
    }
}
