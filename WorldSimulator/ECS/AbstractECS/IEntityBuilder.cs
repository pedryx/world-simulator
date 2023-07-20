namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Builder which can be used to build ECS entities (<see cref="IEntity"/>) from
/// ECS components.
/// </summary>
public interface IEntityBuilder
{
    void AddComponent<TComponent>(TComponent component);

    public void AddComponent<TComponent>()
        where TComponent : new()
        => AddComponent(new TComponent());

    /// <summary>
    /// Build ECS entity (<see cref="IEntity"/>) from added ECS components.
    /// </summary>
    /// <returns></returns>
    IEntity Build(IECSWorld world);
}
