using Leopotam.Ecs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.LeoECS;
internal class LeoECSEntity : IEntity
{
    public EcsEntity Entity { get; private set; }

    public LeoECSEntity(EcsEntity entity)
    {
        Entity = entity;
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : struct 
        => Entity.Replace(component);

    public void Destroy() 
        => Entity.Destroy();

    public ref TComponent GetComponent<TComponent>() 
        where TComponent : struct
    {
        ref TComponent component = ref Entity.Get<TComponent>();
        return ref component;
    }

    public bool HasComponent<TComponent>()
        where TComponent : struct 
        => Entity.Has<TComponent>();

    public void RemoveComponent<TComponent>()
        where TComponent : struct 
        => Entity.Del<TComponent>();

    public bool IsDestroyed()
        => !Entity.IsAlive();
}
