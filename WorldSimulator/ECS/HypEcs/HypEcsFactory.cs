using HypEcs;

using System.Linq;
using System.Reflection;

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
    {
        return new OnPlaceBuildEntityBuilder
        (
            world,
            (types, values, world) =>
            {
                EntityBuilder builder = ((BasicECSWorld<World>)world).World.Spawn();

                MethodInfo method = (from m in typeof(EntityBuilder).GetMethods()
                                     where m.Name == nameof(builder.Add)
                                     where m.GetParameters().Length == 1
                                     where m.GetParameters()[0].Name == "data"
                                     select m).First();

                for (int i = 0; i < types.Count; i++)
                {
                    method.MakeGenericMethod(types[i]).Invoke
                    (
                        builder,
                        new object[] { values[i] }
                    );
                }

                return new HypEcsEntity(builder.Id(), ((BasicECSWorld<World>)world).World);
            }
        );
    }
    // => new HypEcsEntityBuilder(((BasicECSWorld<World>)world).World);

    public override IECSWorld CreateWorld() 
        => new BasicECSWorld<World>(new World(), world => world.Tick());
}
