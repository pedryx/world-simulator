namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Builder which can be used to build ECS worlds (<see cref="IECSWorld"/>) from ECS
/// systems.
/// </summary>
public interface IECSWorldBuilder
{
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
