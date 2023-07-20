namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Base class for entity-component-system system.
/// </summary>
public interface IECSSystem
{
    void Initialize(IECSWorld world); 

    /// <summary>
    /// Update system's state.
    /// </summary>
    /// <param name="deltaTime">Time elapsed between frames.</param>
    void Update(float deltaTime);
}