using Microsoft.Xna.Framework;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator.Systems.Villaages;
/// <summary>
/// System responsible for spawning villagers.
/// </summary>
internal readonly struct VillagerSpawningSystem : IEntityProcessor<Location, VillagerSpawner>
{
    /// <summary>
    /// How long it takes for a villager to spawn in seconds.
    /// </summary>
    private const float villagerSpawnTime = 60.0f;

    private readonly LevelFactory levelFactory;

    public VillagerSpawningSystem(LevelFactory levelFactory)
    {
        this.levelFactory = levelFactory;
    }

    public void Process(ref Location location, ref VillagerSpawner villagerSpawner, float deltaTime)
    {
        if (villagerSpawner.Villager == null || villagerSpawner.Villager.IsDestroyed())
        {
            villagerSpawner.Elapsed += deltaTime;

            if (villagerSpawner.Elapsed >= villagerSpawnTime)
            {
                villagerSpawner.Elapsed = 0.0f;

                IEntity villager = levelFactory.CreateVillager
                (
                    location.Position + Vector2.UnitY,
                    villagerSpawner.Village
                );
                villagerSpawner.Villager = villager;

                ref Village village = ref villagerSpawner.Village.GetComponent<Village>();
                village.UnemployedVillagers[village.UnemployedVillagerCount] = villager;
                village.UnemployedVillagerCount++;
            }
        }
    }
}
