namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Interface for entity-component-system system. It is usually wrapper around <see cref="IEntityProcessor"/>. Each
/// ecs system has different way to iterate entities. It is responsibility of derived systems to call
/// <see cref="IEntityProcessor.PreUpdate(float)"/>, <see cref="IEntityProcessor.PostUpdate(float)"/> and
/// corresponding process methods. Derived systems has to have constructor which accept entity processor as single
/// argument.
/// </summary>
public interface IECSSystem
{
    /// <summary>
    /// Initialize ecs system.
    /// </summary>
    /// <param name="world">ECS world associated with the system.</param>
    void Initialize(IECSWorld world); 

    /// <summary>
    /// Update system's state.
    /// </summary>
    /// <param name="deltaTime">Time elapsed between frames.</param>
    void Update(float deltaTime);
}