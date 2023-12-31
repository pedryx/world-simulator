﻿using System.Linq;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
/// <summary>
/// System which provides the user with debugging information.
/// </summary>
internal readonly struct DebugSystem : IECSSystem
{
    private const int sampleCount = 60;

    private readonly Game game;
    private readonly Camera camera;
    private readonly float[] samples = new float[sampleCount];
    private readonly RefWrapper<int> sampleIndex = new();

    public DebugSystem(Game game, Camera camera)
    {
        this.game = game;
        this.camera = camera;
    }

    public void Initialize(IECSWorld world) { }

    public void Update(float deltaTime)
    {
        samples[sampleIndex] = deltaTime;
        sampleIndex.Value++;
        if (sampleIndex == samples.Length)
        {
            sampleIndex.Value = 0;
        }

        float average = samples.Average();
        game.Window.Title = $"position: {camera.Position.ToPoint()}, FPS: {1 / average:#}," +
            $" deltaTime: {average:#.######}";
    }
}
