namespace WorldSimulator.ECS.AbstractECS;

/// <summary>
/// Base interface for entity processors. Entity processor is ussually wrpped in some system which ussually implements
/// interface <see cref="IECSSystem"/>. The system will call Process method of entity processor on each entity. In
/// other places than <see cref="AbstractECS"/> namespace it is refered to entity processors as "systems".
/// </summary>
public interface IEntityProcessor
{
    /// <summary>
    /// Occur before processing of entities begins.
    /// </summary>
    /// <param name="deltaTime">Time ellapsed between frames.</param>
    virtual void PreUpdate(float deltaTime) { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="deltaTime"></param>
    virtual void PostUpdate(float deltaTime) { }
}

/// <summary>
/// Entity processor for processing component tuples of size one. For more information see
/// <see cref="IEntityProcessor"/>.
/// </summary>
public interface IEntityProcessor<TComponent> : IEntityProcessor
    where TComponent : struct
{
    void Process(ref TComponent component, float deltaTime);
}

/// <summary>
/// Entity processor for processing component tuples of size two. For more information see
/// <see cref="IEntityProcessor"/>.
/// </summary>
public interface IEntityProcessor<TComponent1, TComponent2> : IEntityProcessor
    where TComponent1 : struct
    where TComponent2 : struct
{
    void Process(ref TComponent1 component1, ref TComponent2 component2, float deltaTime);
}

/// <summary>
/// Entity processor for processing component tuples of size three. For more information see
/// <see cref="IEntityProcessor"/>.
/// </summary>
public interface IEntityProcessor<TComponent1, TComponent2, TComponent3> : IEntityProcessor
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
{
    void Process(ref TComponent1 component1, ref TComponent2 component2, ref TComponent3 component3, float deltaTime);
}

/// <summary>
/// Entity processor for processing component tuples of size four. For more information see
/// <see cref="IEntityProcessor"/>.
/// </summary>
public interface IEntityProcessor<TComponent1, TComponent2, TComponent3, TComponent4> : IEntityProcessor
    where TComponent1 : struct
    where TComponent2 : struct
    where TComponent3 : struct
    where TComponent4 : struct
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
                                                                                                                       