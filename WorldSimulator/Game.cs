using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Resources;

using MonoGameBaseGame = Microsoft.Xna.Framework.Game;

namespace WorldSimulator;
/// <summary>
/// Main game class.
/// </summary>
public class Game : MonoGameBaseGame
{
    private const int resolutionWidth = 1280;
    private const int resolutionHeight = 720;
    private readonly Color clearColor = Color.Black;

    private readonly GraphicsDeviceManager graphics;
    /// <summary>
    /// RNG used for generating seeds. Each RNG in the game is based on seed
    /// from this generator. <see cref="GenerateSeed"/> is used for obtaining
    /// seeds.
    /// </summary>
    private readonly Random seedGenerator;
    private IDictionary<Type, IResourceManager> resourceManagers;

    public ECSFactory Factory { get; private set; }
    public float Speed { get; set; } = 1.0f;
    public GameState ActiveState { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }
    /// <summary>
    /// Width and height of game window.
    /// </summary>
    public Vector2 Resolution 
        => new(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

    public Game(ECSFactory factory, int seed)
    {
        graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = resolutionWidth,
            PreferredBackBufferHeight = resolutionHeight,
            SynchronizeWithVerticalRetrace = false,
        };
        seedGenerator = new Random(seed);

        Factory = factory;

        IsMouseVisible = true;
        IsFixedTimeStep = false;
    }

    /// <summary>
    /// Generate seed for RNG.
    /// </summary>
    public int GenerateSeed()
        => seedGenerator.Next();

    public ResourceManager<TResource> GetResourceManager<TResource>()
        => (ResourceManager<TResource>)resourceManagers[typeof(TResource)];

    /// <summary>
    /// Create and switch to a new state of specific type. Created state will also be initialized.
    /// </summary>
    /// <typeparam name="TGameState">Type of state to create and to switch to.</typeparam>
    public void SwitchState<TGameState>()
        where TGameState : GameState, new()
    {
        var newState = new TGameState();
        newState.Initialize(this);
        SwitchState(newState);
    }

    /// <summary>
    /// Switch to an existing state. State will not be initialized.
    /// </summary>
    /// <param name="newState">New state to switch to.</param>
    public void SwitchState(GameState newState)
        => ActiveState = newState;

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        resourceManagers = new Dictionary<Type, IResourceManager>()
        {
            { typeof(Texture2D) ,new TextureManager(GraphicsDevice) },
        };
        foreach (var manager in resourceManagers.Values)
        {
            manager.LoadAll();
        }
        ActiveState.Initialize(this);

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        ActiveState.Update(deltaTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        GraphicsDevice.Clear(clearColor);
        ActiveState.Draw(deltaTime);

        base.Draw(gameTime);
    }

    /// <summary>
    /// Call <see cref="Update(GameTime)"/> method only once. Caller is responsible for
    /// managing gameTime.
    /// </summary>
    public void UpdateOnce(GameTime gameTime)
        => Update(gameTime);
}
