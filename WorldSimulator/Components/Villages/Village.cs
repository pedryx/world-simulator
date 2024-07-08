using System;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components.Villages;
[Component]
internal struct Village
{
    public const int MaxBuildingCount = 64;

    public int MainBuildingID = -1;
    public int StockpileID = -1;

    public int BuildingsArrayID = -1;
    public int BuildingsCount = 0;

    public int BuildOrderIndex = 0;

    /// <summary>
    /// ID of Random number generator for generating random positions for new buildings.
    /// </summary>
    public int RandomID = -1;

    public Village(Game game)
    {
        BuildingsArrayID = game.GetManagedDataManager<IEntity[]>().Insert(new IEntity[MaxBuildingCount]);
        RandomID = game.GetManagedDataManager<Random>().Create();
    }
}
