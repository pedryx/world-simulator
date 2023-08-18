using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
[Component]
internal struct Inventory
{
    public int[] Slots;

    public Inventory()
    {
        Slots = new int[Items.Count];
    }
}
