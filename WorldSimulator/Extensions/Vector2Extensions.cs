using Microsoft.Xna.Framework;

namespace WorldSimulator.Extensions;
/// <summary>
/// Contains extension methods for <see cref="Vector2"/>.
/// </summary>
internal static class Vector2Extensions
{
    /// <summary>
    /// Convert <see cref="Vector2"/> to float array of size 2, where first item corresponds to <see cref="Vector2.X"/>
    /// and second item corresponds to <see cref="Vector2.Y"/>.
    /// </summary>
    public static float[] ToFloat(this Vector2 vector)
        => new float[] { vector.X, vector.Y };

    /// <summary>
    /// Determine if position is close enough to destination (their distance is smaller than threshold).
    /// </summary>
    public static bool IsCloseEnough(this Vector2 position, Vector2 destination, float threshold)
        => Vector2.DistanceSquared(position, destination) < threshold * threshold;
}
