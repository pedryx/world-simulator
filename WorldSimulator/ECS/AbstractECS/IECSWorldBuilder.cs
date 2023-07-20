namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Builder which can be used to build ECS worlds (<see cref="IECSWorld"/>) from ECS
/// systems.
/// </summary>
public interface IECSWorldBuilder
{
    /// <summary>
    /// Creates ecs system from entity processor and adds it to the ecs world.
    /// </summary>
    /// <typeparam name="TECSSystem">Type of system to create.</typeparam>
    /// <typeparam name="TEntityProcessor">
    /// Type of processor from which to create the system.
    /// </typeparam>
    /// <param name="processor">Processor from which to create the system.</param>
    /// <returns>Created ecs system.</returns>
    TECSSystem AddSystem<TECSSystem, TEntityProcessor>
    (
        Game game,
        GameState gameState,
        TEntityProcessor processor
    )
        where TECSSystem : ECSSystem<TEntityProcessor>, new()
        where TEntityProcessor : EntityProcessor
    ;

    /// <summary>
    /// Build ECS world (<see cref="IECSWorld"/>) from added ECS systems.
    /// </summary>
    IECSWorld Build();
}
