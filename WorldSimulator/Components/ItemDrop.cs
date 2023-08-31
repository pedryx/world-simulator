using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Entities with this component will drop items on dead.
/// </summary>
[Component]
internal struct ItemDrop
{
    public ItemCollection Items = new();

    public ItemDrop() { }
}
