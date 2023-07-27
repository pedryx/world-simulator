using System;
using System.Linq;
using System.Runtime.CompilerServices;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Entitas;
/// <summary>
/// Utility class for generting IDs for components.
/// </summary>
internal static class ComponentIDGenerator
{
    /// <summary>
    /// ID which will be asigned to next component.
    /// </summary>
    private static int id = 0;
    /// <summary>
    /// Total number of components in all loaded assemblies.
    /// </summary>
    private static int componentCount;

    /// <summary>
    /// Total number of components in all loaded assemblies.
    /// </summary>
    public static int ComponentCount => componentCount;

    /// <summary>
    /// Generate next component ID.
    /// </summary>
    /// <returns>Generated new component ID.</returns>
    public static int NextID() => id++;

    /// <summary>
    /// Count total number of components in all loaded assemblies.
    /// </summary>
    [ModuleInitializer]
    public static void CountComponents()
    {
        componentCount = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                          from type in assembly.GetTypes()
                          where type.IsValueType
                          where type.IsDefined(typeof(ComponentAttribute), false)
                          select type).Count();
    }
}