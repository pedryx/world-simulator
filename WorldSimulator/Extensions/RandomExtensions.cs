using Microsoft.Xna.Framework;

using System;

namespace WorldSimulator.Extensions;
/// <summary>
/// Contains extension methods for the <see cref="Random"/> class.
/// </summary>
internal static class RandomExtensions
{
    /// <summary>
    /// Generate a random unit vector.
    /// </summary>
    public static Vector2 NextUnitVector(this Random random)
    {
        float angle = random.NextSingle(2.0f * MathF.PI);
        return new(MathF.Cos(angle), MathF.Sin(angle));
    }

    /// <summary>
    /// Generate a random point in a circle.
    /// </summary>
    /// <param name="center">The center of the circle.</param>
    /// <param name="outerRadius">The radius of the circle.</param>
    public static Vector2 NextPointInRing(this Random random, Vector2 center, float innerRadius, float outerRadius) 
        => random.NextUnitVector() * random.NextSingle(innerRadius, outerRadius) + center;

    /// <summary>
    /// Generate a random number in a range [inclusive, exclusive).
    /// </summary>
    public static float NextSingle(this Random random, float inclusive, float exclusive)
        => (exclusive - inclusive) * random.NextSingle() + inclusive;

    /// <summary>
    /// Generate a random number in a range [0, exclusive).
    /// </summary>
    public static float NextSingle(this Random random, float exclusive)
        => random.NextSingle(0.0f, exclusive);
}
