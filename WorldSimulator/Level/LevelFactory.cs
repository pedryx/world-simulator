using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Diagnostics;

using WorldSimulator.Components;
using WorldSimulator.Components.Villages;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Level;
/// <summary>
/// Factory for creating entities within the game's level state.
/// </summary>
internal class LevelFactory
{
    private readonly Game game;
    private readonly LevelState levelState;

    public LevelFactory(Game game, LevelState levelState)
    {
        this.game = game;
        this.levelState = levelState;
    }

    #region Resources
    private IEntity CreateBasicResource(Vector2 position, string textureName, float scale, ResourceType type)
    {
        IEntity entity = CreateBasicEntity(position, textureName, scale);

        entity.AddComponent(new Health()
        {
            Amount = type.HarvestTime
        });
        entity.AddComponent(new ItemDrop()
        {
            Items = new ItemCollection(type.HarvestItem, type.HarvestQuantity),
        });
        entity.AddComponent(new Resource(type));

        return entity;
    }

    public IEntity CreateTree(Vector2 position)
        => CreateBasicResource(position, "tree", 0.4f, ResourceType.Tree);

    public IEntity CreateRock(Vector2 position)
        => CreateBasicResource(position, "boulder", 0.15f, ResourceType.Rock);

    public IEntity CreateDeposit(Vector2 position)
        => CreateBasicResource(position, "iron deposit", 0.15f, ResourceType.Deposit);
    
    public IEntity CreateDeer(Vector2 position)
    {
        IEntity entity = CreateBasicResource(position, "deer", 0.2f, ResourceType.Deer);

        entity.AddComponent(new Movement()
        {
            Speed = 30.0f,
            Destination = position,
        });
        entity.AddComponent(new AnimalBehavior()
        {
            ResourceType = ResourceType.Deer,
        });

        return entity;
    }
    #endregion
    #region Buildings
    private IEntity CreateBasicBuilding
    (
        Vector2 position,
        string textureName,
        float scale,
        IEntity village
    )
    {
        Debug.Assert(village.HasComponent<Village>());

        IEntity entity = CreateBasicEntity(position, textureName, scale);

        entity.AddComponent(new Building()
        {
            Village = village,
        });

        return entity;
    }

    #region Resource Processing Buildings
    private IEntity CreateBasicResourceProcessingBuilding
    (
        Vector2 position,
        string textureName,
        float scale,
        IEntity village,
        ResourceType resource,
        VillagerProfession profession
    )
    {
        IEntity entity = CreateBasicBuilding(position, textureName, scale, village);

        entity.AddComponent<Inventory>();
        entity.AddComponent(new ResourceProcessor()
        {
            InputItem = resource.HarvestItem,
            InputQuantity = 1,
            OutputItem = resource.HarvestItem.ProcessedItem,
            OutputQuantity = 1,
            ProcessingTime = resource.HarvestItem.TimeToProcess,
        });
        entity.AddComponent(new VillagerSpawner()
        {
            Village = village,
            Profession = profession
        });

        ref Village villageComponent = ref village.GetComponent<Village>();
        villageComponent.Buildings[villageComponent.BuildingsCount] = entity;
        villageComponent.BuildingsCount++;

        return entity;
    }

    public IEntity CreateWoodcutterHut(Vector2 position, IEntity village)
    {
        IEntity entity = CreateBasicResourceProcessingBuilding
        (
            position,
            "woodcutter hut",
            0.6f,
            village,
            ResourceType.Tree,
            VillagerProfession.Woodcutter
        );

        return entity;
    }

    public IEntity CreateMinerHut(Vector2 position, IEntity village)
    {
        IEntity entity = CreateBasicResourceProcessingBuilding
        (
            position,
            "miner hut",
            0.6f,
            village,
            ResourceType.Rock,
            VillagerProfession.StoneMiner
        );

        return entity;
    }

    public IEntity CreateSmithy(Vector2 position, IEntity village)
    {
        IEntity entity = CreateBasicResourceProcessingBuilding
        (
            position,
            "smithy",
            0.6f,
            village,
            ResourceType.Deposit,
            VillagerProfession.IronMiner
        );

        return entity;
    }

    public IEntity CreateHunterHut(Vector2 position, IEntity village)
    {
        IEntity entity = CreateBasicResourceProcessingBuilding
        (
            position,
            "hunter hut",
            0.6f,
            village,
            ResourceType.Deer,
            VillagerProfession.Hunter
        );

        return entity;
    }
    #endregion

    public IEntity CreateMainBuilding(Vector2 position, IEntity village)
    {
        Debug.Assert(village.GetComponent<Village>().MainBuilding == null);

        IEntity entity = CreateBasicBuilding(position, "main building", 0.9f, village);

        village.GetComponent<Village>().MainBuilding = entity;

        return entity;
    }

    public IEntity CreateStockpile(Vector2 position, IEntity village)
    {
        Debug.Assert(village.GetComponent<Village>().Stockpile == null);

        IEntity entity = CreateBasicBuilding(position, "stockpile", 0.4f, village);

        entity.AddComponent<Inventory>();
        village.GetComponent<Village>().Stockpile = entity;

        return entity;
    }
    #endregion
    #region Others
    public IEntity CreateTerrain()
    {
        IEntity entity = game.Factory.CreateEntity(levelState.ECSWorld);

        entity.AddComponent<Location>();
        entity.AddComponent<Appearance>();

        return entity;
    }

    public IEntity CreateVillager(Vector2 position, IEntity village)
    {
        IEntity entity = CreateBasicEntity(position, "villager", 0.2f);

        entity.AddComponent(new Movement()
        {
            Destination = position,
            Speed = 120.0f,
        });
        entity.AddComponent<Behavior>();
        entity.AddComponent<VillagerBehavior>();
        entity.AddComponent(new Villager()
        {
            Village = village,
        });
        entity.AddComponent<PathFollow>();
        entity.AddComponent<Inventory>();
        entity.AddComponent(new DamageDealer()
        {
            DamagePerSecond = 1.0f,
        });
        entity.AddComponent(new Health()
        {
            Amount = 60.0f,
        });
        entity.AddComponent<Hunger>();

        return entity;
    }

    public IEntity CreateVillage(Vector2 position)
    {
        IEntity entity = game.Factory.CreateEntity(levelState.ECSWorld);

        entity.AddComponent(new Location()
        {
            Position = position,
        });
        entity.AddComponent<Village>();
        entity.AddComponent(new Owner()
        {
            Entity = entity,
        });

        return entity;
    }
    #endregion

    /// <summary>
    /// Creates a basic entity with essential components: Location, Appearance, and Owner. The Appearance component's
    /// texture is set to the specified texture, its origin is set to the middle-bottom point, and its scale is
    /// adjusted to the specified value.
    /// </summary>
    /// <param name="textureName">The name of the texture for the Appearance component.</param>
    /// <param name="scale">The scale applied to the Appearance component.</param>
    private IEntity CreateBasicEntity(Vector2 position, string textureName, float scale)
    {
        IEntity entity = game.Factory.CreateEntity(levelState.ECSWorld);

        entity.AddComponent(new Location()
        {
            Position = position,
        });
        entity.AddComponent(new Appearance()
        {
            Texture = game.GetResourceManager<Texture2D>()[textureName],
            Origin = new Vector2(0.5f, 1.0f),
            Scale = scale,
        });
        entity.AddComponent(new Owner()
        {
            Entity = entity,
        });

        return entity;
    }
}