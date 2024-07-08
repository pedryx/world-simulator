using Microsoft.Xna.Framework;

using System;
using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Level;
using WorldSimulator.ManagedDataManagers;

namespace WorldSimulator.Systems;
/// <summary>
/// Handles deaths of entities.
/// </summary>
internal readonly struct DeathSystem : IEntityProcessor<Health, Owner>
{
    private readonly GameWorld gameWorld;
    private readonly ManagedDataManager<IEntity> entityManager;
    private readonly ManagedDataManager<Vector2[]> pathManager;
    private readonly ManagedDataManager<ItemCollection?> itemCollectionManager;
    private readonly ManagedDataManager<Random> randomManager;

    public DeathSystem(Game game, GameWorld gameWorld)
    {
        this.gameWorld = gameWorld;

        entityManager = game.GetManagedDataManager<IEntity>();
        pathManager = game.GetManagedDataManager<Vector2[]>();
        itemCollectionManager = game.GetManagedDataManager<ItemCollection?>();
        randomManager = game.GetManagedDataManager<Random>();
    }

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Health health, ref Owner owner, float deltaTime)
    {
        IEntity entity = entityManager[owner.EntityID];

        if (entity == null)
            return;

        if (health.Amount < 0.0f)
        {
            if (health.DamageSourceID != -1 && !entityManager[health.DamageSourceID].IsDestroyed() &&
                entityManager[health.DamageSourceID].HasComponent<Inventory>())
            {
                IEntity damageSourceEntity = entityManager[health.DamageSourceID];
                ref Inventory attackerInventory = ref damageSourceEntity.GetComponent<Inventory>();
                ItemCollection attackerItems = itemCollectionManager[attackerInventory.ItemCollectionID].Value;

                if (entity.HasComponent<ItemDrop>())
                {
                    ref ItemDrop itemDrop = ref entity.GetComponent<ItemDrop>();
                    ItemCollection droppedItems = itemCollectionManager[itemDrop.ItemCollectionID].Value;

                    droppedItems.TransferTo(ref attackerItems);
                }

                if (entity.HasComponent<Inventory>())
                {
                    ref Inventory inventory = ref entity.GetComponent<Inventory>();
                    ItemCollection inventoryItems = itemCollectionManager[inventory.ItemCollectionID].Value;

                    inventoryItems.TransferTo(ref attackerItems);
                }

                itemCollectionManager[attackerInventory.ItemCollectionID] = attackerItems;
            }

            if (entity.HasComponent<VillagerBehavior>())
            {
                ref VillagerBehavior behavior = ref entity.GetComponent<VillagerBehavior>();

                IEntity target = entityManager[behavior.TargetID];

                if (target != null && !target.IsDestroyed() && target.HasComponent<Resource>())
                {
                    ref Resource resource = ref target.GetComponent<Resource>();

                    gameWorld.AddResource
                    (
                        ResourceType.Get(resource.TypeID),
                        target,
                        target.GetComponent<Location>().Position
                    );
                }

                entityManager.Free(behavior.TargetID);
                behavior.TargetID = -1;
            }

            if (entity.HasComponent<PathFollow>())
            {
                ref PathFollow pathFollow = ref entity.GetComponent<PathFollow>();
                pathManager.Free(pathFollow.PathID);
                pathFollow.PathID = -1;
            }

            if (entity.HasComponent<Inventory>())
            {
                ref Inventory inventory = ref entity.GetComponent<Inventory>();
                itemCollectionManager.Free(inventory.ItemCollectionID);
                inventory.ItemCollectionID = -1;
            }

            entityManager.Free(health.DamageSourceID);
            health.DamageSourceID = -1;

            if (entity.HasComponent<DamageDealer>())
            {
                ref DamageDealer damageDealer = ref entity.GetComponent<DamageDealer>();
                entityManager.Free(damageDealer.TargetID);
                damageDealer.TargetID = -1;
            }

            if (entity.HasComponent<AnimalBehavior>())
            {
                ref AnimalBehavior animalBehavior = ref entity.GetComponent<AnimalBehavior>();
                randomManager.Free(animalBehavior.RandomID);
                animalBehavior.RandomID = -1;
            }

            if (entity.HasComponent<Villager>())
            {
                ref Villager villager = ref entity.GetComponent<Villager>();
                entityManager.Free(villager.VillageID);
                villager.VillageID = -1;
            }

            entityManager.Free(owner.EntityID);
            owner.EntityID = -1;

            entity.Destroy();
        }
    }
}
