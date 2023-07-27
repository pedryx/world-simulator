using System;
using System.Linq;

namespace WorldSimulator.ECS.AbstractECS;
/// <summary>
/// Factory for creation of ECS related classes.
/// </summary>
public abstract class ECSFactory
{
    /// <summary>
    /// Type for systems for component tuples of size one.
    /// </summary>
    private readonly Type oneTypeSystem;
    /// <summary>
    /// Type for systems for component tuples of size two.
    /// </summary>
    private readonly Type twoTypeSystem;
    /// <summary>
    /// Type for systems for component tuples of size three.
    /// </summary>
    private readonly Type threeTypeSystem;
    /// <summary>
    /// Type for systems for component tuples of size four.
    /// </summary>
    private readonly Type fourTypeSystem;

    /// <param name="oneTypeSystem">Type for systems for component tuples of size one.</param>
    /// <param name="twoTypeSystem">Type for systems for component tuples of size two.</param>
    /// <param name="threeTypeSystem">Type for systems for component tuples of size three.</param>
    /// <param name="fourTypeSystem">Type for systems for component tuples of size four.</param>
    public ECSFactory(Type oneTypeSystem, Type twoTypeSystem, Type threeTypeSystem, Type fourTypeSystem)
    {
        this.oneTypeSystem = oneTypeSystem;
        this.twoTypeSystem = twoTypeSystem;
        this.threeTypeSystem = threeTypeSystem;
        this.fourTypeSystem = fourTypeSystem;
    }

    /// <summary>
    /// Create builder for entities.
    /// </summary>
    /// <param name="world">World in which will be builder placing created entities.</param>
    /// <returns></returns>
    public abstract IEntityBuilder CreateEntityBuilder(IECSWorld world);

    /// <summary>
    /// Create new ECS world.
    /// </summary>
    public abstract IECSWorld CreateWorld();

    /// <summary>
    /// Create new ecs system which wrappes around entity processor.
    /// </summary>
    public IECSSystem CreateSystem<TEntityProcessor>(TEntityProcessor entityProcessor)
        where TEntityProcessor : struct, IEntityProcessor
    {
        Type[] processorArguments = typeof(TEntityProcessor)
            .GetInterfaces()
            .Where(i => i.GetInterface(nameof(IEntityProcessor)) != null)
            .First()
            .GetGenericArguments();
        Type[] systemArguments = new Type[] { typeof(TEntityProcessor) }
            .Union(processorArguments)
            .ToArray();

        Type systemType = processorArguments.Length switch
        {
            1 => oneTypeSystem,
            2 => twoTypeSystem,
            3 => threeTypeSystem,
            4 => fourTypeSystem,
            _ => throw new InvalidOperationException("Systems for component tuples of this size are not supported!"),
        };
        Type constructed = systemType.MakeGenericType(systemArguments);

        return (IECSSystem)Activator.CreateInstance(constructed, entityProcessor);
    }
}
