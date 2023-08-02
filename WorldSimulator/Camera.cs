using Microsoft.Xna.Framework;

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
    public Vector2 Position;
    public float Scale = 1.0f;

    public Camera(Game game)
    {
        this.game = game;
    }

    /// <summary>
    /// Bounding rectangle of camera's viev.
    /// </summary>
    public Rectangle ViewBounds
    {
        get
        {
            Vector2 size = game.Resolution / Scale;
            return new((Position - size / 2.0f).ToPoint(), size.ToPoint());
        }
    }

    /// <summary>
    /// Get the transofmation matrix representing the camera's view.
    /// </summary>
    /// <returns>The 2D transformation matrix.</returns>
    public Matrix GetTransformMatrix()
        => Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
        * Matrix.CreateScale(Scale, Scale, 1.0f)
        * Matrix.CreateTranslation(game.Resolution.X / 2, game.Resolution.Y / 2, 0);
}
