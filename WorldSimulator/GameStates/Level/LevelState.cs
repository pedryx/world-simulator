using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Systems;

namespace WorldSimulator.GameStates.Level;
public class LevelState : GameState
{
    public LevelFactory LevelFactory { get; private set; }

    protected override void CreateEntities()
    {
        LevelFactory = new LevelFactory(Game, this);
        WorldGenerator worldGenerator = new(Game, LevelFactory);

        worldGenerator.Generate();
    }

    protected override IEnumerable<IECSSystem> CreateSystems()
    {
        return new List<IECSSystem>()
        {
            new InputSystem(Game, Camera),
        };
    }

    protected override IEnumerable<IECSSystem> CreateRenderSystems()
    {
        return new List<IECSSystem>()
        {
            Game.Factory.CreateSystem(new RenderClipSystem(Game, Camera)),
            Game.Factory.CreateSystem(new RenderSystem(Game.SpriteBatch, Camera)),
        };
    }
}
