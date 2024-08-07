﻿using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Level;
using WorldSimulator.ManagedDataManagers;

namespace WorldSimulator.Systems.Villages;
internal readonly struct VillageBuildingSystem : IEntityProcessor<Location, Village, Owner>
{
    /// <summary>
    /// Minimal distance of a building to all other buildings.
    /// </summary>
    private const float minBuildDistance = 150.0f;
    /// <summary>
    /// Maximal distance of a building to any other building.
    /// </summary>
    private const float maxBuildDistance = 200.0f;
    /// <summary>
    /// Second power of minimal building distance.
    /// </summary>
    private const float minDistanceSquared = minBuildDistance * minBuildDistance;

    private readonly LevelFactory levelFactory;
    private readonly GameWorld gameWorld;
    private readonly ManagedDataManager<IEntity> entityManager;
    private readonly ManagedDataManager<ItemCollection?> itemCollectionManager;
    private readonly ManagedDataManager<IEntity[]> entityArrayManager;
    private readonly ManagedDataManager<Random> randomManager;

    /// <summary>
    /// At first buildings are build according to the build order.
    /// </summary>
    private readonly List<BuildOrderItem> buildOrder;
    /// <summary>
    /// When all builds from the build order are build, then buildings are build according to the build loop.
    /// </summary>
    private readonly List<Func<Vector2, IEntity, IEntity>> buildLoop;
    /// <summary>
    /// The initial cost of a building in the build loop.
    /// </summary>
    private readonly ItemCollection buildLoopInitialCost;
    /// <summary>
    /// The increase in cost of a building in the build loop. Increase is applied with each building build from the
    /// build loop.
    /// </summary>
    private readonly ItemCollection buildLoopIncreaseCost;

    public VillageBuildingSystem(Game game, LevelState levelState)
    {
        levelFactory = levelState.LevelFactory;
        gameWorld = levelState.GameWorld;

        entityManager = game.GetManagedDataManager<IEntity>();
        itemCollectionManager = game.GetManagedDataManager<ItemCollection?>();
        entityArrayManager = game.GetManagedDataManager<IEntity[]>();
        randomManager = game.GetManagedDataManager<Random>();

        buildOrder = new List<BuildOrderItem>()
        {
            new(new ItemCollection(), levelFactory.CreateWoodcutterHut),
            new(new ItemCollection(5), levelFactory.CreateHunterHut),
            new(new ItemCollection(5), levelFactory.CreateMinerHut),
            new(new ItemCollection(10, 5), levelFactory.CreateSmithy),
        };
        buildLoopInitialCost = new ItemCollection(15, 10, 5);
        buildLoopIncreaseCost = new ItemCollection(5, 5, 5);
        buildLoop = new List<Func<Vector2, IEntity, IEntity>>()
        {
            levelFactory.CreateWoodcutterHut,
            levelFactory.CreateHunterHut,
            levelFactory.CreateMinerHut,
            levelFactory.CreateSmithy,
        };
    }

    [MethodImpl(Game.EntityProcessorInline)]
    public void Process(ref Location location, ref Village village, ref Owner owner, float deltaTime)
    {
        TryBuild(ref location, ref village, ref owner);
    }

    /// <summary>
    /// Try build next building. At first takes buildings from the build order, after that it will takes buildings from
    /// the build loop.
    /// </summary>
    /// <param name="location">Village's location component.</param>
    /// <param name="village">Village's village component.</param>
    /// <param name="owner">Village's owner component.</param>
    /// <returns></returns>
    private void TryBuild(ref Location location, ref Village village, ref Owner owner)
    {
        IEntity entity = entityManager[owner.EntityID];

        if (village.MainBuildingID == -1)
        {
            levelFactory.CreateMainBuilding(location.Position, entity);
            return;
        }

        if (village.StockpileID == -1)
        {
            levelFactory.CreateStockpile(GetBuildingPosition(ref village), entity);
            return;
        }

        IEntity stockpileEntity = entityManager[village.StockpileID];

        ref Inventory inventory = ref stockpileEntity.GetComponent<Inventory>();
        ItemCollection inventoryItems = itemCollectionManager[inventory.ItemCollectionID].Value;

        if (village.BuildOrderIndex >= buildOrder.Count)
        {
            ItemCollection cost = buildLoopInitialCost 
                + buildLoopIncreaseCost * (village.BuildOrderIndex - buildOrder.Count);
            if (inventoryItems.Contains(cost))
            {
                inventoryItems.Remove(cost);
                buildLoop[(village.BuildOrderIndex - buildOrder.Count) % buildLoop.Count]
                (
                    GetBuildingPosition(ref village),
                    entity
                );
                village.BuildOrderIndex++;
            }
        }
        else if (inventoryItems.Contains(buildOrder[village.BuildOrderIndex].Items))
        {
            inventoryItems.Remove(buildOrder[village.BuildOrderIndex].Items);
            buildOrder[village.BuildOrderIndex].BuildMethod(GetBuildingPosition(ref village), entity);
            village.BuildOrderIndex++;
        }

        itemCollectionManager[inventory.ItemCollectionID] = inventoryItems;
    }

    /// <summary>
    /// Get next suitable building position.
    /// </summary>
    private Vector2 GetBuildingPosition(ref Village village)
    {
        Vector2[] buildingPositions =  new IEntity[]
        {
            entityManager[village.MainBuildingID],
            entityManager[village.StockpileID],
        }
        .Concat(entityArrayManager[village.BuildingsArrayID])
        .Where(b => b != null)
        .Select(b => b.GetComponent<Location>().Position)
        .ToArray();

        Random random = randomManager[village.RandomID];

        while (true)
        {
            Vector2 center = buildingPositions[random.Next(buildingPositions.Length)];
            Vector2 position = random.NextPointInRing(center, minBuildDistance, maxBuildDistance);

            var query = buildingPositions.Where(p => Vector2.DistanceSquared(p, position) < minDistanceSquared);
            if (!query.Any() && gameWorld.CanBuildAt(position))
            {
                return position;
            }
        }
    }

    /// <summary>
    /// Build order item.
    /// </summary>
    /// <param name="Items">Cost of the building.</param>
    /// <param name="BuildMethod">Function to build the building.</param>
    private record struct BuildOrderItem(ItemCollection Items, Func<Vector2, IEntity, IEntity> BuildMethod);
}