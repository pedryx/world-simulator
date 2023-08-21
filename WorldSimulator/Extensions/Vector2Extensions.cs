using Microsoft.Xna.Framework;

namespace WorldSimulator.Extensions;
/// <summary>
/// Contains extension methods for the <see cref="Vector2"/>.
/// </summary>
internal static class Vector2Extensions
{
    /// <summary>
    /// Convert a <see cref="Vector2"/> to a float array of size 2, where the first item corresponds to the
    /// <see cref="Vector2.X"/> and the second item corresponds to the <see cref="Vector2.Y"/>.
    /// </summary>
    public static float[] ToFloat(this Vector2 vector)
        => new float[] { vector.X, vector.Y };

    /// <summary>
    /// Determine if a position is close enough to a destination (their distance is smaller than a threshold).
    /// </summary>
    public static bool IsCloseEnough(this Vector2 position, Vector2 destination, float threshold)
        => Vector2.DistanceSquared(position, destination) < threshold * threshold;
}
