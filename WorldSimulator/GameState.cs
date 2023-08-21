using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.UI;

namespace WorldSimulator;
/// <summary>
/// Represent a state of the game ("game screen"). Can be used to represent a menu, settings, credits, level, etc.
/// </summary>
public abstract class GameState
{
    /// <summary>
    /// Contains systems handled in <see cref="Update(float)"/>.
    /// </summary>
    private IEnumerable<IECSSystem> systems;
    /// <summary>
    /// Contains systems handled in <see cref="Draw(float)"/>.
    /// </summary>
    private IEnumerable<IECSSystem> renderSystems;

    internal Game Game { get; private set; }
    internal IECSWorld ECSWorld { get; private set; }
    internal Camera Camera { get; private set; }
    internal UILayer UILayer { get; private set; }

    protected abstract void CreateEntities();
    /// <summary>
    /// Create processors for systems which will be handled in <see cref="Update(float)"/>.
    /// </summary>
    protected abstract IEnumerable<IECSSystem> CreateSystems();
    /// <summary>
    /// Create processors for systems which will be handled in <see cref="Draw(float)"/>.
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerable<IECSSystem> CreateRenderSystems();
    protected virtual void CreateUI() { }

    internal void Initialize(Game game)
    {
        Game = game;
        ECSWorld = Game.Factory.CreateWorld();
        Camera = new Camera();
        UILayer = new UILayer(this);

        CreateEntities();
        systems = CreateSystems();
        renderSystems = CreateRenderSystems();
        CreateUI();

        foreach (var system in systems)
        {
            system.Initialize(ECSWorld);
        }
        foreach (var system in renderSystems)
        {
            system.Initialize(ECSWorld);
        }
    }

    internal void Update(float deltaTime)
    {
        foreach (var system in systems)
        {
            system.Update(deltaTime);
        }

        ECSWorld.Update();
        UILayer.Update(deltaTime);
    }

    internal void Draw(float deltaTime)
    {
        foreach (var system in renderSystems)
        {
            system.Update(deltaTime);
        }

        UILayer.Draw(deltaTime);
    }
}
