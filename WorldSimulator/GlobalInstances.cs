using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace WorldSimulator;

internal abstract class GlobalInstances
{
    [ModuleInitializer]
    public static void Initialize()
    {
        var query = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => typeof(GlobalInstances).IsAssignableFrom(t));

        foreach (var type in query)
        {
            RuntimeHelpers.RunClassConstructor(type.TypeHandle);
        }
    }
}

/// <summary>
/// Represent base class for classes which will have inly limited amount of instances with global access to them.
/// </summary>
/// <typeparam name="TDerived">Type derived from <see cref="GlobalInstances{TDerived}"/>.</typeparam>
internal abstract class GlobalInstances<TDerived> : GlobalInstances
    where TDerived : GlobalInstances<TDerived>
{
    /// <summary>
    /// Contains all instances.
    /// </summary>
    private static readonly List<TDerived> items = new();

    /// <summary>
    /// Get number of instances.
    /// </summary>
    public static int Count => items.Count;

    /// <summary>
    /// Get instance with specific id.
    /// </summary>
    /// <param name="id">ID of instance to get.</param>
    public static TDerived Get(int id)
        => items[id];

    /// <summary>
    /// Instance id.
    /// </summary>
    public int ID { get; private init; } = items.Count;

    public GlobalInstances()
    {
        items.Add((TDerived)this);
    }

    /// <summary>
    /// Get all instances.
    /// </summary>
    public static IEnumerable<TDerived> GetAll()
        => items;
}
