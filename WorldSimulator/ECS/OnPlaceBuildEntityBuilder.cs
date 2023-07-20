using System;
using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS;
public class OnPlaceBuildEntityBuilder : IEntityBuilder
{
    private readonly List<Type> types = new();
    private readonly List<object> values = new();
    private readonly Func<List<Type>, List<object>, IECSWorld, IEntity> build;

    public OnPlaceBuildEntityBuilder(Func<List<Type>, List<object>, IECSWorld, IEntity> build)
    {
        this.build = build;
    }

    public void AddComponent<TComponent>(TComponent component)
    {
        types.Add(component.GetType());
        values.Add(component);
    }

    public IEntity Build(IECSWorld world)
        => build(types, values, world);
}
