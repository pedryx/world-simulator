using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
[Component]
internal struct Resource 
{
    public ResourceType Type;

    public Resource(ResourceType type)
    {
        Type = type;
    }
}
