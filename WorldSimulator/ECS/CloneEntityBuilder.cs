using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS;
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
