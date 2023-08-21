using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS;
/// <summary>
/// Entity builder which will create an entity as a prototype and clone this prototype upon calling 
/// <see cref="Build(IECSWorld)"/>.
/// </summary>
public class CloneEntityBuilder : IEntityBuilder
{
    /// <summary>
    /// The ECS world into which the cloned entity will be placed.
    /// </summary>
    private readonly IECSWorld world;
    /// <summary>
    /// The prototype entity that serves as the basis for cloning.
    /// </summary>
    private readonly IEntity prototype;
    /// <summary>
    /// The function used for cloning the prototype entity.
    /// </summary>
    private readonly Func<IECSWorld, IEntity, IEntity> clone;


    /// <param name="world">The ECS world into which the cloned entity will be placed.</param>
    /// <param name="prototype">The prototype entity that serves as the basis for cloning.</param>
    /// <param name="clone">The function used for cloning the prototype entity.</param>
    public CloneEntityBuilder(IECSWorld world, IEntity prototype, Func<IECSWorld, IEntity, IEntity> clone)
    {
        this.world = world ?? throw new ArgumentNullException(nameof(world));
        this.prototype = prototype ?? throw new ArgumentNullException(nameof(prototype));
        this.clone = clone ?? throw new ArgumentNullException(nameof(clone));
    }

    /// <summary>
    /// Add component to the prototype entity.
    /// </summary>
    /// <param name="component">The component to add.</param>
    /// <typeparam name="TComponent">The type of component to add.</typeparam>
    public void AddComponent<TComponent>(TComponent component)
        where TComponent : struct 
        => prototype.AddComponent(component);

    /// <summary>
    /// Construct a new entity by cloning the prototype entity.
    /// </summary>
    public IEntity Build()
        => clone(world, prototype);
}
