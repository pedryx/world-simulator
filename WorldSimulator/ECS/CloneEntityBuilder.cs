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

    public CloneEntityBuilder(IEntity prototype, Func<IECSWorld, IEntity, IEntity> clone)
    {
        this.prototype = prototype;
        this.clone = clone;
    }

    public void AddComponent<TComponent>(TComponent component)
    {
        prototype.AddComponent(component);
    }

    public IEntity Build(IECSWorld world)
    {
        return clone(world, prototype);
    }
}
