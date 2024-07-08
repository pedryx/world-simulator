using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds health mechanic to an entity.
/// </summary>
[Component]
internal struct Health
{
    /// <summary>
    /// The current health amount.
    /// </summary>
    public float Amount;
    /// <summary>
    /// The ID of the most recent entity which has damaged the current entity.
    /// </summary>
    public int DamageSourceID = -1;

    public Health() { }
}
