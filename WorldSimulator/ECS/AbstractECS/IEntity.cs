namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Represent an ECS entity.
/// </summary>
public interface IEntity
{
    void AddComponent<TComponent>(TComponent component);

    public void AddComponent<TComponent>()
        where TComponent : new()
        => AddComponent(new TComponent());

    void RemoveComponent<TComponent>();

    ref TComponent GetComponent<TComponent>();

    bool HasComponent<TComponent>();

    void Destroy();
}