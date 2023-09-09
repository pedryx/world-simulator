using System.Diagnostics;
using System.Linq;

namespace WorldSimulator;
/// <summary>
/// Represent a collection of items.
/// </summary>
internal readonly struct ItemCollection
{
    /// <summary>
    /// Map IDs of item types to quantities.
    /// </summary>
    private readonly int[] quantities = new int[ItemType.Count];

    public ItemCollection() { }

    public ItemCollection(ItemType itemType, int quantity)
    {
        quantities[itemType.ID] = quantity;
    }

    /// <summary>
    /// Determine if the item collection contains at least one item of a specified type.
    /// </summary>
    public bool Has(ItemType itemType)
        => quantities[itemType.ID] > 0;

    /// <summary>
    /// Transfer all items to an another item collection.
    /// </summary>
    /// <param name="other">The item collection to which will be items transfered.</param>
    public void TransferTo(ref ItemCollection other)
    {
        for (int i = 0; i < ItemType.Count; i++)
        {
            other.quantities[i] += quantities[i];
            quantities[i] = 0;
        }
    }

    /// <summary>
    /// Transfer a specified amount of items of a specified item type to an another item collection.
    /// </summary>
    /// <param name="other">The item collection to which transfer the items.</param>
    /// <param name="itemType">The specified type of items to transfer.</param>
    /// <param name="quantity">The specified amount of items to transfer.</param>
    public void TransferTo(ref ItemCollection other, ItemType itemType, int quantity)
    {
        Debug.Assert(quantities[itemType.ID] >= quantity);

        quantities[itemType.ID] -= quantity;
        other.quantities[itemType.ID] += quantity;
    }

    /// <summary>
    /// Get the quantity of a specified item.
    /// </summary>
    public int GetQuantity(ItemType itemType)
        => quantities[itemType.ID];

    /// <summary>
    /// Add an item to the collection.
    /// </summary>
    /// <param name="itemType">The type of item to add.</param>
    /// <param name="quantity">How many items of the specified type to add.</param>
    public void Add(ItemType itemType, int quantity)
        => quantities[itemType.ID] += quantity;

    /// <summary>
    /// Removes item from the collection.
    /// </summary>
    /// <param name="itemType">The type of item to remove.</param>
    /// <param name="quantity">How many items of the specified type to remove.</param>
    public void Remove(ItemType itemType, int quantity)
    {
        Debug.Assert(quantities[itemType.ID] >= quantity);

        quantities[itemType.ID] -= quantity;
    }

    /// <summary>
    /// Transform items of a specified type to an another specified type.
    /// </summary>
    /// <param name="itemType1">The type of item to transform.</param>
    /// <param name="itemType2">The type of item to which to transform.</param>
    public void Transform(ItemType itemType1, ItemType itemType2)
    {
        quantities[itemType2.ID] += quantities[itemType1.ID];
        quantities[itemType1.ID] = 0;
    }
}
