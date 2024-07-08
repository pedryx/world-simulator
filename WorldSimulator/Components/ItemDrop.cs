using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Entities with this component will drop items on dead.
/// </summary>
[Component]
internal struct ItemDrop
{
    /// <summary>
    /// ID of item collection which contains items which will be dropped when entity dies.
    /// </summary>
    public int ItemCollectionID = -1;

    public ItemDrop(Game game, ItemCollection items)
    {
        ItemCollectionID = game.GetManagedDataManager<ItemCollection?>().Insert(items);
    }
}
