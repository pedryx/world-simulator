namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Represent base interface for entity processors. Processor is responsible for processing
/// entities, class derived from <see cref="ECSSystem{TEntityProcessor}"/> is responsible
/// to call processor's process method on each associated entity.
/// </summary>
public abstract class EntityProcessor 
{
    protected Game Game { get; private set; }
    protected GameState GameState { get; private set; }

    public EntityProcessor(Game game, GameState gameState)
    {
        Game = game;
        GameState = gameState;
    }

    /// <summary>
    /// Occur at the start of system update call.
    /// </summary>
    /// <param name="deltaTime">Elapsed time between frames.</param>
    public virtual void PreUpdate(float deltaTime) { }
    /// <summary>
    /// Occur at the end of system update call.
    /// </summary>
    /// <param name="deltaTime">Elapsed time between frames.</param>
    public virtual void PostUpdate(float deltaTime) { }
}

/// <summary>
/// Represent system for processing component tuples with size one.
/// </summary>
/// <typeparam name="TComponent">Type of first component.</typeparam>
public abstract class EntityProcessor<TComponent> : EntityProcessor
{
    protected EntityProcessor(Game game, GameState gameState) 
        : base(game, gameState) { }

    /// <summary>
    /// Process component tuple.
    /// </summary>
    /// <param name="component">First component.</param>
    /// <param name="deltaTime">Elapsed time between frames.</param>
    public abstract void Process(ref TComponent component, float deltaTime);
}

/// <summary>
/// Represent system for processing component tuples with size two.
/// </summary>
/// <typeparam name="TComponent1">Type of first component.</typeparam>
/// <typeparam name="TComponent2">Type of second component.</typeparam>
public abstract class EntityProcessor<TComponent1, TComponent2> : EntityProcessor
{
    protected EntityProcessor(Game game, GameState gameState) 
        : base(game, gameState) { }

    /// <summary>
    /// Process component tuple.
    /// </summary>
    /// <param name="component1">First component.</param>
    /// <param name="component2">Second component.</param>
    /// <param name="deltaTime">Elapsed time between frames.</param>
    public abstract void Process
    (
        ref TComponent1 component1,
        ref TComponent2 component2,
        float deltaTime
    );
}

/// <summary>
/// Represent system for processing component tuples with size three.
/// </summary>
/// <typeparam name="TComponent1">Type of first component.</typeparam>
/// <typeparam name="TComponent2">Type of second component.</typeparam>
/// <typeparam name="TComponent3">Type of third component.</typeparam>
public abstract class EntityProcessor<TComponent1, TComponent2, TComponent3> : EntityProcessor
{
    protected EntityProcessor(Game game, GameState gameState) 
        : base(game, gameState) { }

    /// <summary>
    /// Process component tuple.
    /// </summary>
    /// <param name="component1">First component.</param>
    /// <param name="component2">Second component.</param>
    /// <param name="component3">Third component.</param>
    /// <param name="deltaTime">Elapsed time between frames.</param>
    public abstract void Process
    (
        ref TComponent1 component1,
        ref TComponent2 component2,
        ref TComponent3 component3,
        float deltaTime
    );
}

/// <summary>
/// Represent system for processing component tuples with size four.
/// </summary>
/// <typeparam name="TComponent1">Type of first component.</typeparam>
/// <typeparam name="TComponent2">Type of second component.</typeparam>
/// <typeparam name="TComponent3">Type of third component.</typeparam>
/// <typeparam name="TComponent4">Type of fourth component.</typeparam>
public abstract class EntityProcessor
<
    TComponent1,
    TComponent2,
    TComponent3,
    TComponent4
> : EntityProcessor
{
    protected EntityProcessor(Game game, GameState gameState) 
        : base(game, gameState) { }

    /// <summary>
    /// Process component tuple.
    /// </summary>
    /// <param name="component1">First component.</param>
    /// <param name="component2">Second component.</param>
    /// <param name="component3">Third component.</param>
    /// <param name="component4">Fourth component.</param>
    /// <param name="deltaTime">Elapsed time between frames.</param>
    public abstract void Process
    (
        ref TComponent1 component1,
        ref TComponent2 component2,
        ref TComponent3 component3,
        ref TComponent4 component4,
        float deltaTime
    );
}