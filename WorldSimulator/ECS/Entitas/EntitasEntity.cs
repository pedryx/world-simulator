using Entitas;

namespace WorldSimulator.ECS.Entitas;
internal class EntitasEntity : AbstractECS.IEntity
{
    private bool isDestroyed = false;

    public Entity Entity { get; private set; }

    public EntitasEntity(Entity entity)
    {
        Entity = entity;
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : unmanaged
        => Entity.AddComponent(ComponentWrapper<TComponent>.ID, new ComponentWrapper<TComponent>(component));

    public void Destroy()
    {
        isDestroyed = true;
        Entity.Destroy();
    }

    public ref TComponent GetComponent<TComponent>()
        where TComponent : unmanaged
        => ref ((ComponentWrapper<TComponent>)Entity.GetComponent(ComponentWrapper<TComponent>.ID)).Component;

    public bool HasComponent<TComponent>()
        where TComponent : unmanaged
        => Entity.HasComponent(ComponentWrapper<TComponent>.ID); 
    
    public void RemoveComponent<TComponent>()
        where TComponent : unmanaged
        => Entity.RemoveComponent(ComponentWrapper<TComponent>.ID);

    public bool IsDestroyed()
        => isDestroyed;
}
