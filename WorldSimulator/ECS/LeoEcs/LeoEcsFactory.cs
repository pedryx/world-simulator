using Leopotam.Ecs;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.LeoECS;
public class LeoECSFactory : ECSFactory
{
    public LeoECSFactory()
        : base
        (
            typeof(LeoEcsSystem<,>),
            typeof(LeoEcsSystem<,,>),
            typeof(LeoEcsSystem<,,,>),
            typeof(LeoEcsSystem<,,,,>)
        ) { }

    public override IEntity CreateEntity(IECSWorld world)
        => new LeoECSEntity(((BasicECSWorld<EcsWorld>)world).World.NewEntity());

    public override IECSWorld CreateWorld()
        => new BasicECSWorld<EcsWorld>(new EcsWorld());
}
