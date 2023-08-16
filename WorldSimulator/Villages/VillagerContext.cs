using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator.Villages;
/// <summary>
/// Context used for passing arguments to behavior tree.
/// </summary>
internal class VillagerContext
{
    public IEntity Entity { get; private set; }
    public GameWorld GameWorld { get; private set; }
    public float DeltaTime { get; private set; }

    public VillagerContext(IEntity entity, GameWorld gameWorld, float deltaTime)
    {
        Entity = entity;
        GameWorld = gameWorld;
        DeltaTime = deltaTime;
    }
}