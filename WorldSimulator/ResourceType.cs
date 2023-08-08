using System.Collections.Generic;
using System.Linq;

namespace WorldSimulator;
internal class ResourceType
{
}

internal static class ResourceTypes
{
    public readonly static ResourceType Tree = new();
    public readonly static ResourceType Rock = new();
    public readonly static ResourceType Deposite = new();
    public readonly static ResourceType Deer = new();

    public static IEnumerable<ResourceType> GetAllTypes()
        => typeof(ResourceTypes).GetFields().Select(p => (ResourceType)p.GetValue(null));
}