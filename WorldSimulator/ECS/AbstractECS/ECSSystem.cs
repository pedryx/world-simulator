namespace WorldSimulator.ECS.AbstractECS;

public abstract class ECSSystem
{
    public virtual void Initialize() { }

    /// <summary>
    /// Update system's state.
    /// </summary>
    /// <param name="deltaTime">Time elapsed between frames.</param>
    public abstract void Update(float deltaTime);
}

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
