using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS;
/// <summary>
/// Entity builder which will create an entity as prototype and it will clone this prototype
/// on <see cref="Build(IECSWorld)"/> call.
/// </summary>
internal class CloneEntityBuilder : IEntityBuilder
{
    private readonly IEntity prototype;
    private readonly Func<IECSWorld, IEntity, IEntity> clone;
    private readonly IECSWorld world;

    public CloneEntityBuilder(IECSWorld world, IEntity prototype, Func<IECSWorld, IEntity, IEntity> clone)
    {
        this.prototype = prototype;
        this.clone = clone;
        this.world = world;
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : struct
    {
        prototype.AddComponent(component);
    }

    public IEntity Build()
    {
        return clone(world, prototype);
    }
}
