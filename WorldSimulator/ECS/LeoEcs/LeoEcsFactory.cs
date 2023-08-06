using Leopotam.Ecs;

using System.Reflection;

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

    public override IEntityBuilder CreateEntityBuilder(IECSWorld world)
    {
        return new OnPlaceBuildEntityBuilder
        (
            world,
            (types, values, world) =>
            {
                EcsEntity entity = ((BasicECSWorld<EcsWorld>)world).World.NewEntity();
                MethodInfo method = typeof(EcsEntityExtensions).GetMethod(nameof(EcsEntityExtensions.Replace));

                for (int i = 0; i < types.Count; i++)
                {
                    method.MakeGenericMethod(types[i]).Invoke
                    (
                        null,
                        new object[] { entity, values[i] }
                    );
                }

                return new LeoEcsEntity(entity);
            }
        );
    }

    public override IECSWorld CreateWorld()
        => new BasicECSWorld<EcsWorld>(new EcsWorld());
}
