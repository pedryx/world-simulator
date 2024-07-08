using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds capability to own items to an entity.
/// </summary>
[Component]
internal struct Inventory
{
    /// <summary>
    /// ID of item collection which contains inventory items.
    /// </summary>
    public int ItemCollectionID = -1;

    public Inventory(Game game, ItemCollection itemCollection) 
    {
        ItemCollectionID = game.GetManagedDataManager<ItemCollection?>().Insert(itemCollection);
    }
}
