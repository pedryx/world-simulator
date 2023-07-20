namespace WorldSimulator.ECS.AbstractECS;

// TODO: dont pass world on build, world will be passed by for example constructors

/// <summary>
/// Builder which can be used to build ECS entities (<see cref="IEntity"/>) from
/// ECS components.
/// </summary>
public interface IEntityBuilder
{
    void AddComponent<TComponent>(TComponent component)
        where TComponent : struct;

    /// <summary>
    /// Create new instance of specific component type and add it to entity builder.
    /// </summary>
    /// <typeparam name="TComponent">Type of component to create and add.</typeparam>
    public void AddComponent<TComponent>()
        where TComponent : struct
        => AddComponent(new TComponent());

    /// <summary>
    /// Build ECS entity (<see cref="IEntity"/>) with all added ECS components.
    /// </summary>
    IEntity Build();
}
