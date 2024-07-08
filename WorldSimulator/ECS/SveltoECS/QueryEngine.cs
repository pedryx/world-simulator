using Svelto.ECS;

namespace WorldSimulator.ECS.SveltoECS;

internal class QueryEngine : IQueryingEntitiesEngine
{
    public EntitiesDB entitiesDB { get; set; }

    public void Ready() { }

    public ref TComponent GetComponent<TComponent>(EGID egid)
        where TComponent : unmanaged
        => ref entitiesDB.QueryEntity<ComponentWrapper<TComponent>>(egid).Component;

    public bool HasComponent<TComponent>(EGID egid)
        where TComponent : unmanaged
        => entitiesDB.Exists<ComponentWrapper<TComponent>>(egid);

    public void SetComponent<TComponent>(EGID egid, TComponent component)
        where TComponent : unmanaged
        => entitiesDB.QueryEntity<ComponentWrapper<TComponent>>(egid) = new ComponentWrapper<TComponent>(component);
}
