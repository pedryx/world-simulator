using MonoGame.Extended.Entities;

using System.Diagnostics;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.MonoGameExtendedEntities;
internal class MonoGameExtendedEntity : IEntity
{
    private readonly MonoGameExtendedWorld world;

    private Entity entity;
    private bool isDestroyed = false;

    public MonoGameExtendedEntity(Entity entity, MonoGameExtendedWorld world)
    {
        this.entity = entity;
        this.world = world;
    }

    public void Build(Entity entity)
    {
        Debug.Assert(this.entity == null);
        Debug.Assert(entity != null);

        this.entity = entity;
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : unmanaged
    {
        if (entity == null)
            world.ScheduleComponentAdd(this, component);
        else
            entity.Attach(new ComponentWrapper<TComponent>(component));
    }

    public void Destroy()
    {
        if (entity == null)
            world.ScheduleDestroy(this);
        else
            entity.Destroy();

        isDestroyed = true;
    }

    public ref TComponent GetComponent<TComponent>()
        where TComponent : unmanaged
    {
        if (entity == null)
            return ref world.GetScheduledComponent<TComponent>(this).Component;
        else
            return ref entity.Get<ComponentWrapper<TComponent>>().Component;
    }

    public bool HasComponent<TComponent>()
        where TComponent : unmanaged
    {
        if (entity == null)
            return world.GetScheduledComponent<TComponent>(this) == null;
        else
            return entity.Has<ComponentWrapper<TComponent>>();
    }

    public bool IsDestroyed()
        => isDestroyed;

    public void RemoveComponent<TComponent>() 
        where TComponent : unmanaged
    {
        if (entity == null)
            world.ScheduleComponentRemove<TComponent>(this);
        else
            entity.Detach<ComponentWrapper<TComponent>>();
    }
}
