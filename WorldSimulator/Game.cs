using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.AssetManagers;

using MonoGameBaseGame = Microsoft.Xna.Framework.Game;

namespace WorldSimulator;
/// <summary>
/// Main game class.
/// </summary>
public class Game : MonoGameBaseGame
{
    public const MethodImplOptions EntityProcessorInline = 0;

    private const int defaultResolutionWidth = 1920;
    private const int defaultResolutionHeight = 1080;
    private const int initialResolutionWidth = 1280;
    private const int initialResolutionHeight = 720;

    private readonly Color clearColor = Color.Black;

    /// <summary>
    /// Random number generator used for generating seeds. Each random number generator in the game is based on a seed
    /// from this generator. Use <see cref="GenerateSeed"/> is used for obtaining seeds.
    /// </summary>
    private readonly Random seedGenerator;
    private IDictionary<Type, IAssetManager> resourceManagers;

    internal ECSFactory Factory { get; private set; }
    public float Speed { get; set; } = 1.0f;
    internal GameState ActiveState { get; private set; }
    internal SpriteBatch SpriteBatch { get; private set; }
    internal GraphicsDeviceManager Graphics { get; private set; }
    internal Texture2D BlankTexture { get; private set; }

    /// <summary>
    /// The actual width and height of the game window in pixels.
    /// </summary>
    internal Vector2 Resolution => new(Graphics.PreferredBackBufferWidth, Graphics.PreferredBackBufferHeight);

    /// <summary>
    /// The default width and height of the game window. This resolution remains the same as all other resolutions if the
    /// game uses any other resolution everything gets rescaled by resolution scale.
    /// </summary>
    internal static Vector2 DefaultResolution => new(defaultResolutionWidth, defaultResolutionHeight);

    /// <summary>
    /// Scaling factor for the actual resolution.
    /// </summary>
    internal Vector2 ResolutionScale => Resolution / DefaultResolution;

    public Game(ECSFactory factory, int seed)
    {
        Graphics = new GraphicsDeviceManager(this)
        {
            PreferredBackBufferWidth = initialResolutionWidth,
            PreferredBackBufferHeight = initialResolutionHeight,
            SynchronizeWithVerticalRetrace = false,
        };
        seedGenerator = new Random(seed);

        Factory = factory;

        IsMouseVisible = true;
        IsFixedTimeStep = false;
    }

    /// <summary>
    /// Generate seed for random number generation.
    /// </summary>
    internal int GenerateSeed()
        => seedGenerator.Next();

    internal AssetManager<TResource> GetResourceManager<TResource>()
        => (AssetManager<TResource>)resourceManagers[typeof(TResource)];

    /// <summary>
    /// Create and switch to a new state of a specified type. The created state will also be initialized.
    /// </summary>
    /// <typeparam name="TGameState">The type of state to create and to switch to.</typeparam>
    internal void SwitchState<TGameState>()
        where TGameState : GameState, new()
    {
        var newState = new TGameState();
        newState.Initialize(this);
        SwitchState(newState);
    }

    /// <summary>
    /// Switch to an existing state. The state will not be initialized.
    /// </summary>
    /// <param name="newState">The new state to switch to.</param>
    public void SwitchState(GameState newState)
        => ActiveState = newState;

    protected override void LoadContent()
    {
        if (ActiveState == null)
            throw new InvalidOperationException("Active state is is not set.");

        SpriteBatch = new SpriteBatch(GraphicsDevice);

        BlankTexture = new Texture2D(GraphicsDevice, 1, 1);
        BlankTexture.SetData(new Color[] { Color.White });

        resourceManagers = new Dictionary<Type, IAssetManager>()
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
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;

        ActiveState.Update(deltaTime);

        base.Update(gameTime);
    }


    protected override void Draw(GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;

        GraphicsDevice.Clear(clearColor);
        ActiveState.Draw(deltaTime);

        base.Draw(gameTime);
    }

    /// <summary>
    /// Call <see cref="Update(GameTime)"/> method only once. A caller is responsible for
    /// managing gameTime.
    /// </summary>
    public void UpdateOnce(GameTime gameTime)
        => Update(gameTime);
}
