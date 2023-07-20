namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Represent an ECS entity.
/// </summary>
public interface IEntity
{
    void AddComponent<TComponent>(TComponent component);

    /// <summary>
    /// Create instance of specific component type and add it to entity.
    /// </summary>
    /// <typeparam name="TComponent">Type of component to create and add.</typeparam>
    public void AddComponent<TComponent>()
        where TComponent : new()
        => AddComponent(new TComponent());

    void RemoveComponent<TComponent>();

    ref TComponent GetComponent<TComponent>();

    bool HasComponent<TComponent>();

    /// <summary>
    /// Destroy entity. (Entity will no longer be managed by its ecs world and all
    /// components will no longer be valid.)
    /// </summary>
    void Destroy();
}