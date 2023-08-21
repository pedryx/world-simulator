using Microsoft.Xna.Framework;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
/// <summary>
/// Adds animal behavior to an entity.
/// </summary>
[Component]
internal struct AnimalBehavior 
{
    /// <summary>
    /// The time reaming until the animal's position within its corresponding KD-tree got updated.
    /// </summary>
    public float TimeToUpdate;

    /// <summary>
    /// The resource type of the animal.
    /// </summary>
    public ResourceType ResourceType;

    /// <summary>
    /// The position of the entity during the last KD-tree position update.
    /// </summary>
    public Vector2 PreviousPosition;

    /// <summary>
    /// Determine if the animal position will be updated in its corresponding KD-tree.
    /// </summary>
    public bool UpdateEnabled = true;

    public AnimalBehavior() { }
}
