namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Represent base interface for entity processors.
/// </summary>
public abstract class EntityProcessor 
{
    protected Game Game { get; private set; }
    protected GameState GameState { get; private set; }

    public void Initialize(Game game, GameState gameState)
    {
        Game = game;
        GameState = gameState;
        Initialize();
    }

    public virtual void Initialize() { }
    public virtual void PreProcess(float deltaTime) { }
    public virtual void PostProcess(float deltaTime) { }
}

/// <summary>
/// Represent system for processing component tuples with size one.
/// </summary>
/// <typeparam name="TComponent">Type of first component.</typeparam>
public abstract class EntityProcessor<TComponent> : EntityProcessor
{
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