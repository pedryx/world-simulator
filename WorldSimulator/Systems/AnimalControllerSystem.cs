using Microsoft.Xna.Framework;

using System;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
/// <summary>
/// System responsible for controlling movement of animals.
/// </summary>
internal readonly struct AnimalControllerSystem : IEntityProcessor<Position, Movement, AnimalController, Owner>
{
    private const float maxRadius = 80.0f;
    private const float minRadius = 20.0f;
    private const float maxTimeToUpdate = 1.0f;
    private const float minTimeToUpdate = 0.5f;

    private readonly Random destinationRandom;
    private readonly Random timeToUpdateRandom = new();
    private readonly LegacyGameWorld gameWorld;

    public AnimalControllerSystem(Game game, LegacyGameWorld gameWorld)
    {
        this.gameWorld = gameWorld;

        destinationRandom = new Random(game.GenerateSeed());
    }

    public void Process
    (
        ref Position position,
        ref Movement movement,
        ref AnimalController controller,
        ref Owner owner,
        float deltaTime
    )
    {
        if (controller.UpdateEnabled)
        {
            controller.TimeToUpdate -= deltaTime;
            // check if entity position in corresponding kd-tree should be updated
            if (controller.TimeToUpdate <= 0.0f)
            {
                controller.TimeToUpdate = timeToUpdateRandom.NextSingle(minTimeToUpdate, maxTimeToUpdate);
                if (controller.PreviousPosition != position.Coordinates)
                {
                    gameWorld.UpdateResourcePosition
                    (
                        controller.ResourceType,
                        owner.Entity,
                        controller.PreviousPosition,
                        position.Coordinates
                    );
                    controller.PreviousPosition = position.Coordinates;
                }
            }
        }

        // Check if the animal has reached its destination.
        if (position.Coordinates.IsCloseEnough(movement.Destination, movement.Speed * deltaTime))
        {
            /*
             * Keep generating random destination around entity's position until valid location is found. The search
             * radius will keep increasing until valid location is found.
             */
            float radiusOffset = 0.0f;
            Vector2 destination;
            do
            {
                destination = destinationRandom.NextPointInRing(position.Coordinates, minRadius, maxRadius + radiusOffset);
                radiusOffset += 1.0f;
            }
            while (!gameWorld.IsWalkableForAnimals(destination));

            movement.Destination = destination;
        }
    }
}
