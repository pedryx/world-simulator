using Entitas;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Entitas;
internal class EntitasFactory : IECSFactory
{
    private readonly Context<Entity> prototypesContext = new
    (
        ComponentIDGenerator.ComponentCount,
        () => new Entity()
    );

    public void Initialize() { }

    public IEntityBuilder CreateEntityBuilder(IECSWorld world)
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

    public IECSWorldBuilder CreateWorldBuilder()
        => new EntitasECSWorldBuilder();
}
