﻿using Microsoft.Xna.Framework;

using System;

namespace WorldSimulator.Extensions;
/// <summary>
/// Contains extension methods for <see cref="Random"/> class.
/// </summary>
internal static class RandomExtensions
{
    /// <summary>
    /// Generate random unit vector.
    /// </summary>
    public static Vector2 NextUnitVector2(this Random random)
    {
        float angle = random.NextSingle(2.0f * MathF.PI);
        return new(MathF.Cos(angle), MathF.Sin(angle));
    }

    /// <summary>
    /// Generate random number in range [inclusive, exclusive).
    /// </summary>
    public static float NextSingle(this Random random, float inclusive, float exclusive)
        => (exclusive - inclusive) * random.NextSingle() + inclusive;

    /// <summary>
    /// Generate random runber in range [0, exclusive).
    /// </summary>
    public static float NextSingle(this Random random, float exclusive)
        => random.NextSingle(0.0f, exclusive);
}