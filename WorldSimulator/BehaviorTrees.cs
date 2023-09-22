using BehaviourTree;
using BehaviourTree.FluentBuilder;

using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Villages;

namespace WorldSimulator;
internal class BehaviorTrees
{
    private readonly List<Pair> pairs = new();

    private void SetBehaviorTree(IEntity entity, IBehaviour<VillagerContext> behaviorTree)
    {
        ref VillagerBehavior behavior = ref entity.GetComponent<VillagerBehavior>();

        if (behavior.BehaviorTreeIndex != -1)
        {
            RemoveBehaviorTree(entity);
        }

        entity.GetComponent<VillagerBehavior>().BehaviorTreeIndex = pairs.Count;
        pairs.Add(new Pair()
        {
            Entity = entity,
            BehaviorTree = behaviorTree
        });
    }

    public void RemoveBehaviorTree(IEntity entity)
    {
        Debug.Assert(entity.HasComponent<VillagerBehavior>());

        ref VillagerBehavior behavior = ref entity.GetComponent<VillagerBehavior>();
        int index = behavior.BehaviorTreeIndex;

        pairs[index] = pairs[^1];
        pairs[index].Entity.GetComponent<VillagerBehavior>().BehaviorTreeIndex = index;

        pairs.RemoveAt(pairs.Count - 1);
        behavior.BehaviorTreeIndex = -1;
    }

    public IBehaviour<VillagerContext> GetBehaviorTree(int index)
        => pairs[index].BehaviorTree;

    public void SetHarvesterBehavior(IEntity entity, IEntity workplace, IEntity stockpile, ResourceType resourceType)
    {
        Debug.Assert(workplace.HasComponent<ResourceProcessor>());
        Debug.Assert(entity.HasComponent<VillagerBehavior>());

        entity.GetComponent<VillagerBehavior>().WorkPlace = workplace;

        IBehaviour<VillagerContext> behaviorTree = FluentBuilder.Create<VillagerContext>()
            .Sequence("villager job sequence")
                .Do("find nearest resource", FindNearestResource(resourceType))
                .Do("move to nearest resource", MoveTo(null))
                .Do("harvest resource", HarvestResource)
                .Do("move to workplace", MoveTo(workplace))
                .Do("start processing resources", StartResourceProcessing)
                .Do("wait until resources are processed", WaitForResources)
                .Do("move to stockpile", MoveTo(stockpile))
                .Do("store items", StoreItems)
            .End()
            .Build();

        SetBehaviorTree(entity, behaviorTree);
    }

    private record struct Pair(IEntity Entity, IBehaviour<VillagerContext> BehaviorTree);

    #region behavior tree nodes
    private static Func<VillagerContext, BehaviourStatus> MoveTo(IEntity target)
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

    private static Func<VillagerContext, BehaviourStatus> FindNearestResource(ResourceType resourceType)
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

    private static BehaviourStatus HarvestResource(VillagerContext context)
    {
        ref DamageDealer damageDealer = ref context.Entity.GetComponent<DamageDealer>();
        ref VillagerBehavior behavior = ref context.Entity.GetComponent<VillagerBehavior>();

        damageDealer.Target = behavior.Target;

        return damageDealer.Target.IsDestroyed() ? BehaviourStatus.Succeeded : BehaviourStatus.Running;
    }

    private static BehaviourStatus StartResourceProcessing(VillagerContext context)
    {
        IEntity workPlace = context.Entity.GetComponent<VillagerBehavior>().WorkPlace;

        ref Inventory villagerInventory = ref context.Entity.GetComponent<Inventory>();
        ref Inventory workPlaceInventory = ref workPlace.GetComponent<Inventory>();
        ref ResourceProcessor resourceProcessor = ref workPlace.GetComponent<ResourceProcessor>();

        villagerInventory.Items.TransferTo
        (
            ref workPlaceInventory.Items,
            resourceProcessor.InputItem,
            resourceProcessor.InputQuantity
        );

        return BehaviourStatus.Succeeded;
    }

    private static BehaviourStatus WaitForResources(VillagerContext context)
    {
        IEntity workPlace = context.Entity.GetComponent<VillagerBehavior>().WorkPlace;

        ref Inventory villagerInventory = ref context.Entity.GetComponent<Inventory>();
        ref Inventory workPlaceInventory = ref workPlace.GetComponent<Inventory>();
        ref ResourceProcessor resourceProcessor = ref workPlace.GetComponent<ResourceProcessor>();

        if (workPlaceInventory.Items.Has(resourceProcessor.OutputItem))
        {
            workPlaceInventory.Items.TransferTo(ref villagerInventory.Items);
            return BehaviourStatus.Succeeded;
        }

        return BehaviourStatus.Running;
    }

    private BehaviourStatus StoreItems(VillagerContext context)
    {
        IEntity stockpile = context.Entity.GetComponent<VillagerBehavior>().Stockpile;

        ref Inventory stockpileInventory = ref stockpile.GetComponent<Inventory>();
        context.Entity.GetComponent<Inventory>().Items.TransferTo(ref stockpileInventory.Items);

        return BehaviourStatus.Succeeded;
    }
    #endregion
}
