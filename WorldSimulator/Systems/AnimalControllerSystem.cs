using Microsoft.Xna.Framework;

using System;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Systems;
internal readonly struct AnimalControllerSystem : IEntityProcessor<Transform, Movement, AnimalController>
{
    private readonly float radius = 80.0f;

    private readonly Random random;

    public AnimalControllerSystem(Game game)
    {
        random = new Random(game.GenerateSeed());
    }

    public void Process(ref Transform transform, ref Movement movement, ref AnimalController _, float deltaTime)
    {
        if (Vector2.Distance(transform.Position, movement.Destination) <= movement.Speed * deltaTime)
        {
            transform.Position = movement.Destination;
            movement.Destination = random.NextUnitVector2() * radius + transform.Position;
        }
    }
}
