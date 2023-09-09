using Entitas;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Entitas;
public class EntitasFactory : ECSFactory
{
    public EntitasFactory()
        : base
        (
            typeof(EntitasSystem<,>),
            typeof(EntitasSystem<,,>),
            typeof(EntitasSystem<,,,>),
            typeof(EntitasSystem<,,,,>)
        ) { }

    public override AbstractECS.IEntity CreateEntity(IECSWorld world) 
        => new EntitasEntity(((BasicECSWorld<Context<Entity>>)world).World.CreateEntity());

    public override IECSWorld CreateWorld()
    {
        return new BasicECSWorld<Context<Entity>>(new Context<Entity>
        (
            ComponentIDGenerator.ComponentCount,
            () => new Entity()
        ));
    }
}
