﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Systems;
using WorldSimulator.Systems.Villaages;
using WorldSimulator.Systems.Villages;
using WorldSimulator.UI.Elements;

namespace WorldSimulator.Level;
public class LevelState : GameState
{
    private readonly bool regenerateWorld;

    internal LevelFactory LevelFactory { get; private set; }
    internal GameWorld GameWorld { get; private set; }
    internal BehaviorTrees BehaviorTrees { get; private init; } = new();

    /// <param name="regenerateWorld">
    /// Determine if world should be regenerated (new terrain + new positions of resources).
    /// </param>
    public LevelState(bool regenerateWorld)
    {
        this.regenerateWorld = regenerateWorld;
    }

    protected override void CreateEntities()
    {
        LevelFactory = new LevelFactory(Game, this);
        GameWorldGenerator worldGenerator = new(Game, LevelFactory, regenerateWorld);
        Camera.Position = GameWorld.Size.ToVector2() / 2.0f;

        GameWorld = worldGenerator.Generate();
    }

    protected override IEnumerable<IECSSystem> CreateSystems()
    {
        return new List<IECSSystem>()
        {
            new DebugSystem(Game, Camera),
            new InputSystem(this),
            Game.Factory.CreateSystem(new AnimalBehaviorSystem(Game, GameWorld)),
            Game.Factory.CreateSystem(new MovementSystem()),
            Game.Factory.CreateSystem(new BehaviorSystem(GameWorld, BehaviorTrees, Game)),
            Game.Factory.CreateSystem(new PathFollowSystem(Game)),
            Game.Factory.CreateSystem(new DamageSystem(Game)),
            Game.Factory.CreateSystem(new DeathSystem(Game, GameWorld)),
            Game.Factory.CreateSystem(new ResourceProcessingSystem(Game)),
            Game.Factory.CreateSystem(new VillagerSpawningSystem(Game, LevelFactory, BehaviorTrees)),
            Game.Factory.CreateSystem(new VillageBuildingSystem(Game, this)),
            Game.Factory.CreateSystem(new HungerSystem()),
        };
    }

    protected override IEnumerable<IECSSystem> CreateRenderSystems()
    {
        return new List<IECSSystem>()
        {
            new TerrainRenderSystem(this),
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
