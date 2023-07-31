using Microsoft.Xna.Framework;

using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Systems;

namespace WorldSimulator.Level;
public class LevelState : GameState
{
    internal LevelFactory LevelFactory { get; private set; }
    internal GameWorld GameWorld { get; private set; }

    protected override void CreateEntities()
    {
        LevelFactory = new LevelFactory(Game, this);
        GameWorldGenerator worldGenerator = new(Game, LevelFactory);
        Camera.Position = new Vector2(GameWorldGenerator.WorldSize) / 2.0f;

        GameWorld = worldGenerator.Generate();
    }

    protected override IEnumerable<IECSSystem> CreateSystems()
    {
        return new List<IECSSystem>()
        {
            new InputSystem(Game, Camera, GameWorld),
            Game.Factory.CreateSystem(new AnimalControllerSystem(Game, GameWorld)),
            Game.Factory.CreateSystem(new PathFollowSystem()),
            Game.Factory.CreateSystem(new MovementSystem()),
        };
    }

    protected override IEnumerable<IECSSystem> CreateRenderSystems()
    {
        return new List<IECSSystem>()
        {
            Game.Factory.CreateSystem(new OrientationUpdateSystem()),
            Game.Factory.CreateSystem(new RenderClipSystem(Game, Camera)),
            Game.Factory.CreateSystem(new LayerUpdateSystem(Game, Camera)),
            Game.Factory.CreateSystem(new RenderSystem(Game.SpriteBatch, Camera)),
        };
    }
}
