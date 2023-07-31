using Microsoft.Xna.Framework;

using System;
using System.Linq;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
internal readonly struct AnimalControllerSystem : IEntityProcessor<Transform, PathFollow, AnimalController>
{
    private const float radius = 80.0f;

    private readonly Random random;
    private readonly GameWorld gameWorld;

    public AnimalControllerSystem(Game game, GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;

        random = new Random(game.GenerateSeed());
    }

    public void Process(ref Transform transform, ref PathFollow pathFollow, ref AnimalController _, float deltaTime)
    {
        if (pathFollow.Current == pathFollow.Path.Length)
        {
            // Generate random destinations around this position until one is valid. This usually succeed at first try.
            Vector2 destination;
            do
            {
                destination = random.NextUnitVector2() * radius + transform.Position;
            }
            while (!gameWorld.IsAnimalWalkable(destination));

            // Find path between current position and destination. Usually size of the path is two, but can be bigger.
            pathFollow.Current = 0;
            pathFollow.Path = gameWorld.FindPath(transform.Position, destination).ToArray();
        }
    }
}
