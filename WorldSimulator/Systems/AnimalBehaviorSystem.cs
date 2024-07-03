using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
/// <summary>
/// System responsible for controlling the logic behind the behavior of animals.
/// </summary>
internal readonly struct AnimalBehaviorSystem : IEntityProcessor<Location, Movement, AnimalBehavior, Owner>
{
    /// <summary>
    /// The maximum radius for random point generation. Used for simulating the random walk of an animal.
    /// </summary>
    private const float maxRadius = 80.0f;
    /// <summary>
    /// The minimum radius for random point generation. Used for simulating the random walk of an animal.
    /// </summary>
    private const float minRadius = 20.0f;
    /// <summary>
    /// The maximum time interval for a position of an animal to be updated in the corresponding KD-tree.
    /// </summary>
    private const float maxTimeToUpdate = 2.0f;
    /// <summary>
    /// The minimal time interval for a position of an animal to be updated in the corresponding KD-tree.
    /// </summary>
    private const float minTimeToUpdate = 1.0f;

    private readonly GameWorld gameWorld;

    public AnimalBehaviorSystem(Game game, GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;
    }

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process
    (
        ref Location location,
        ref Movement movement,
        ref AnimalBehavior behavior,
        ref Owner owner,
        float deltaTime
    )
    {
        if (behavior.UpdateEnabled)
        {
            behavior.TimeToUpdate -= deltaTime;
            if (behavior.TimeToUpdate <= 0.0f)
            {
                behavior.TimeToUpdate = behavior.Random.NextSingle(minTimeToUpdate, maxTimeToUpdate);
                if (behavior.PreviousPosition != location.Position)
                {
                    gameWorld.UpdateResourcePosition
                    (
                        behavior.ResourceType,
                        owner.Entity,
                        behavior.PreviousPosition,
                        location.Position
                    );
                    behavior.PreviousPosition = location.Position;
                }
            }
        }

        if (location.Position.IsCloseEnough(movement.Destination, movement.Speed * deltaTime))
        {
            float radiusOffset = 0.0f;
            Vector2 destination;
            TerrainType terrainType;
            do
            {
                destination = behavior.Random.NextPointInRing
                (
                    location.Position,
                    minRadius,
                    maxRadius + radiusOffset
                );
                radiusOffset += 1.0f;
                terrainType = gameWorld.GetTerrain(destination);
            }
            // Deer can live only in plain biome.
            while (!(terrainType != null && terrainType.CanWalk && terrainType == TerrainType.Plain));

            movement.Destination = destination;
        }
    }
}
