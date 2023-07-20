namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Builder which can be used to build ECS worlds (<see cref="IECSWorld"/>) from ECS
/// systems.
/// </summary>
public interface IECSWorldBuilder
{
    IECSSystem AddSystem<TComponent>
    (
        EntityProcessor<TComponent> processor
    );

    IECSSystem AddSystem<TComponent1, TComponent2>
    (
        EntityProcessor<TComponent1, TComponent2> processor
    );

    IECSSystem AddSystem<TComponent1, TComponent2, TComponent3>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3> processor
    );

    IECSSystem AddSystem<TComponent1, TComponent2, TComponent3, TComponent4>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor
    );

    /// <summary>
    /// Build ECS world (<see cref="IECSWorld"/>) from added ECS systems.
    /// </summary>
    IECSWorld Build();
}
