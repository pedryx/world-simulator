using Leopotam.Ecs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.LeoEcs;
public class LeoEcsFactory : ECSFactory
{
    public LeoEcsFactory()
        : base
        (
            typeof(LeoEcsSystem<,>),
            typeof(LeoEcsSystem<,,>),
            typeof(LeoEcsSystem<,,,>),
            typeof(LeoEcsSystem<,,,>)
        ) { }

    public override IEntity CreateEntity(IECSWorld world)
        => new LeoEcsEntity(((BasicECSWorld<EcsWorld>)world).World.NewEntity());

    public override IECSWorld CreateWorld()
        => new BasicECSWorld<EcsWorld>(new EcsWorld());
}
