namespace WorldSimulator.ECS.AbstractECS;

// TODO: incosistent style in world builders

/// <summary>
/// Builder which can be used to build ECS worlds (<see cref="IECSWorld"/>) from ECS
/// systems.
/// </summary>
public interface IECSWorldBuilder
{
    IECSSystem AddSystem<TComponent>
    (
        EntityProcessor<TComponent> processor
    )
        where TComponent : struct;

    IECSSystem AddSystem<TComponent1, TComponent2>
    (
        EntityProcessor<TComponent1, TComponent2> processor
    )
        where TComponent1 : struct
        where TComponent2 : struct;

    IECSSystem AddSystem<TComponent1, TComponent2, TComponent3>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3> processor
    )
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct;

    IECSSystem AddSystem<TComponent1, TComponent2, TComponent3, TComponent4>
    (
        EntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> processor
    )
        where TComponent1 : struct
        where TComponent2 : struct
        where TComponent3 : struct
        where TComponent4 : struct;

    /// <summary>
    /// Build ECS world (<see cref="IECSWorld"/>) from added ECS systems.
    /// </summary>
    IECSWorld Build();
}
