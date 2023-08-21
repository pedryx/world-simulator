namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Interface for a wrapper around an entity.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Determine if the entity has been destroyed.
    /// </summary>
    /// <returns>True, if the entity has been destroyed, otherwise false.</returns>
    bool IsDestroyed();

    void AddComponent<TComponent>(TComponent component)
        where TComponent : struct;

    /// <summary>
    /// Create an instance of the specified component type and add it to the entity.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to create and add.</typeparam>
    public void AddComponent<TComponent>()
        where TComponent : struct
        => AddComponent(new TComponent());

    void RemoveComponent<TComponent>()
        where TComponent : struct;

    ref TComponent GetComponent<TComponent>()
        where TComponent : struct;

    bool HasComponent<TComponent>()
        where TComponent : struct;

    /// <summary>
    /// Destroys the entity. The instance will still exist, but using it in any operation may lead to undefined
    /// behavior.
    /// </summary>
    void Destroy();

}