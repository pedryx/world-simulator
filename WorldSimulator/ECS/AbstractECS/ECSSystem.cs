namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Base class for entity-component-system system.
/// </summary>
public abstract class ECSSystem
{
    public virtual void Initialize() { }

    /// <summary>
    /// Update system's state.
    /// </summary>
    /// <param name="deltaTime">Time elapsed between frames.</param>
    public abstract void Update(float deltaTime);
}

/// <summary>
/// base class for entity-component-system system specialized for specific entity processor.
/// Each ecs system has one processor. Class derived from this is responsible for iterating
/// all entities with associated components and calling processor's process method on them.
/// </summary>
public abstract class ECSSystem<TEntityProcessor> : ECSSystem
    where TEntityProcessor : EntityProcessor
{
    protected TEntityProcessor Processor { get; private set; }

    public void Initialize(Game game, GameState gameState, TEntityProcessor processor)
    {
        Processor = processor;
        processor.Initialize(game, gameState);
        Initialize();
    }
}
