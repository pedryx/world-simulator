using Arch.Core;
using Arch.Core.Extensions;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Arch;
internal class ArchEntity : IEntity
{
    private readonly Entity entity;
    private readonly World world;

    public ArchEntity(Entity entity, World world)
    {
        this.entity = entity;
        this.world = world;
    }

    public void AddComponent<TComponent>(TComponent component)
        => entity.Add(component);

    public void Destroy()
        => world.Destroy(entity);

    public ref TComponent GetComponent<TComponent>()
        => ref entity.Get<TComponent>();

    public bool HasComponent<TComponent>()
        => entity.Has<TComponent>();

    public void RemoveComponent<TComponent>()
        => entity.Remove<TComponent>();
}
