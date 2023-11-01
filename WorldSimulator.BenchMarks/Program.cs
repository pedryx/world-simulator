using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

using WorldSimulator.Benchmarks;

BenchmarkRunner.Run<ECSBenchmarks>(ManualConfig
    .Create(DefaultConfig.Instance)
    .WithOptions(ConfigOptions.DisableOptimizationsValidator));