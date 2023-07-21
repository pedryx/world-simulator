﻿using Microsoft.Xna.Framework;

using WorldSimulator.Components;

namespace WorldSimulator;
/// <summary>
/// Simple 2D camera.
/// </summary>
public class Camera
{
    private readonly Game game;

    /// <summary>
    /// Position of the center of the camera.
    /// </summary>
    public Vector2 Position { get; private set; }
    public float Scale { get; private set; } = 1.0f;
    public float Rotation { get; private set; }

    public Camera(Game game)
    {
        this.game = game;
    }

    public Matrix GetTransformMatrix()
        => Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
        * Matrix.CreateScale(Scale, Scale, 1.0f)
        * Matrix.CreateRotationZ(Rotation)
        * Matrix.CreateTranslation(game.Resolution.X / 2, game.Resolution.Y / 2, 0);
}
