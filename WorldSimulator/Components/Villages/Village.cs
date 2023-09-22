using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
[Component]
internal struct Village
{
    public const int MaxVillagerCount = 64;

    public IEntity MainBuilding;
    public IEntity StockPile;
    public IEntity WoodcutterHut;
    public IEntity MinerHut;
    public IEntity Smithy;
    public IEntity HunterHut;

    public IEntity[] Houses = new IEntity[MaxVillagerCount];
    public int HouseCount = 0;

    public IEntity[] UnemployedVillagers = new IEntity[MaxVillagerCount];
    public int UnemployedVillagerCount = 0;

    public int BuildOrderIndex;

    public Village() { }
}
