
using DefaultEcs;

using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.DefaultEcs;
/// <summary>
/// https://github.com/Doraku/DefaultEcs
/// </summary>
public class DefaultEcsFactory : ECSFactory
{
    private readonly World prototypesWorld = new();

    public DefaultEcsFactory() 
        : base
        (
            typeof(DefaultEcsSystem<,>),
            typeof(DefaultEcsSystem<,,>),
            typeof(DefaultEcsSystem<,,,>),
            typeof(DefaultEcsSystem<,,,,>)
        ) { }

    public override IEntityBuilder CreateEntityBuilder(IECSWorld world)
    {
        return new CloneEntityBuilder
        (
            world,
            new DefaultEcsEntity(prototypesWorld.CreateEntity()),
            (world, prototype) =>
            {
                return new DefaultEcsEntity
                (
                    ((DefaultEcsEntity)prototype)
                        .Entity.CopyTo(((BasicECSWorld<World>)world).World)
                );
            }
        );
    }

    public override IECSWorld CreateWorld()
        => new BasicECSWorld<World>(new World());
}
