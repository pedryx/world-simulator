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
internal readonly struct AnimalControllerSystem : IEntityProcessor<Position, Movement, AnimalController>
{
    private const float maxRadius = 80.0f;
    private const float minRadius = 20.0f;

    private readonly Random random;
    private readonly GameWorld gameWorld;

    public AnimalControllerSystem(Game game, GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;

        random = new Random(game.GenerateSeed());
    }

    public void Process(ref Position position, ref Movement movement, ref AnimalController _, float deltaTime)
    {
        // Check if the animal has reached its destination.
        if (Vector2.Distance(position.Coordinates, movement.Destination) <= movement.Speed * deltaTime)
        {
            /*
             * Keep generating random destination around entity's position until valid location is found. The search
             * radius will keep increasing until valid location is found.
             */
            float radiusOffset = 0.0f;
            Vector2 destination;
            do
            {
                destination = random.NextPointInRing(position.Coordinates, minRadius, maxRadius + radiusOffset);
                radiusOffset += 1.0f;
            }
            while (!gameWorld.IsAnimalWalkable(destination));

            movement.Destination = destination;
        }
    }
}
