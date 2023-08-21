namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// An interface for a builder which can be used to construct entities (<see cref="IEntity"/>) from components.
/// </summary>
public interface IEntityBuilder
{
    void AddComponent<TComponent>(TComponent component)
        where TComponent : struct;

    /// <summary>
    /// Create a new instance of the specified component type and add it to the entity builder.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to create and add.</typeparam>
    public void AddComponent<TComponent>()
        where TComponent : struct
        => AddComponent(new TComponent());

    /// <summary>
    /// Constructs an entity (<see cref="IEntity"/>) from all added components.
    /// </summary>
    IEntity Build();
}
