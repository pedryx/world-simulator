using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;
using WorldSimulator.Villages;

namespace WorldSimulator.Systems;
/// <summary>
/// Handles AI behavior of villagers.
/// </summary>
internal readonly struct VillagerBehaviorSystem : IEntityProcessor<VillagerBehavior, Owner>
{
    private readonly GameWorld gameWorld;

    public VillagerBehaviorSystem(GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;
    }

    public void Process(ref VillagerBehavior behavior, ref Owner owner, float deltaTime)
    {
        gameWorld.GetVillage(behavior.VillageID)
            .GetBehaviorTree(owner.Entity)
            .Tick(new VillagerContext(owner.Entity, gameWorld, deltaTime));
    }
}
