using System;
using System.Linq;

namespace WorldSimulator.ECS.AbstractECS;
/// <summary>
/// The factory for creating ECS-related classes.
/// </summary>
public abstract class ECSFactory
{
    /// <summary>
    /// The type for systems handling component tuples of size one.
    /// </summary>
    private readonly Type oneTypeSystem;
    /// <summary>
    /// The type for systems handling component tuples of size two.
    /// </summary>
    private readonly Type twoTypeSystem;
    /// <summary>
    /// The type for systems handling component tuples of size three.
    /// </summary>
    private readonly Type threeTypeSystem;
    /// <summary>
    /// The type for systems handling component tuples of size four.
    /// </summary>
    private readonly Type fourTypeSystem;

    /// <param name="oneTypeSystem">The type for systems handling component tuples of size one.</param>
    /// <param name="twoTypeSystem">The type for systems handling component tuples of size two.</param>
    /// <param name="threeTypeSystem">The type for systems handling component tuples of size three.</param>
    /// <param name="fourTypeSystem">The type for systems handling component tuples of size four.</param>
    public ECSFactory(Type oneTypeSystem, Type twoTypeSystem, Type threeTypeSystem, Type fourTypeSystem)
    {
        this.oneTypeSystem = oneTypeSystem ?? throw new ArgumentNullException(nameof(oneTypeSystem));
        this.twoTypeSystem = twoTypeSystem ?? throw new ArgumentNullException(nameof(twoTypeSystem));
        this.threeTypeSystem = threeTypeSystem ?? throw new ArgumentNullException(nameof(threeTypeSystem));
        this.fourTypeSystem = fourTypeSystem ?? throw new ArgumentNullException(nameof(fourTypeSystem));
    }

    /// <summary>
    /// Create a builder for entities.
    /// </summary>
    /// <param name="world">The world in which the builder will place created entities.</param>
    public abstract IEntityBuilder CreateEntityBuilder(IECSWorld world);

    /// <summary>
    /// Create a new ECS world.
    /// </summary>
    public abstract IECSWorld CreateWorld();

    /// <summary>
    /// Create an ECS system from an entity processor.
    /// </summary>
    internal  IECSSystem CreateSystem<TEntityProcessor>(TEntityProcessor entityProcessor)
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
