using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
[Component]
internal struct Resource 
{
    public int TypeID = -1;

    public Resource(ResourceType type)
    {
        TypeID = type.ID;
    }
}
