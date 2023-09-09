using DefaultEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.DefaultEcs;
/// <summary>
/// https://github.com/Doraku/DefaultEcs
/// </summary>
public class DefaultEcsFactory : ECSFactory
{
    public DefaultEcsFactory() 
        : base
        (
            typeof(DefaultEcsSystem<,>),
            typeof(DefaultEcsSystem<,,>),
            typeof(DefaultEcsSystem<,,,>),
            typeof(DefaultEcsSystem<,,,,>)
        ) { }

    public override IEntity CreateEntity(IECSWorld worldWrapper)
        => new DefaultEcsEntity(((BasicECSWorld<World>)worldWrapper).World.CreateEntity());

    public override IECSWorld CreateWorld()
        => new BasicECSWorld<World>(new World());
}
