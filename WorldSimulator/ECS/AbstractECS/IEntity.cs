namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Represent an ECS entity.
/// </summary>
public interface IEntity
{
    void AddComponent<TComponent>(TComponent component)
        where TComponent : struct;

    /// <summary>
    /// Create instance of specific component type and add it to entity.
    /// </summary>
    /// <typeparam name="TComponent">Type of component to create and add.</typeparam>
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
    /// Destroy entity. (Entity will no longer be managed by its ecs world and all
    /// components will no longer be valid.)
    /// </summary>
    void Destroy();
}