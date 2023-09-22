using Microsoft.Xna.Framework;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.Level;

namespace WorldSimulator.Systems.Villages;
internal readonly struct VillageAISystem : IEntityProcessor<Location, Village, Owner>
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
    private readonly List<BuildOrderItem> buildOrder;
    private readonly Random random;
    private readonly GameWorld gameWorld;
    private readonly LevelState levelState;

    public VillageAISystem(LevelState levelState)
    {
        this.levelState = levelState;
        levelFactory = levelState.LevelFactory;
        gameWorld = levelState.GameWorld;

        random = new Random(levelState.Game.GenerateSeed());
        buildOrder = new List<BuildOrderItem>()
        {
            new BuildOrderItem(new ItemCollection(5), levelFactory.CreateHunterHut),
            new BuildOrderItem(new ItemCollection(5), levelFactory.CreateHouse),
            new BuildOrderItem(new ItemCollection(5), levelFactory.CreateMinerHut),
            new BuildOrderItem(new ItemCollection(5), levelFactory.CreateHouse),
            new BuildOrderItem(new ItemCollection(10, 5), levelFactory.CreateHouse),
            new BuildOrderItem(new ItemCollection(10, 5), levelFactory.CreateHouse),
            new BuildOrderItem(new ItemCollection(10, 5), levelFactory.CreateHouse),
            new BuildOrderItem(new ItemCollection(20, 10), levelFactory.CreateSmithy),
            new BuildOrderItem(new ItemCollection(10, 5), levelFactory.CreateHouse),
        };
    }

    public void Process(ref Location location, ref Village village, ref Owner owner, float deltaTime)
    {
        while (TryBuild(ref location, ref village, ref owner)) ;
    }

    private bool TryBuild(ref Location location, ref Village village, ref Owner owner)
    {
        if (village.MainBuilding == null)
        {
            levelFactory.CreateMainBuilding(location.Position, owner.Entity);
            levelFactory.CreateStockpile(GetBuildingPosition(ref village), owner.Entity);
            levelFactory.CreateHouse(GetBuildingPosition(ref village), owner.Entity);
            return true;
        }

        ref Inventory inventory = ref village.StockPile.GetComponent<Inventory>();
        if (inventory.Items.Contains(buildOrder[village.BuildOrderIndex].Items))
        {
            buildOrder[village.BuildOrderIndex].BuildMethod(GetBuildingPosition(ref village), owner.Entity);
            village.BuildOrderIndex++;
            return true;
        }

        return false;
    }

    private bool TryAsignProfession(ref Village village)
    {
        return false;
    }

    private Vector2 GetBuildingPosition(ref Village village)
    {
        Vector2[] buildingPositions =  new IEntity[]
        {
            village.MainBuilding,
            village.StockPile,
            village.WoodcutterHut,
            village.MinerHut,
            village.Smithy,
            village.HunterHut
        }
        .Concat(village.Houses)
        .Where(b => b != null)
        .Select(b => b.GetComponent<Location>().Position)
        .ToArray();

        while (true)
        {
            Vector2 center = buildingPositions[random.Next(buildingPositions.Length)];
            Vector2 position = random.NextPointInRing(center, minBuildDistance, maxBuildDistance);

            var query = buildingPositions.Where(p => Vector2.DistanceSquared(p, position) < minDistanceSquared);
            if (!query.Any() && gameWorld.GetTerrain(position).Buildable)
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
