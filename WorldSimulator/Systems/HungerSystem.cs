using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
internal readonly struct HungerSystem : IEntityProcessor<Health, Hunger>
{
    /// <summary>
    /// Increase rate of hunger (per second).
    /// </summary>
    private const float hungerRate = 1.0f;
    /// <summary>
    /// Health decrease rate when villager is starving (per second).
    /// </summary>
    private const float starvationHealthDecreaseRate = 1.0f;
    /// <summary>
    /// Minimal amount of hunger so villager is considered starving.
    /// </summary>
    private const float starvationThreshold = 120.0f;

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Health health, ref Hunger hunger, float deltaTime)
    {
        hunger.Amount += hungerRate * deltaTime;
        
        if (hunger.Amount >= starvationThreshold)
        {
            health.Amount -= starvationHealthDecreaseRate * deltaTime;
        }
    }
}
