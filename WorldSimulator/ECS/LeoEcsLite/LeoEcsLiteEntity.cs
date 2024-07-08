using Leopotam.EcsLite;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.LeoEcsLite;
internal class LeoEcsLiteEntity : IEntity
{
    private readonly int entity;
    private readonly EcsWorld world;

    private bool isDestroyed = false;

    public LeoEcsLiteEntity(int entity, EcsWorld world)
    {
        this.entity = entity;
        this.world = world;
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : unmanaged
        => world.GetPool<TComponent>().Add(entity) = component;

    public void Destroy()
    {
        isDestroyed = true;
        world.DelEntity(entity);
    }

    public ref TComponent GetComponent<TComponent>()
        where TComponent : unmanaged
        => ref world.GetPool<TComponent>().Get(entity);

    public bool HasComponent<TComponent>()
        where TComponent : unmanaged
        => world.GetPool<TComponent>().Has(entity);

    public bool IsDestroyed()
        => isDestroyed;

    public void RemoveComponent<TComponent>()
        where TComponent : unmanaged
        => world.GetPool<TComponent>().Del(entity);
}
