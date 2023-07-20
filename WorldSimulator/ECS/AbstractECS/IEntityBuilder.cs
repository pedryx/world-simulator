namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Builder which can be used to build ECS entities (<see cref="IEntity"/>) from
/// ECS components.
/// </summary>
public interface IEntityBuilder
{
    void AddComponent<TComponent>(TComponent component);

    /// <summary>
    /// Create new instance of specific component type and add it to entity builder.
    /// </summary>
    /// <typeparam name="TComponent">Type of component to create and add.</typeparam>
    public void AddComponent<TComponent>()
        where TComponent : new()
        => AddComponent(new TComponent());

    /// <summary>
    /// Build ECS entity (<see cref="IEntity"/>) with all added ECS components.
    /// </summary>
    IEntity Build(IECSWorld world);
}
