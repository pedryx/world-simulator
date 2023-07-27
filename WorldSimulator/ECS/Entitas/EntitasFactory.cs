using Entitas;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Entitas;
public class EntitasFactory : ECSFactory
{
    private readonly Context<Entity> prototypesContext = new
    (
        ComponentIDGenerator.ComponentCount,
        () => new Entity()
    );

    public EntitasFactory()
        : base
        (
            typeof(EntitasSystem<,>),
            typeof(EntitasSystem<,,>),
            typeof(EntitasSystem<,,,>),
            typeof(EntitasSystem<,,,,>)
        ) { }

    public override IEntityBuilder CreateEntityBuilder(IECSWorld world)
    {
        return new CloneEntityBuilder
        (
            world,
            new EntitasEntity(prototypesContext.CreateEntity()),
            (world, prototype) =>
            {
                return new EntitasEntity
                (
                    ((BasicECSWorld<Context<Entity>>)world).World
                        .CloneEntity(((EntitasEntity)prototype).Entity)
                );
            }
        );
    }

    public override IECSWorld CreateWorld()
    {
        return new BasicECSWorld<Context<Entity>>(new Context<Entity>
        (
            ComponentIDGenerator.ComponentCount,
            () => new Entity()
        ));
    }
}
