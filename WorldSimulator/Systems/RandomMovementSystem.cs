using System;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Systems;
internal readonly struct RandomMovementSystem : IEntityProcessor<Movement, RandomMovement>
{
    private const float minDuration = 3.0f;
    private const float maxDuration = 8.0f;

    private readonly Random random;

    public RandomMovementSystem(Game game)
    {
        random = new Random(game.GenerateSeed());
    }

    public void Process(ref Movement movement, ref RandomMovement randomMovement, float deltaTime)
    {
        /* 
         * Entity will move in random direction until reaming reaches 0, after that it will me asigned new random
         * direction and new reaming.
         */

        randomMovement.Reaming -= deltaTime;

        if (randomMovement.Reaming <= 0.0f)
        {
            randomMovement.Reaming = random.NextSingle(minDuration, maxDuration);
            movement.Direction = random.NextUnitVector2();
        }
    }
}
