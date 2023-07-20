﻿using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Systems;

namespace WorldSimulator.GameStates.Level;
internal class LevelState : GameState
{
    public LevelFactory LevelFactory { get; private set; }

    protected override void CreateEntities()
    {
        LevelFactory = new LevelFactory(Game, this);

        Texture2D texture = Texture2D.FromFile(Game.GraphicsDevice, "test.png");

        var builder = Game.Factory.CreateEntityBuilder();
        builder.AddComponent<Transform>();
        builder.AddComponent(new Appearance(texture));
        var entity = builder.Build(ECSWorld);
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
            builder.AddSystem(new RenderSystem(Game, this)),
        };
    }
}