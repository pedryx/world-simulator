using BenchmarkDotNet.Attributes;

using Microsoft.Xna.Framework;

using System;

using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.ECS.Arch;
using WorldSimulator.ECS.DefaultEcs;
using WorldSimulator.ECS.Entitas;
using WorldSimulator.ECS.HypEcs;
using WorldSimulator.ECS.LeoECS;
using WorldSimulator.ECS.LeoEcsLite;
using WorldSimulator.ECS.MonoGameExtendedEntities;
using WorldSimulator.ECS.RelEcs;
using WorldSimulator.ECS.SveltoECS;
using WorldSimulator.Level;

namespace WorldSimulator.Benchmarks;
/// <summary>
/// Contains  benchmark for <see cref="Game"/> with various ECS frameworks.
/// </summary>
public class ECSBenchmarks
{
    /// <summary>
    /// Game seed.
    /// </summary>
    private const int seed = 0;
    /// <summary>
    /// Delta time used for game simulation.
    /// </summary>
    private const double deltaTime = 1.0 / 60.0;
    /// <summary>
    /// Number of iterations for which will game run during iteration setup.
    /// </summary>
    private const int setupIterationCount = 30 * 60; // ~30 seconds
    /// <summary>
    /// Number of iterations for which will game run during benchmark.
    /// </summary>
    private const int benchmarkIterationCount = 1 * 60 * 60; // ~1 minute

    private Game game;
    private TimeSpan elapsedTimeSpan;
    private TimeSpan deltaTimeTimeSpan;

    [Params
    (
        typeof(ArchFactory),
        typeof(DefaultEcsFactory),
        typeof(EntitasFactory),
        typeof(HypEcsFactory),
        typeof(LeoECSFactory),
        typeof(LeoEcsLiteFactory),
        typeof(MonoGameExtendedFactory),
        typeof(RelEcsFactory),
        typeof(SveltoECSFactory)
    )]
    public Type ECSFactoryType { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        SeedGenerator.SetGlobalSeed(seed);
        ECSFactory ecsFactory = (ECSFactory)Activator.CreateInstance(ECSFactoryType);

        game = new Game(ecsFactory);
        game.SwitchState(new LevelState(true));
        game.RunOneFrame();

        elapsedTimeSpan = TimeSpan.FromSeconds(0.0);
        deltaTimeTimeSpan = TimeSpan.FromSeconds(deltaTime);

        RunFor(setupIterationCount);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        game = null;
        GC.Collect();
    }

    [Benchmark]
    public void Benchmark()
    {
        RunFor(benchmarkIterationCount);
    }

    /// <summary>
    /// Run game for specific amount of iterations. During these iterations only the
    /// <see cref="WorldSimulatorGame.Update(GameTime)"/> will be called.
    /// </summary>
    /// <param name="iterations">Number of iterations.</param>
    public void RunFor(int iterations)
    {
        for (int i = 0; i < iterations; i++)
        {
            game.UpdateOnce(new GameTime(elapsedTimeSpan, deltaTimeTimeSpan));
            elapsedTimeSpan += deltaTimeTimeSpan;
        }
    }
}
