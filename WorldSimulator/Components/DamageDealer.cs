using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds damage dealing capability to an entity.
/// </summary>
[Component]
internal struct DamageDealer
{
    public float DamagePerSecond;
    public IEntity Target;
}
