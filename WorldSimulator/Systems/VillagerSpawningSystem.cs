using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;

namespace WorldSimulator.Systems;
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
    private readonly GameWorld gameWorld;

    public VillagerSpawningSystem(LevelFactory levelFactory, GameWorld gameWorld)
    {
        this.levelFactory = levelFactory;
        this.gameWorld = gameWorld;
    }

    public void Process(ref Location location, ref VillagerSpawner villagerSpawner, float deltaTime)
    {
        if (villagerSpawner.Villager == null || villagerSpawner.Villager.IsDestroyed())
        {
            villagerSpawner.Elapsed += deltaTime;

            if (villagerSpawner.Elapsed >= villagerSpawnTime)
            {
                villagerSpawner.Elapsed = 0.0f;

                IEntity villager = levelFactory.CreateVillager(location.Position + Vector2.UnitY);
                gameWorld.GetVillage(villagerSpawner.VillageID).AddVillager(villager);
                villagerSpawner.Villager = villager;
            }
        }
    }
}
