using Leopotam.EcsLite;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.LeoEcsLite;
/// <summary>
/// https://github.com/Leopotam/ecslite
/// </summary>
internal class LeoEcsLiteFactory : ECSFactory
{
    public LeoEcsLiteFactory() 
        : base
        (
            typeof(LeoEcsLiteSystem<,>), 
            typeof(LeoEcsLiteSystem<,,>), 
            typeof(LeoEcsLiteSystem<,,,>),
            typeof(LeoEcsLiteSystem<,,,,>)
        ) { }

    public override IEntity CreateEntity(IECSWorld wrapper)
    {
        EcsWorld world = ((BasicECSWorld<EcsWorld>)wrapper).World;
        int entity = world.NewEntity();

        return new LeoEcsLiteEntity(entity, world);
    }

    public override IECSWorld CreateWorld()
        => new BasicECSWorld<EcsWorld>(new EcsWorld());
}
