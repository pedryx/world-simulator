using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator.Villages;
/// <summary>
/// Context used for passing arguments to behavior tree.
/// </summary>
internal class VillagerContext
{
    public IEntity Entity { get; private set; }
    public LegacyGameWorld GameWorld { get; private set; }
    public float DeltaTime { get; private set; }

    public VillagerContext(IEntity entity, LegacyGameWorld gameWorld, float deltaTime)
    {
        Entity = entity;
        GameWorld = gameWorld;
        DeltaTime = deltaTime;
    }
}