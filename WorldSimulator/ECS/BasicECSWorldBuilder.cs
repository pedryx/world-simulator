using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS;
/// <summary>
/// Simple ECS world builder for ecs libraries which dont need to store ecs system in ecs
/// world.
/// </summary>
/// <typeparam name="TWorld">Type of ecs world (actual type not the wrapper).</typeparam>
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
