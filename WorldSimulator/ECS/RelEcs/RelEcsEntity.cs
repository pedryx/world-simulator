using RelEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.RelEcs;
internal class RelEcsEntity : IEntity
{
    private readonly Entity entity;
    private readonly World world;

    public RelEcsEntity(Entity entity, World world)
    {
        this.entity = entity;
        this.world = world;
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : unmanaged
        => world.AddComponent(entity, new ComponentWrapper<TComponent>(component));

    public void Destroy()
        => world.Despawn(entity);

    public ref TComponent GetComponent<TComponent>()
        where TComponent : unmanaged
        => ref world.GetComponent<ComponentWrapper<TComponent>>(entity).Component;

    public bool HasComponent<TComponent>() 
        where TComponent : unmanaged
        => world.HasComponent<ComponentWrapper<TComponent>>(entity);

    public bool IsDestroyed()
        => !world.IsAlive(entity);

    public void RemoveComponent<TComponent>()
        where TComponent : unmanaged
        => world.RemoveComponent<ComponentWrapper<TComponent>>(entity);
}
