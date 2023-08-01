using Microsoft.Xna.Framework;

using System;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
internal readonly struct AnimalControllerSystem : IEntityProcessor<Transform, Movement, AnimalController>
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

    public void Process(ref Transform transform, ref Movement movement, ref AnimalController _, float deltaTime)
    {
        if (Vector2.Distance(transform.Position, movement.Destination) <= movement.Speed * deltaTime)
        {
            // 
            /*
             * Generate random destinations around this position until one is valid.
             * This usually succeed at first try.
             * On failure, increase search radius.
             */
            float radiusOffset = 0.0f;
            Vector2 destination;
            do
            {
                destination = random.NextPointInRing(transform.Position, minRadius, maxRadius + radiusOffset);
                radiusOffset += 1.0f;
            }
            while (!gameWorld.IsAnimalWalkable(destination));

            movement.Destination = destination;
        }
    }
}
