using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
[Component]
internal struct Building
{
    public int VillageID = -1;

    public Building() { }
}
