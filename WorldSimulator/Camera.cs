using Microsoft.Xna.Framework;

namespace WorldSimulator;
/// <summary>
/// Simple 2D camera.
/// </summary>
internal class Camera
{
    /// <summary>
    /// Position of the center of the camera.
    /// </summary>
    public Vector2 Position;
    public float Scale = 1.0f;

    /// <summary>
    /// Bounding rectangle of camera's view.
    /// </summary>
    public Rectangle ViewBounds
    {
        get
        {
            Vector2 size = Game.DefaultResolution / Scale;
            return new((Position - size / 2.0f).ToPoint(), size.ToPoint());
        }
    }

    /// <summary>
    /// Get the transformation matrix representing the camera's view.
    /// </summary>
    /// <returns>The 2D transformation matrix.</returns>
    public Matrix GetTransformMatrix()
        => Matrix.CreateTranslation(-Position.X, -Position.Y, 0)
        * Matrix.CreateScale(Scale, Scale, 1.0f)
        * Matrix.CreateTranslation(Game.DefaultResolution.X / 2, Game.DefaultResolution.Y / 2, 0);
}
