using Entitas;

using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Entitas;
internal class EntitasECSWorldBuilder : IECSWorldBuilder
{
    private readonly Context<Entity> context;
    private bool builded = false;

    public EntitasECSWorldBuilder()
    {
        context = new Context<Entity>(ComponentIDGenerator.ComponentCount, () => new Entity());
    }

    public IECSSystem AddSystem<TComponent>(EntityProcessor<TComponent> processor)
        where TComponent : struct
        => new EntitasSystem<TComponent>(processor, context);

    public IECSSystem AddSystem<TComponent1, TComponent2>
    (
        EntityProcessor<TComponent1, TComponent2> processor
    )
        where TComponent1 : struct
        where TComponent2 : struct
        => new EntitasSystem<TComponent1, TComponent2>(processor, context);

    public IECSSystem AddSystem<TComponent1, TComponent2, TComponent3>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3> processor
    )
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct
        => new EntitasSystem<TComponent1, TComponent2, TComponent3>(processor, context);

    public IECSSystem AddSystem<TComponent1, TComponent2, TComponent3, TComponent4>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor
    )
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct
        where TComponent4 : struct
        => new EntitasSystem<TComponent1, TComponent2, TComponent3, TComponent4>(processor, context);

    public IECSWorld Build()
    {
        if (builded)
            throw new InvalidOperationException("Due to the Entitas framework limitations, Builder could be invoked only once on each world builder!");

        builded = true;
        return new BasicECSWorld<Context<Entity>>(context);
    }
}
