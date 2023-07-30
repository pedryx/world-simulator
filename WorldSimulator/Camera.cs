using Microsoft.Xna.Framework;

using WorldSimulator.Components;

namespace WorldSimulator;
/// <summary>
/// Simple 2D camera.
/// </summary>
internal class Camera
{
    private readonly Game game;

    /// <summary>
    /// Position of the center of the camera.
    /// </summary>
    public Vector2 Position { get; set; }
    public float Scale { get; set; } = 1.0f;
    public float Rotation { get; set; }

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
