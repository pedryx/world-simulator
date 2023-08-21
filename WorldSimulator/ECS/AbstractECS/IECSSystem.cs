namespace WorldSimulator.ECS.AbstractECS;

/// <summary> 
/// An interface for an ECS system. Typically, it wraps around <see cref="IEntityProcessor"/>. Each ECS system has a
/// distinct method to iterate through entities. Derived systems are responsible for invoking
/// <see cref="IEntityProcessor.PreUpdate(float)"/>, <see cref="IEntityProcessor.PostUpdate(float)"/> methods and the
/// relevant Process methods. Derived systems must have a constructor that accepts an entity processor as its sole
/// argument.
/// </summary>
public interface IECSSystem
{
    /// <summary>
    /// Initialize the ECS system.
    /// </summary>
    /// <param name="world">An ECS world associated with the system.</param>
    void Initialize(IECSWorld world); 

    /// <summary>
    /// Update the state of the system.
    /// </summary>
    /// <param name="deltaTime">Time elapsed between frames.</param>
    void Update(float deltaTime);
}