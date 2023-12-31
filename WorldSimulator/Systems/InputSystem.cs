﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
/// <summary>
/// System responsible for handling input from mouse and keyboard.
/// </summary>
internal readonly struct InputSystem : IECSSystem
{
    private const float cameraMoveSpeed = 8.0f;
    /// <summary>
    /// The speed of the camera zooming in/out.
    /// </summary>
    private const float cameraZoomAmount = 0.2f;
    /// <summary>
    /// The minimum camera zoomed value.
    /// </summary>
    private const float cameraMinZoom = 0.08f;
    /// <summary>
    /// The maximum camera zoomed value.
    /// </summary>
    private const float cameraMaxZoom = 4.0f;

    private readonly Point fullscreenResolution = new(1920, 1080);
    private readonly Point windowedResolution = new(1280, 720);

    private readonly Game game;
    private readonly Camera camera;
    private readonly GameState gameState;
    private readonly InputState state = new();

    public InputSystem(LevelState levelState)
    {
        game = levelState.Game;
        camera = levelState.Camera;
        gameState = levelState;
    }

    public void Initialize(IECSWorld world) { }

    public void Update(float deltaTime)
    {
        state.Update();

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
        if (!gameState.UILayer.MouseHover)
        {
            if (state.CurrentMouse.ScrollWheelValue > state.LastMouse.ScrollWheelValue)
                zoomDirection = 1.0f;
            else if (state.CurrentMouse.ScrollWheelValue < state.LastMouse.ScrollWheelValue)
                zoomDirection = -1.0f;
        }

        camera.Position += movementDirection * cameraMoveSpeed * (1 / camera.Scale);
        camera.Scale *= 1.0f + zoomDirection * cameraZoomAmount;

        camera.Position.X = MathHelper.Clamp(camera.Position.X, GameWorld.Bounds.Left, GameWorld.Bounds.Right);
        camera.Position.Y = MathHelper.Clamp(camera.Position.Y, GameWorld.Bounds.Top, GameWorld.Bounds.Bottom);
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
        => state.CurrentKeyboard.IsKeyDown(key);

    private bool IsPressed(Keys key)
        => state.CurrentKeyboard.IsKeyDown(key) && state.LastKeyboard.IsKeyUp(key);

    private class InputState
    {
        public KeyboardState LastKeyboard;
        public KeyboardState CurrentKeyboard = Keyboard.GetState();
        public MouseState LastMouse;
        public MouseState CurrentMouse = Mouse.GetState();

        public void Update()
        {
            LastKeyboard = CurrentKeyboard;
            CurrentKeyboard = Keyboard.GetState();
            LastMouse = CurrentMouse;
            CurrentMouse = Mouse.GetState();
        }
    }
}
