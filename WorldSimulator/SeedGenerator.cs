using System;

namespace WorldSimulator
{
    /// <summary>
    /// Generator of seeds for random number generators.
    /// </summary>
    public static class SeedGenerator
    {
        /// <summary>
        /// Global random number generator used for generating random seeds.
        /// </summary>
        private static Random seedGenerator = new();

        /// <summary>
        /// Set global seed for seed generator. This will reset psedo generator of seeds.
        /// </summary>
        public static void SetGlobalSeed(int seed)
            => seedGenerator = new Random(seed);

        /// <summary>
        /// Generate new random seed.
        /// </summary>
        public static int Generate()
            => seedGenerator.Next();

        /// <summary>
        /// Create new instance of <see cref="Random"/> using new random seed.
        /// </summary>
        public static Random CreateRandom()
            => new(Generate());
    }
}
