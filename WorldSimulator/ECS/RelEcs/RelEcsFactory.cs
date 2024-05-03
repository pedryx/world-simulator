using RelEcs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.RelEcs;
/// <summary>
/// https://github.com/Byteron/RelEcs
/// </summary>
public class RelEcsFactory : ECSFactory
{
    public RelEcsFactory() 
        : base
        (
            typeof(RelEcsSystem<,>),
            typeof(RelEcsSystem<,,>),
            typeof(RelEcsSystem<,,,>),
            typeof(RelEcsSystem<,,,,>)
        ) { }

    public override IEntity CreateEntity(IECSWorld wrapper)
    {
        World world = ((BasicECSWorld<World>)wrapper).World;
        Entity entity = world.Spawn().Id();

        return new RelEcsEntity(entity, world);
    }

    public override IECSWorld CreateWorld()
        => new BasicECSWorld<World>(new World());
}
