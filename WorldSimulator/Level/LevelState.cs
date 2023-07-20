using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Level;
internal class LevelState : GameState
{
    protected override void CreateEntities()
    {

    }

    protected override IEnumerable<IECSSystem> CreateSystems(IECSWorldBuilder builder)
    {
        return new List<IECSSystem>()
        {
        };
    }

    protected override IEnumerable<IECSSystem> CreateRenderSystems(IECSWorldBuilder builder)
    {
        return new List<IECSSystem>()
        {

        };
    }
}
