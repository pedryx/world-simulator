using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
/// <summary>
/// handles input from mouse and keyboard.
/// </summary>
internal struct InputSystem : IECSSystem
{
    private const float cameraMoveSpeed = 1000.0f;
    /// <summary>
    /// Speed of camera zooming in/out.
    /// </summary>
    private const float cameraZoomSpeed = 50.0f;
    /// <summary>
    /// Minimum camera zoomed value.
    /// </summary>
    private const float cameraMinZoom = 0.1f;
    /// <summary>
    /// Maximum camera zoomed value.
    /// </summary>
    private const float cameraMaxZoom = 5.0f;

    private readonly Point fullscreenResolution = new(1920, 1080);
    private readonly Point windowedResolution = new(1280, 720);

    private readonly Game game;
    private readonly Camera camera;

    private KeyboardState lastKeyboardState;
    private KeyboardState currentKeyboardState = Keyboard.GetState();
    private MouseState lastMouseState;
    private MouseState currentMouseState = Mouse.GetState();

    public InputSystem(Game game, Camera camera)
    {
        this.game = game;
        this.camera = camera;
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

        float zoomDirection = 0.0f;
        if (currentMouseState.ScrollWheelValue > lastMouseState.ScrollWheelValue)
            zoomDirection = 1.0f;
        else if (currentMouseState.ScrollWheelValue < lastMouseState.ScrollWheelValue)
            zoomDirection = -1.0f;

        camera.Position += movementDirection * cameraMoveSpeed * deltaTime * (1 / camera.Scale);
        camera.Scale *= (1.0f + zoomDirection * cameraZoomSpeed * deltaTime);

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
