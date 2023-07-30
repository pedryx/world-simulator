using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Entities with this component will move randomly around the map.
/// </summary>
[Component]
internal struct RandomMovement
{
    /// <summary>
    /// Reaming time until current movement ends (in seconds).
    /// </summary>
    public float Reaming;
}
