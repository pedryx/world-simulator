using DefaultEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.DefaultEcs;
internal class DefaultECSWorldBuilder : IECSWorldBuilder
{
    public IECSSystem AddSystem<TComponent>(EntityProcessor<TComponent> processor)
        => new DefaultEcsSystem<TComponent>(processor);

    public IECSSystem AddSystem<TComponent1, TComponent2>
    (
        EntityProcessor<TComponent1, TComponent2> processor
    )
        => new DefaultEcsSystem<TComponent1, TComponent2>(processor);

    public IECSSystem AddSystem<TComponent1, TComponent2, TComponent3>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3> processor
    )
        => new DefaultEcsSystem<TComponent1, TComponent2, TComponent3>(processor);

    public IECSSystem AddSystem<TComponent1, TComponent2, TComponent3, TComponent4>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor
    )
        => new DefaultEcsSystem<TComponent1, TComponent2, TComponent3, TComponent4>(processor);

    public IECSWorld Build()
        => new BasicECSWorld<World>(new World());
}
