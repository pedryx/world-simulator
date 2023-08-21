using System;
using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS;
/// <summary>
/// A simple entity builder that constructs an entity from added components based on the build method provided as an
/// argument. The build method will be invoked upon calling <see cref="Build(IECSWorld)"/>.
/// </summary>
public class OnPlaceBuildEntityBuilder : IEntityBuilder
{
    /// <summary>
    /// The types of components used for the construction of an entity.
    /// </summary>
    private readonly List<Type> types = new();
    /// <summary>
    /// The values of components used for the construction of an entity.
    /// </summary>
    private readonly List<object> values = new();
    /// <summary>
    /// The build method to construct an entity.
    /// </summary>
    private readonly Func<List<Type>, List<object>, IECSWorld, IEntity> build;
    /// <summary>
    /// The ECS world into which will be constructed entities placed.
    /// </summary>
    private readonly IECSWorld world;

    /// <param name="world">The ECS world into which will be constructed entities placed.</param>
    /// <param name="build">The build method to construct an entity.</param>
    public OnPlaceBuildEntityBuilder(IECSWorld world, Func<List<Type>, List<object>, IECSWorld, IEntity> build)
    {
        this.build = build ?? throw new ArgumentNullException(nameof(build));
        this.world = world ?? throw new ArgumentNullException(nameof(world));
    }

    public void AddComponent<TComponent>(TComponent component)
        where TComponent : struct
    {
        types.Add(component.GetType());
        values.Add(component);
    }

    public IEntity Build()
        => build(types, values, world);
}
