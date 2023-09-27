using System.Diagnostics;

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

    public ItemCollection(params int[] quantities)
    {
        Debug.Assert(quantities.Length <= this.quantities.Length);

        for (int i = 0; i < quantities.Length; i++)
        {
            this.quantities[i] = quantities[i];
        }
    }

    /// <summary>
    /// Determine if the item collection contains any items.
    /// </summary>
    public bool Empty()
    {
        for (int i = 0; i < quantities.Length; i++)
        {
            if (quantities[i] > 0)
                return false;
        }

        return true;
    }

    /// <summary>
    /// Determine if the item collection contains at least one item of a specified type.
    /// </summary>
    public bool Has(ItemType itemType)
        => quantities[itemType.ID] > 0;

    /// <summary>
    /// Determine if item collection contains specified amounts of specified items.
    /// </summary>
    public bool Contains(ItemCollection  items)
    {
        for (int i = 0; i < quantities.Length; i++)
        {
            if (quantities[i] < items.quantities[i])
                return false;
        }

        return true;
    }

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
    /// Removes items from item collection. Items to remove are specified with an another item collection.
    /// </summary>
    /// <param name="items">The item collection containing items to remove.</param>
    public void Remove(ItemCollection items)
    {
        for (int i = 0; i < quantities.Length; i++)
        {
            // TODO: fix bug where this assert triggers (caused sometimes when new building should be build)
            Debug.Assert(quantities[i] <= items.quantities[i]);

            quantities[i] -= items.quantities[i];
        }
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

    /// <summary>
    /// Concatenate two item collections into a new one.
    /// </summary>
    public static ItemCollection operator+(ItemCollection itemCollection1, ItemCollection itemCollection2)
    {
        int[] quantities = new int[ItemType.Count];

        for (int i = 0; i < quantities.Length; i++)
        {
            quantities[i] = itemCollection1.quantities[i] + itemCollection2.quantities[i];
        }

        return new ItemCollection(quantities);
    }

    /// <summary>
    /// Multiply quantity of each item in the item collection by a specified multiplier.
    /// </summary>
    public static ItemCollection operator *(ItemCollection itemCollection, float multiplier)
    {
        int[] quantities = new int[ItemType.Count];

        for (int i = 0; i < quantities.Length; i++)
        {
            quantities[i] = (int)(itemCollection.quantities[i] * multiplier);
        }

        return new ItemCollection(quantities);
    }
}
