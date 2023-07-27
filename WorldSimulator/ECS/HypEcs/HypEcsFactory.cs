using HypEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.HypEcs;
public class HypEcsFactory : ECSFactory
{
    public HypEcsFactory()
        : base
        (
            typeof(HypEcsSystem<,>),
            typeof(HypEcsSystem<,,>),
            typeof(HypEcsSystem<,,,>),
            typeof(HypEcsSystem<,,,,>)
        ) { }

    public override IEntityBuilder CreateEntityBuilder(IECSWorld world) 
        => new HypEcsEntityBuilder(((BasicECSWorld<World>)world).World);

    public override IECSWorld CreateWorld() 
        => new BasicECSWorld<World>(new World());
}
