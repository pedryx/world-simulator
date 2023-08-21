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
        where TComponent : struct
        => Entity.Set(component);

    public void Destroy()
        => Entity.Dispose();

    public ref TComponent GetComponent<TComponent>()
        where TComponent : struct
        => ref Entity.Get<TComponent>();

    public bool HasComponent<TComponent>()
        where TComponent : struct
        => Entity.Has<TComponent>();

    public void RemoveComponent<TComponent>()
        where TComponent : struct
        => Entity.Remove<TComponent>();

    public bool IsDestroyed()
        => !Entity.IsAlive;
}
