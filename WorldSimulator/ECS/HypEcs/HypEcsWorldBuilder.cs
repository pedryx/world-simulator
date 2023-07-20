using HypEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.HypEcs;
internal class HypEcsWorldBuilder : IECSWorldBuilder
{
    public IECSSystem AddSystem<TComponent>(EntityProcessor<TComponent> processor)
        where TComponent : struct
        => new HypEcsSystem<TComponent>(processor);

    public IECSSystem AddSystem<TComponent1, TComponent2>
    (
        EntityProcessor<TComponent1, TComponent2> processor
    )
        where TComponent1 : struct
        where TComponent2 : struct
        => new HypEcsSystem<TComponent1, TComponent2>(processor);

    public IECSSystem AddSystem<TComponent1, TComponent2, TComponent3>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3> processor
    )
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct
        => new HypEcsSystem<TComponent1, TComponent2, TComponent3>(processor);

    public IECSSystem AddSystem<TComponent1, TComponent2, TComponent3, TComponent4>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor
    )
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct
        where TComponent4 : struct
        => new HypEcsSystem<TComponent1, TComponent2, TComponent3, TComponent4>(processor);

    public IECSWorld Build()
        => new BasicECSWorld<World>(new World());
}
