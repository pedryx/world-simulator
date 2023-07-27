using HypEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.HypEcs;
internal class HypEcsEntity : IEntity
{
    private readonly Entity entity;
    private readonly World world;

    public HypEcsEntity(Entity entity, World world)
    {
        this.entity = entity;
        this.world = world;
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : struct 
        => world.AddComponent(entity, component);

    public void Destroy() 
        => world.Despawn(entity);

    public ref TComponent GetComponent<TComponent>()
        where TComponent : struct 
        => ref world.GetComponent<TComponent>(entity);

    public bool HasComponent<TComponent>()
        where TComponent : struct 
        => world.HasComponent<TComponent>(entity);

    public void RemoveComponent<TComponent>()
        where TComponent : struct 
        => world.RemoveComponent<TComponent>(entity);
}
