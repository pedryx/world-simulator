using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
/// <summary>
/// handles input from mouse and keyboard.
/// </summary>
internal struct InputSystem : IECSSystem
{
    private const float cameraMoveSpeed = 2250.0f;
    /// <summary>
    /// Speed of camera zooming in/out.
    /// </summary>
    private const float cameraZoomSpeed = 0.2f;
    /// <summary>
    /// Minimum camera zoomed value.
    /// </summary>
    private const float cameraMinZoom = 0.2f;
    /// <summary>
    /// Maximum camera zoomed value.
    /// </summary>
    private const float cameraMaxZoom = 4.0f;

    private readonly Point fullscreenResolution = new(1920, 1080);
    private readonly Point windowedResolution = new(1280, 720);

    private readonly Game game;
    private readonly Camera camera;
    private readonly GameWorld gameWorld;

    private KeyboardState lastKeyboardState;
    private KeyboardState currentKeyboardState = Keyboard.GetState();
    private MouseState lastMouseState;
    private MouseState currentMouseState = Mouse.GetState();

    public InputSystem(Game game, Camera camera, GameWorld gameWorld)
    {
        this.game = game;
        this.camera = camera;
        this.gameWorld = gameWorld;
    }

    public void Initialize(IECSWorld world) { }

    public void Update(float deltaTime)
    {
        lastKeyboardState = currentKeyboardState;
        currentKeyboardState = Keyboard.GetState();
        lastMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();

        HandleSystemControl();
        HandleCameraControl(deltaTime);
    }

    private void HandleSystemControl()
    {
        if (IsPressed(Keys.Escape))
            game.Exit();
        if (IsPressed(Keys.F11))
            ToggleFullscreen();
    }

    private void HandleCameraControl(float deltaTime)
    {
        // get movement direction
        Vector2 movementDirection = new();
        if (IsDown(Keys.W))
            movementDirection += -Vector2.UnitY;
        if (IsDown(Keys.A))
            movementDirection += -Vector2.UnitX;
        if (IsDown(Keys.S))
            movementDirection += Vector2.UnitY;
        if (IsDown(Keys.D))
            movementDirection += Vector2.UnitX;

        if (movementDirection != Vector2.Zero)
            movementDirection.Normalize();

        // get zoom direction
        float zoomDirection = 0.0f;
        if (currentMouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue)
            zoomDirection = 1.0f;
        else if (currentMouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)
            zoomDirection = -1.0f;

        // compute changes
        camera.Position += movementDirection * cameraMoveSpeed * deltaTime * (1 / camera.Scale);
        camera.Scale *= (1.0f + zoomDirection * cameraZoomSpeed);

        // clamp values
        camera.Position.X = MathHelper.Clamp(camera.Position.X, gameWorld.Bounds.Left, gameWorld.Bounds.Right);
        camera.Position.Y = MathHelper.Clamp(camera.Position.Y, gameWorld.Bounds.Top, gameWorld.Bounds.Bottom);
        camera.Scale = MathHelper.Clamp(camera.Scale, cameraMinZoom, cameraMaxZoom);
    }

    private void ToggleFullscreen()
    {
        if (game.Graphics.IsFullScreen)
        {
            game.Graphics.PreferredBackBufferWidth = windowedResolution.X;
            game.Graphics.PreferredBackBufferHeight = windowedResolution.Y;
            game.Graphics.IsFullScreen = false;
        }
        else
        {
            game.Graphics.PreferredBackBufferWidth = fullscreenResolution.X;
            game.Graphics.PreferredBackBufferHeight = fullscreenResolution.Y;
            game.Graphics.IsFullScreen = true;
        }
        game.Graphics.ApplyChanges();
    }

    private bool IsDown(Keys key)
        => currentKeyboardState.IsKeyDown(key);

    private bool IsPressed(Keys key)
        => currentKeyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
}
