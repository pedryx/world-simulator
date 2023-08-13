using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.ContentResources;

using MonoGameBaseGame = Microsoft.Xna.Framework.Game;

namespace WorldSimulator;
/// <summary>
/// Main game class.
/// </summary>
public class Game : MonoGameBaseGame
{
    private const int defaultResolutionWidth = 1920;
    private const int defaultResolutionHeight = 1080;
    private const int resolutionWidth = 1280;
    private const int resolutionHeight = 720;
    private readonly Color clearColor = Color.Black;

    /// <summary>
    /// RNG used for generating seeds. Each RNG in the game is based on seed
    /// from this generator. <see cref="GenerateSeed"/> is used for obtaining
    /// seeds.
    /// </summary>
    private readonly Random seedGenerator;
    private IDictionary<Type, IResourceManager> resourceManagers;

    internal ECSFactory Factory { get; private set; }
    public float Speed { get; set; } = 1.0f;
    internal GameState ActiveState { get; private set; }
    internal SpriteBatch SpriteBatch { get; private set; }
    internal GraphicsDeviceManager Graphics { get; private set; }
    internal Texture2D BlankTexture { get; private set; }

    /// <summary>
    /// Width and height of game window.
    /// </summary>
    internal Vector2 Resolution => new(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

    /// <summary>
    /// Default width and height of game window.
    /// </summary>
    internal static Vector2 DefaultResolution => new(defaultResolutionWidth, defaultResolutionHeight);

    internal Vector2 ResolutionScale => Resolution / DefaultResolution;

    public Game(ECSFactory factory, int seed)
    {
        Graphics = new GraphicsDeviceManager(this)
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
    internal int GenerateSeed()
        => seedGenerator.Next();

    internal ResourceManager<TResource> GetResourceManager<TResource>()
        => (ResourceManager<TResource>)resourceManagers[typeof(TResource)];

    /// <summary>
    /// Create and switch to a new state of specific type. Created state will also be initialized.
    /// </summary>
    /// <typeparam name="TGameState">Type of state to create and to switch to.</typeparam>
    internal void SwitchState<TGameState>()
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

        BlankTexture = new Texture2D(GraphicsDevice, 1, 1);
        BlankTexture.SetData(new Color[] { Color.White });

        resourceManagers = new Dictionary<Type, IResourceManager>()
        {
            { typeof(Texture2D) ,new TextureManager(GraphicsDevice) },
            { typeof(Effect), new ShaderManager(GraphicsDevice) },
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
