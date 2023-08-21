using Microsoft.Xna.Framework;

using System;

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

    /// <summary>
    /// Random number generator used for generating destinations for random walks.
    /// </summary>
    private readonly Random destinationRandom;
    /// <summary>
    /// Random number generator used for generating intervals for updating animal position in corresponding KD-tree.
    /// </summary>
    private readonly Random timeToUpdateRandom = new();
    private readonly GameWorld gameWorld;

    public AnimalBehaviorSystem(Game game, GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;

        destinationRandom = new Random(game.GenerateSeed());
    }

    public void Process
    (
        ref Location location,
        ref Movement movement,
        ref AnimalBehavior controller,
        ref Owner owner,
        float deltaTime
    )
    {
        if (controller.UpdateEnabled)
        {
            controller.TimeToUpdate -= deltaTime;
            if (controller.TimeToUpdate <= 0.0f)
            {
                controller.TimeToUpdate = timeToUpdateRandom.NextSingle(minTimeToUpdate, maxTimeToUpdate);
                if (controller.PreviousPosition != location.Position)
                {
                    gameWorld.UpdateResourcePosition
                    (
                        controller.ResourceType,
                        owner.Entity,
                        controller.PreviousPosition,
                        location.Position
                    );
                    controller.PreviousPosition = location.Position;
                }
            }
        }

        if (location.Position.IsCloseEnough(movement.Destination, movement.Speed * deltaTime))
        {
            float radiusOffset = 0.0f;
            Vector2 destination;
            Terrain terrain;
            do
            {
                destination = destinationRandom.NextPointInRing(location.Position, minRadius, maxRadius + radiusOffset);
                radiusOffset += 1.0f;
                terrain = gameWorld.GetTerrain(destination);
            }
            while (terrain != null && terrain.Walkable);

            movement.Destination = destination;
        }
    }
}
