using System;

namespace WorldSimulator.ManagedDataManagers;

internal class RandomManager : ManagedDataManager<Random>
{
    public RandomManager() : base(true) { }

    protected override Random CreateDataInstance()
        => SeedGenerator.CreateRandom();

    protected override Random CreateEmpty()
        => null;
}
