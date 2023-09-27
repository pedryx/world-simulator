using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator;
internal class BehaviorContext
{
    public required IEntity Entity { get; init; }
    public required GameWorld GameWorld { get; init; }
    public required float DeltaTime { get; init; }
}
