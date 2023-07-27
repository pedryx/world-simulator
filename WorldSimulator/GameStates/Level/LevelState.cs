using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Systems;

namespace WorldSimulator.GameStates.Level;
public class LevelState : GameState
{
    public LevelFactory LevelFactory { get; private set; }

    protected override void CreateEntities()
    {
        LevelFactory = new LevelFactory(Game, this);

        Texture2D texture = Game.GetResourceManager<Texture2D>()["test"];

        var builder = Game.Factory.CreateEntityBuilder(ECSWorld);
        builder.AddComponent<Transform>();
        builder.AddComponent(new Appearance(texture));
        var entity = builder.Build();
    }

    protected override IEnumerable<IECSSystem> CreateSystems()
    {
        return new List<IECSSystem>()
        {

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
