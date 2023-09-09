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

    public override IEntity CreateEntity(IECSWorld worldWrapper)
    {
        World world = ((BasicECSWorld<World>)worldWrapper).World;

        return new HypEcsEntity(world.Spawn().Id(), world);
    }

    public override IECSWorld CreateWorld() 
        => new BasicECSWorld<World>(new World(), world => world.Tick());
}
