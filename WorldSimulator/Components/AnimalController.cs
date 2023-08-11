using Microsoft.Xna.Framework;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Entities with this component will move randomly around the map.
/// </summary>
[Component]
internal struct AnimalController 
{
    /// <summary>
    /// Time until animal position inside corresponding kd-tree got updated.
    /// </summary>
    public float TimeToUpdate;

    /// <summary>
    /// Resource type of animal.
    /// </summary>
    public ResourceType ResourceType;

    /// <summary>
    /// Position of entity during last kd-tree position update.
    /// </summary>
    public Vector2 PreviousPosition;

    /// <summary>
    /// Determine if animal position will be updated in its corresponding kd-tree.
    /// </summary>
    public bool UpdateEnabled = true;

    public AnimalController() { }
}
