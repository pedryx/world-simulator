using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Provides owner information to an entity.
/// </summary>
[Component]
internal struct Owner
{
    /// <summary>
    /// The ID of an entity which owns this component.
    /// </summary>
    public int EntityID = -1;

    public Owner(Game game, IEntity entity)
    {
        EntityID = game.GetManagedDataManager<IEntity>().Insert(entity);

    }
}
