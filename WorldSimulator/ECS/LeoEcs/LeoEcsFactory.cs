using Leopotam.Ecs;

using System.Reflection;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.LeoEcs;
internal class LeoEcsFactory : IECSFactory
{
    public void Initialize() { }

    public IEntityBuilder CreateEntityBuilder(IECSWorld world)
    {
        return new OnPlaceBuildEntityBuilder
        (
            world,
            (types, values, world) =>
            {
                EcsEntity entity = ((BasicECSWorld<EcsWorld>)world).World.NewEntity();
                MethodInfo method = typeof(EcsEntityExtensions).GetMethod("Replace");

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

    public IECSWorldBuilder CreateWorldBuilder()
        => new LeoEcsWorldBuilder();
}
