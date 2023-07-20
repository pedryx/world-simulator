using Arch.Core;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Arch;
internal class ArchWorldBuilder : IECSWorldBuilder
{
    public IECSSystem AddSystem<TComponent>(EntityProcessor<TComponent> processor)
        => new ArchSystem<TComponent>(processor);

    public IECSSystem AddSystem<TComponent1, TComponent2>
    (
        EntityProcessor<TComponent1, TComponent2> processor
    )
        => new ArchSystem<TComponent1, TComponent2>(processor);

    public IECSSystem AddSystem<TComponent1, TComponent2, TComponent3>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3> processor
    )
        => new ArchSystem<TComponent1, TComponent2, TComponent3>(processor);

    public IECSSystem AddSystem<TComponent1, TComponent2, TComponent3, TComponent4>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor
    )
        => new ArchSystem<TComponent1, TComponent2, TComponent3, TComponent4>(processor);

    public IECSWorld Build()
        => new BasicECSWorld<World>(World.Create());
}
