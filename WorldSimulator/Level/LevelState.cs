using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Systems;
using WorldSimulator.UI.Elements;

namespace WorldSimulator.Level;
public class LevelState : GameState
{
    internal LevelFactory LevelFactory { get; private set; }
    internal GameWorld GameWorld { get; private set; }

    protected override void CreateEntities()
    {
        LevelFactory = new LevelFactory(Game, this);
        GameWorldGenerator worldGenerator = new(Game, LevelFactory);
        Camera.Position = new Vector2(GameWorld.Size) / 2.0f;

        GameWorld = worldGenerator.Generate();
    }

    protected override IEnumerable<IECSSystem> CreateSystems()
    {
        return new List<IECSSystem>()
        {
            new DebugSystem(Game, Camera),
            new InputSystem(this),
            Game.Factory.CreateSystem(new AnimalControllerSystem(Game, GameWorld)),
            Game.Factory.CreateSystem(new MovementSystem()),
        };
    }

    protected override IEnumerable<IECSSystem> CreateRenderSystems()
    {
        return new List<IECSSystem>()
        {
            Game.Factory.CreateSystem(new OrientationUpdateSystem()),
            Game.Factory.CreateSystem(new RenderSystem(Game, Camera)),
        };
    }

    protected override void CreateUI()
    {
        Texture2D bigBorder = Game.GetResourceManager<Texture2D>()["ui border big"];

        UILayer.AddElement(new Minimap(this, new Vector2(300.0f, 300.0f), bigBorder)
        {
            Offset = new Vector2(Game.DefaultResolution.X - 300.0f - 5.0f, 5.0f),
        });

        base.CreateUI();
    }
}
