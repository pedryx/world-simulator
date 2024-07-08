// Ignore Spelling: Ecs

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
        where TComponent : unmanaged
        => Entity.Set(component);

    public void Destroy()
        => Entity.Dispose();

    public ref TComponent GetComponent<TComponent>()
        where TComponent : unmanaged
        => ref Entity.Get<TComponent>();

    public bool HasComponent<TComponent>()
        where TComponent : unmanaged
        => Entity.Has<TComponent>();

    public void RemoveComponent<TComponent>()
        where TComponent : unmanaged
        => Entity.Remove<TComponent>();

    public bool IsDestroyed()
        => !Entity.IsAlive;
}
