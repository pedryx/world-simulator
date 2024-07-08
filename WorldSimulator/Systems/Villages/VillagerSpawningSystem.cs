using BehaviourTree;
using BehaviourTree.FluentBuilder;

using Microsoft.Xna.Framework;

using System;
using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Level;
using WorldSimulator.ManagedDataManagers;

namespace WorldSimulator.Systems.Villaages;
/// <summary>
/// System responsible for spawning villagers.
/// </summary>
internal readonly struct VillagerSpawningSystem : IEntityProcessor<Location, VillagerSpawner, Owner>
{
    /// <summary>
    /// How long it takes for a villager to spawn in seconds.
    /// </summary>
    private const float villagerSpawnTime = 60.0f;

    private readonly LevelFactory levelFactory;
    private readonly BehaviorTrees behaviorTrees;
    private readonly ManagedDataManager<IEntity> entityManager;

    public VillagerSpawningSystem(Game game, LevelFactory levelFactory, BehaviorTrees behaviorTrees)
    {
        this.levelFactory = levelFactory;
        this.behaviorTrees = behaviorTrees;
        entityManager = game.GetManagedDataManager<IEntity>();
    }

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Location location, ref VillagerSpawner villagerSpawner, ref Owner owner, float deltaTime)
    {
        IEntity entity = entityManager[owner.EntityID];
        IEntity village = entityManager[villagerSpawner.VillageID];

        if (villagerSpawner.JustSpawned)
        {
            villagerSpawner.JustSpawned = false;

            IEntity stockpile = entityManager[village.GetComponent<Village>().StockpileID];
            ResourceType resource = villagerSpawner.Profession switch
            {
                VillagerProfession.Woodcutter => ResourceType.Tree,
                VillagerProfession.StoneMiner => ResourceType.Rock,
                VillagerProfession.IronMiner => ResourceType.Deposit,
                VillagerProfession.Hunter => ResourceType.Deer,

                _ => throw new InvalidOperationException("Unsupported villager profession."),
            };

            SetProfession(entityManager[villagerSpawner.VillagerID], resource, entity, stockpile, stockpile);
        }

        if (villagerSpawner.VillagerID == -1 || entityManager[villagerSpawner.VillagerID].IsDestroyed())
        {
            villagerSpawner.Elapsed += deltaTime;

            if (villagerSpawner.Elapsed >= villagerSpawnTime)
            {
                villagerSpawner.Elapsed = 0.0f;

                IEntity villager = levelFactory.CreateVillager
                (
                    location.Position + Vector2.UnitY,
                    village
                );
                villagerSpawner.VillagerID = entityManager.Insert(villager);

                villagerSpawner.JustSpawned = true;
            }
        }
    }

    private void SetProfession
    (
        IEntity villager,
        ResourceType resource,
        IEntity workplace,
        IEntity stockpile,
        IEntity foodStockpile
    )
    {
        IBehaviour<BehaviorContext> behaviorTree = FluentBuilder.Create<BehaviorContext>()
            .Selector("root")
                .Sequence("hunger")
                    .Condition("is hunger bellow threshold?", IsHungry)
                    .Condition("is there food?", IsThereFood(foodStockpile))
                    .Do("go to get food", MoveTo(foodStockpile))
                    // We need to check again because an another villager could take the food in the meantime.
                    .Condition("is there food?", IsThereFood(foodStockpile))
                    .Do("get food", GetFood(foodStockpile))
                    .Do("eat food", EatFood)
                .End()

                .Sequence("villager job sequence")
                    .Do("find nearest resource", FindNearestResource(resource))
                    .Do("move to nearest resource", MoveTo(null))
                    .Do("harvest resource", HarvestResource)
                    .Do("move to workplace", MoveTo(workplace))
                    .Do("start processing resources", StartResourceProcessing(workplace))
                    .Do("wait until resources are processed", WaitForResources(workplace))
                    .Do("move to stockpile", MoveTo(stockpile))
                    .Do("store items", StoreItems(stockpile))
                .End()
            .End()
            .Build();

        behaviorTrees.SetBehaviorTree(villager, behaviorTree);
    }

    #region behavior tree nodes
    private static Func<BehaviorContext, bool> IsThereFood(IEntity foodStorage)
    {
        return (context) =>
        {
            ManagedDataManager<ItemCollection?> itemCollectionManager =
                context.Game.GetManagedDataManager<ItemCollection?>();

            ref Inventory inventory = ref foodStorage.GetComponent<Inventory>();
            ItemCollection inventoryItem = itemCollectionManager[inventory.ItemCollectionID].Value;

            return inventoryItem.Has(ItemType.Food);
        };
    }
    
    private static BehaviourStatus EatFood(BehaviorContext context)
    {
        ManagedDataManager<ItemCollection?> itemCollectionManager =
                context.Game.GetManagedDataManager<ItemCollection?>();

        ref Inventory inventory = ref context.Entity.GetComponent<Inventory>();
        ItemCollection inventoryItem = itemCollectionManager[inventory.ItemCollectionID].Value;
        inventoryItem.Remove(ItemType.Food, 1);


        ref Hunger hunger = ref context.Entity.GetComponent<Hunger>();
        hunger.Amount = 0.0f;

        return BehaviourStatus.Succeeded;
    }

    private static Func<BehaviorContext, BehaviourStatus> GetFood(IEntity foodStockpile)
    {
        return (context) =>
        {
            ManagedDataManager<ItemCollection?> itemCollectionManager =
                context.Game.GetManagedDataManager<ItemCollection?>();

            ref Inventory stockpileInventory = ref foodStockpile.GetComponent<Inventory>();
            ref Inventory inventory = ref context.Entity.GetComponent<Inventory>();

            ItemCollection stockpileItems = itemCollectionManager[stockpileInventory.ItemCollectionID].Value;
            ItemCollection inventoryItem = itemCollectionManager[inventory.ItemCollectionID].Value;

            stockpileItems.TransferTo(ref inventoryItem, ItemType.Food.ID, 1);

            itemCollectionManager[stockpileInventory.ItemCollectionID] = stockpileItems;
            itemCollectionManager[inventory.ItemCollectionID] = inventoryItem;

            return BehaviourStatus.Succeeded;
        };
    }

    private static bool IsHungry(BehaviorContext context)
    {
        ref Hunger hunger = ref context.Entity.GetComponent<Hunger>();

        return hunger.Amount >= 60.0f;
    }

    private static Func<BehaviorContext, BehaviourStatus> MoveTo(IEntity target)
    {
        return (context) =>
        {
            var pathManager = context.Game.GetManagedDataManager<Vector2[]>();
            var entityManager = context.Game.GetManagedDataManager<IEntity>();

            ref PathFollow pathFollow = ref context.Entity.GetComponent<PathFollow>();
            Vector2[] path = pathManager[pathFollow.PathID];

            if (pathFollow.PathNodeIndex == path.Length)
            {
                // when two entities are at same position, depth layer fighting occur
                Vector2 targetPosition = (target ?? entityManager[context.Entity.GetComponent<VillagerBehavior>().TargetID])
                    .GetComponent<Location>().Position + Vector2.UnitY;
                Vector2 position = context.Entity.GetComponent<Location>().Position;
                float speed = context.Entity.GetComponent<Movement>().Speed;

                if (position.IsCloseEnough(targetPosition, speed * context.DeltaTime))
                    return BehaviourStatus.Succeeded;

                pathManager[pathFollow.PathID] = context.GameWorld.FindPath(position, targetPosition);
                pathFollow.PathNodeIndex = 0;
            }

            return BehaviourStatus.Running;
        };
    }

    private static Func<BehaviorContext, BehaviourStatus> FindNearestResource(ResourceType resourceType)
    {
        return (context) =>
        {
            var entityManager = context.Game.GetManagedDataManager<IEntity>();

            Vector2 position = context.Entity.GetComponent<Location>().Position;
            IEntity resource = context.GameWorld.GetAndRemoveNearestResource(resourceType, position);

            if (resource == null)
                return BehaviourStatus.Failed;

            context.Entity.GetComponent<VillagerBehavior>().TargetID = entityManager.Insert(resource);

            if (resource.HasComponent<AnimalBehavior>())
            {
                resource.GetComponent<AnimalBehavior>().UpdateEnabled = false;
                resource.GetComponent<Movement>().Speed = 0.0f;
            }

            return BehaviourStatus.Succeeded;
        };
    }

    private static BehaviourStatus HarvestResource(BehaviorContext context)
    {
        var entityManager = context.Game.GetManagedDataManager<IEntity>();

        ref DamageDealer damageDealer = ref context.Entity.GetComponent<DamageDealer>();
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        damageDealer.TargetID = entityManager.Insert(entityManager[behavior.TargetID]);

        return entityManager[damageDealer.TargetID].IsDestroyed() ? BehaviourStatus.Succeeded : BehaviourStatus.Running;
    }

    private static Func<BehaviorContext, BehaviourStatus> StartResourceProcessing(IEntity workplace)
    {
        return (context) =>
        {
            ManagedDataManager<ItemCollection?> itemCollectionManager =
                context.Game.GetManagedDataManager<ItemCollection?>();

            ref ResourceProcessor resourceProcessor = ref workplace.GetComponent<ResourceProcessor>();
            ref Inventory villagerInventory = ref context.Entity.GetComponent<Inventory>();
            ref Inventory workPlaceInventory = ref workplace.GetComponent<Inventory>();

            ItemCollection villagerItems = itemCollectionManager[villagerInventory.ItemCollectionID].Value;
            ItemCollection workplaceItems = itemCollectionManager[workPlaceInventory.ItemCollectionID].Value;

            villagerItems.TransferTo
            (
                ref workplaceItems,
                resourceProcessor.InputItemID,
                resourceProcessor.InputQuantity
            );

            itemCollectionManager[villagerInventory.ItemCollectionID] = villagerItems;
            itemCollectionManager[workPlaceInventory.ItemCollectionID] = workplaceItems;

            return BehaviourStatus.Succeeded;
        };
    }

    private static Func<BehaviorContext, BehaviourStatus> WaitForResources(IEntity workplace)
    {
        return (context) =>
        {
            ManagedDataManager<ItemCollection?> itemCollectionManager =
                context.Game.GetManagedDataManager<ItemCollection?>();

            ref ResourceProcessor resourceProcessor = ref workplace.GetComponent<ResourceProcessor>();
            ref Inventory villagerInventory = ref context.Entity.GetComponent<Inventory>();
            ref Inventory workPlaceInventory = ref workplace.GetComponent<Inventory>();

            ItemCollection villagerItems = itemCollectionManager[villagerInventory.ItemCollectionID].Value;
            ItemCollection workplaceItems = itemCollectionManager[workPlaceInventory.ItemCollectionID].Value;


            if (workplaceItems.Has(ItemType.Get(resourceProcessor.OutputItemID)))
            {
                workplaceItems.TransferTo(ref villagerItems);

                itemCollectionManager[villagerInventory.ItemCollectionID] = villagerItems;
                itemCollectionManager[workPlaceInventory.ItemCollectionID] = workplaceItems;

                return BehaviourStatus.Succeeded;
            }

            return BehaviourStatus.Running;
        };
    }

    private static Func<BehaviorContext, BehaviourStatus> StoreItems(IEntity stockpile)
    {
        return (context) =>
        {
            ManagedDataManager<ItemCollection?> itemCollectionManager =
                context.Game.GetManagedDataManager<ItemCollection?>();

            ref Inventory stockpileInventory = ref stockpile.GetComponent<Inventory>();
            ref Inventory villagerInventory = ref context.Entity.GetComponent<Inventory>();

            ItemCollection stockpileItems = itemCollectionManager[stockpileInventory.ItemCollectionID].Value;
            ItemCollection villagerItems = itemCollectionManager[villagerInventory.ItemCollectionID].Value;

            villagerItems.TransferTo(ref stockpileItems);

            itemCollectionManager[stockpileInventory.ItemCollectionID] = stockpileItems;
            itemCollectionManager[villagerInventory.ItemCollectionID] = villagerItems;

            return BehaviourStatus.Succeeded;
        };
    }
    #endregion
}
