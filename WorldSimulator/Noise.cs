namespace WorldSimulator;
/// <summary>
/// Simplex noise generator with 3 octanes. You need to call <see cref="Begin"/> at least once before calling
/// <see cref="CalculateValue(int, int)"/>. When you call <see cref="Begin"/> on another noise instance the begin call
/// on all other instances is invalidated.
/// </summary>
internal class Noise
{
    /// <summary>
    /// Amplitude of first octane.
    /// </summary>
    private const float amplitude1 = 1.0f;
    /// <summary>
    /// Amplitude of second octane.
    /// </summary>
    private const float amplitude2 = 0.5f;
    /// <summary>
    /// Amplitude of third octane.
    /// </summary>
    private const float amplitude3 = 0.25f;

    public int Seed { get; private set; }
    /// <summary>
    /// Scale of first octane.
    /// </summary>
    public float Scale1 { get; private set; }
    /// <summary>
    /// Scale of second octane.
    /// </summary>
    public float Scale2 { get; private set; }
    /// <summary>
    /// Scake of third octane.
    /// </summary>
    public float Scale3 { get; private set; }

    /// <param name="scale1">Scale of first octane.</param>
    /// <param name="scale2">Scale of second octane.</param>
    /// <param name="scale3">Scake of third octane.</param>
    public Noise(int seed, float scale1, float scale2, float scale3)
    {
        Seed = seed;
        Scale1 = scale1;
        Scale2 = scale2;
        Scale3 = scale3;
    }

    /// <summary>
    /// Call this before starting go calculate values.
    /// </summary>
    public void Begin()
        => SimplexNoise.Noise.Seed = Seed;

    /// <summary>
    /// Calculate noise value at specific position.
    /// </summary>
    public float CalculateValue(int x, int y)
    {
        float value = amplitude1 * SimplexNoise.Noise.CalcPixel2D(x, y, Scale1)
                + amplitude2 * SimplexNoise.Noise.CalcPixel2D(x, y, Scale2)
                + amplitude3 * SimplexNoise.Noise.CalcPixel2D(x, y, Scale3);
        value /= (amplitude1 + amplitude2 + amplitude3) * 255.0f;

        return value;
    }
}
