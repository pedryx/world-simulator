using HypEcs;

using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.HypEcs;
public class HypEcsFactory : ECSFactory
{
    private readonly List<HypEcsEntity> entities = new();

    internal void AddEntity(HypEcsEntity entity)
        => entities.Add(entity);

    internal void RemoveEntity(HypEcsEntity entity)
        => entities.Remove(entity);

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

        return new HypEcsEntity(world.Spawn().Id(), world, this);
    }

    public override IECSWorld CreateWorld()
    {
        return new BasicECSWorld<World>(new World(), world =>
        {
            foreach (var entity in entities)
            {
                entity.Update();
            }

            world.Tick();
        });
    }
}
