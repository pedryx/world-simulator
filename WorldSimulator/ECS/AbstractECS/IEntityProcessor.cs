namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// The base interface for entity processors. An entity processor is typically wrapped within an ECS system, which
/// often implements the <see cref="IECSSystem"/> interface. The system calls the Process method of the entity
/// processor for each entity. Outside of the <see cref="AbstractECS"/> namespace, entity processors are commonly
/// referred to as "systems."
/// </summary>
public interface IEntityProcessor
{
    /// <summary>
    /// Occur before the processing of entities begins.
    /// </summary>
    /// <param name="deltaTime">The time elapsed between frames.</param>
    virtual void PreUpdate(float deltaTime) { }
    /// <summary>
    /// Occur after the processing of entities ends.
    /// </summary>
    /// <param name="deltaTime">The time elapsed between frames.</param>
    virtual void PostUpdate(float deltaTime) { }
}

/// <summary>
/// Entity processor for processing component tuples of size one. For more information see
/// <see cref="IEntityProcessor"/>.
/// </summary>
public interface IEntityProcessor<TComponent> : IEntityProcessor
    where TComponent : unmanaged
{
    void Process(ref TComponent component, float deltaTime);
}

/// <summary>
/// Entity processor for processing component tuples of size two. For more information see
/// <see cref="IEntityProcessor"/>.
/// </summary>
public interface IEntityProcessor<TComponent1, TComponent2> : IEntityProcessor
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
{
    void Process(ref TComponent1 component1, ref TComponent2 component2, float deltaTime);
}

/// <summary>
/// Entity processor for processing component tuples of size three. For more information see
/// <see cref="IEntityProcessor"/>.
/// </summary>
public interface IEntityProcessor<TComponent1, TComponent2, TComponent3> : IEntityProcessor
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
{
    void Process(ref TComponent1 component1, ref TComponent2 component2, ref TComponent3 component3, float deltaTime);
}

/// <summary>
/// Entity processor for processing component tuples of size four. For more information see
/// <see cref="IEntityProcessor"/>.
/// </summary>
public interface IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> : IEntityProcessor
    where TComponent1 : unmanaged
    where TComponent2 : unmanaged
    where TComponent3 : unmanaged
    where TComponent4 : unmanaged
{
    void Process
    (
        ref TComponent1 component1,
        ref TComponent2 component2,
        ref TComponent3 component3,
        ref TComponent4 component4,
        float deltaTime
    );
}
                                                                                                                       