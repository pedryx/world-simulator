using Entitas;

namespace WorldSimulator.ECS.Entitas;
internal class EntitasEntity : AbstractECS.IEntity
{
    public Entity Entity { get; private set; }

    public EntitasEntity(Entity entity)
    {
        Entity = entity;
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : struct
        => Entity.AddComponent(ComponentWrapper<TComponent>.ID, new ComponentWrapper<TComponent>(component));

    public void Destroy()
        => Entity.Destroy();

    public ref TComponent GetComponent<TComponent>()
        where TComponent : struct
        => ref ((ComponentWrapper<TComponent>)Entity.GetComponent(ComponentWrapper<TComponent>.ID)).Component;

    public bool HasComponent<TComponent>()
        where TComponent : struct
        => Entity.HasComponent(ComponentWrapper<TComponent>.ID); 
    
    public void RemoveComponent<TComponent>()
        where TComponent : struct
        => Entity.RemoveComponent(ComponentWrapper<TComponent>.ID);
}
