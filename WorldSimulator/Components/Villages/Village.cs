using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
[Component]
internal struct Village
{
    public const int MaxBuildingCount = 64;

    public IEntity MainBuilding;
    public IEntity Stockpile;

    public IEntity[] Buildings = new IEntity[MaxBuildingCount];
    public int BuildingsCount = 0;

    public int BuildOrderIndex = 0;

    public Village() { }
}
