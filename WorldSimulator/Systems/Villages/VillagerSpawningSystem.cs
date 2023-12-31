﻿using BehaviourTree;
using BehaviourTree.FluentBuilder;

using Microsoft.Xna.Framework;

using System;
using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Level;

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

    public VillagerSpawningSystem(LevelFactory levelFactory, BehaviorTrees behaviorTrees)
    {
        this.levelFactory = levelFactory;
        this.behaviorTrees = behaviorTrees;
    }

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Location location, ref VillagerSpawner villagerSpawner, ref Owner owner, float deltaTime)
    {
        if (villagerSpawner.JustSpawned)
        {
            villagerSpawner.JustSpawned = false;

            IEntity stockpile = villagerSpawner.Village.GetComponent<Village>().Stockpile;
            ResourceType resource = villagerSpawner.Profession switch
            {
                VillagerProfession.Woodcutter => ResourceType.Tree,
                VillagerProfession.StoneMiner => ResourceType.Rock,
                VillagerProfession.IronMiner => ResourceType.Deposit,
                VillagerProfession.Hunter => ResourceType.Deer,

                _ => throw new InvalidOperationException("Unsupported villager profession."),
            };

            SetProfession(villagerSpawner.Villager, resource, owner.Entity, stockpile, stockpile);
        }

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
            ref Inventory inventory = ref foodStorage.GetComponent<Inventory>();

            return inventory.Items.Has(ItemType.Food);
        };
    }
    
    private static BehaviourStatus EatFood(BehaviorContext context)
    {
        ref Inventory inventory = ref context.Entity.GetComponent<Inventory>();
        ref Hunger hunger = ref context.Entity.GetComponent<Hunger>();

        inventory.Items.Remove(ItemType.Food, 1);
        hunger.Amount = 0.0f;

        return BehaviourStatus.Succeeded;
    }

    private static Func<BehaviorContext, BehaviourStatus> GetFood(IEntity foodStockpile)
    {
        return (context) =>
        {
            ref Inventory stockpileInventory = ref foodStockpile.GetComponent<Inventory>();
            ref Inventory inventory = ref context.Entity.GetComponent<Inventory>();

            stockpileInventory.Items.TransferTo(ref inventory.Items, ItemType.Food, 1);
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
            ref PathFollow pathFollow = ref context.Entity.GetComponent<PathFollow>();

            if (pathFollow.PathIndex == pathFollow.Path.Length)
            {
                // when two entities are at same position, depth layer fighting occur
                Vector2 targetPosition = (target ?? context.Entity.GetComponent<VillagerBehavior>().Target)
                    .GetComponent<Location>().Position + Vector2.UnitY;
                Vector2 position = context.Entity.GetComponent<Location>().Position;
                float speed = context.Entity.GetComponent<Movement>().Speed;

                if (position.IsCloseEnough(targetPosition, speed * context.DeltaTime))
                    return BehaviourStatus.Succeeded;

                pathFollow.Path = context.GameWorld.FindPath(position, targetPosition);
                pathFollow.PathIndex = 0;
            }

            return BehaviourStatus.Running;
        };
    }

    private static Func<BehaviorContext, BehaviourStatus> FindNearestResource(ResourceType resourceType)
    {
        return (context) =>
        {
            Vector2 position = context.Entity.GetComponent<Location>().Position;
            IEntity resource = context.GameWorld.GetAndRemoveNearestResource(resourceType, position);

            if (resource == null)
                return BehaviourStatus.Failed;

            context.Entity.GetComponent<VillagerBehavior>().Target = resource;

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
        ref DamageDealer damageDealer = ref context.Entity.GetComponent<DamageDealer>();
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        damageDealer.Target = behavior.Target;

        return damageDealer.Target.IsDestroyed() ? BehaviourStatus.Succeeded : BehaviourStatus.Running;
    }

    private static Func<BehaviorContext, BehaviourStatus> StartResourceProcessing(IEntity workplace)
    {
        return (context) =>
        {
            ref Inventory villagerInventory = ref context.Entity.GetComponent<Inventory>();
            ref Inventory workPlaceInventory = ref workplace.GetComponent<Inventory>();
            ref ResourceProcessor resourceProcessor = ref workplace.GetComponent<ResourceProcessor>();

            villagerInventory.Items.TransferTo
            (
                ref workPlaceInventory.Items,
                resourceProcessor.InputItem,
                resourceProcessor.InputQuantity
            );

            return BehaviourStatus.Succeeded;
        };
    }

    private static Func<BehaviorContext, BehaviourStatus> WaitForResources(IEntity workplace)
    {
        return (context) =>
        {
            ref Inventory villagerInventory = ref context.Entity.GetComponent<Inventory>();
            ref Inventory workPlaceInventory = ref workplace.GetComponent<Inventory>();
            ref ResourceProcessor resourceProcessor = ref workplace.GetComponent<ResourceProcessor>();

            if (workPlaceInventory.Items.Has(resourceProcessor.OutputItem))
            {
                workPlaceInventory.Items.TransferTo(ref villagerInventory.Items);
                return BehaviourStatus.Succeeded;
            }

            return BehaviourStatus.Running;
        };
    }

    private static Func<BehaviorContext, BehaviourStatus> StoreItems(IEntity stockpile)
    {
        return (context) =>
        {
            ref Inventory stockpileInventory = ref stockpile.GetComponent<Inventory>();
            context.Entity.GetComponent<Inventory>().Items.TransferTo(ref stockpileInventory.Items);

            return BehaviourStatus.Succeeded;
        };
    }
    #endregion
}
