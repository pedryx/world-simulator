using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS;
public class BasicECSWorldBuilder<TWorld> : IECSWorldBuilder
{
    private readonly TWorld world;

    public BasicECSWorldBuilder(TWorld world)
    {
        this.world = world;
    }

    public TECSSystem AddSystem<TECSSystem, TEntityProcessor>
    (
        Game game,
        GameState gameState,
        TEntityProcessor processor
    )
        where TECSSystem : ECSSystem<TEntityProcessor>, new()
        where TEntityProcessor : EntityProcessor
    {
        TECSSystem system = new();
        system.Initialize(game, gameState, processor);

        return system;
    }

    public IECSWorld Build()
        => new BasicECSWorld<TWorld>(world);
}
