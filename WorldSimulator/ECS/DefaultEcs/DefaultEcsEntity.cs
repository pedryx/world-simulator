using DefaultEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.DefaultEcs;
internal class DefaultEcsEntity : IEntity
{
    public Entity Entity { get; private set; }

    public DefaultEcsEntity(Entity entity)
    {
        Entity = entity;
    }

    public void AddComponent<TComponent>(TComponent component)
        => Entity.Set(component);

    public void Destroy()
        => Entity.Dispose();

    public ref TComponent GetComponent<TComponent>()
        => ref Entity.Get<TComponent>();

    public bool HasComponent<TComponent>()
        => Entity.Has<TComponent>();

    public void RemoveComponent<TComponent>()
        => Entity.Remove<TComponent>();
}
