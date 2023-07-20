using System;
using System.Linq;
using System.Runtime.CompilerServices;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.ECS.Entitas;
internal static class ComponentIDGenerator
{
    private static int id = 0;
    private static int componentCount;

    public static int ComponentCount => componentCount;

    public static int NextID() => id++;

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