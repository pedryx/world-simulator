using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator;
/// <summary>
/// Represent state of the game ("game screen"). Can be used to represent menu, settings,
/// credits, level, etc.
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

    public Game Game { get; private set; }
    public IECSWorld ECSWorld { get; private set; }
    public Camera Camera { get; private set; }

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

    public void Initialize(Game game)
    {
        Game = game;
        ECSWorld = Game.Factory.CreateWorld();
        Camera = new Camera(game);

        systems = CreateSystems();
        renderSystems = CreateRenderSystems();

        foreach (var system in systems)
        {
            system.Initialize(ECSWorld);
        }
        foreach (var system in renderSystems)
        {
            system.Initialize(ECSWorld);
        }

        CreateEntities();
    }

    public void Update(float deltaTime)
    {
        foreach (var system in systems)
        {
            system.Update(deltaTime);
        }

        ECSWorld.Update();
    }

    public void Draw(float deltaTime)
    {
        foreach (var system in renderSystems)
        {
            system.Update(deltaTime);
        }
    }
}
