using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using WorldSimulator.Components;
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
    /// <summary>
    /// Creates an entity representing a specified resource type at the given spawn position.
    /// </summary>
    /// <param name="resourceType">
    /// The type of resource to create (e.g., Tree, Rock, Deposit, Deer).
    /// </param>
    /// <param name="position">The position at which the resource entity will spawn.</param>
    /// <returns>The created resource entity.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when attempting to create an entity for an unsupported resource type.
    /// </exception>
    public IEntity CreateResource(ResourceType resourceType, Vector2 position)
    {
        if (resourceType == ResourceType.Tree)
            return CreateTree(position);
        else if (resourceType == ResourceType.Rock)
            return CreateRock(position);
        else if (resourceType == ResourceType.Deposit)
            return CreateDeposit(position);
        else if (resourceType == ResourceType.Deer)
            return CreateDeer(position);

        throw new InvalidOperationException("Entity for this resource not exist!");
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
    #region Resource Processing Buildings
    public IEntity CreateWoodcutterHut(Vector2 position)
        => CreateBasicResourceProcessor(position, "woodcutter hut", 0.6f, ResourceType.Tree);

    public IEntity CreateMinerHut(Vector2 position)
        => CreateBasicResourceProcessor(position, "miner hut", 0.6f, ResourceType.Rock);

    public IEntity CreateSmithy(Vector2 position)
        => CreateBasicResourceProcessor(position, "smithy", 0.6f, ResourceType.Deposit);

    public IEntity CreateHunterHut(Vector2 position)
        => CreateBasicResourceProcessor(position, "hunter hut", 0.6f, ResourceType.Deer);
    #endregion
    #region Other Building
    public IEntity CreateMainBuilding(Vector2 position)
        => CreateBasicEntity(position, "main building", 0.9f);

    public IEntity CreateStockpile(Vector2 position)
    {
        IEntity entity = CreateBasicEntity(position, "stockpile", 0.4f);

        entity.AddComponent<Inventory>();

        return entity;
    }

    public IEntity CreateHouse(Vector2 position)
    {
        IEntity entity = CreateBasicEntity(position, "house", 0.4f);

        entity.AddComponent<VillagerSpawner>();

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

    public IEntity CreateVillager(Vector2 position)
    {
        IEntity entity = CreateBasicEntity(position, "villager", 0.2f);

        entity.AddComponent(new Movement()
        {
            Destination = position,
            Speed = 60.0f,
        });
        entity.AddComponent<VillagerBehavior>();
        entity.AddComponent<PathFollow>();
        entity.AddComponent<Inventory>();
        entity.AddComponent(new DamageDealer()
        {
            DamagePerSecond = 1.0f,
        });

        return entity;
    }
    #endregion
    #region Shared
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

    private IEntity CreateBasicResource(Vector2 position, string textureName, float scale, ResourceType resource)
    {
        IEntity entity = CreateBasicEntity(position, textureName, scale);

        entity.AddComponent(new Health()
        {
            Amount = resource.HarvestTime
        });
        entity.AddComponent(new ItemDrop()
        {
            Items = new ItemCollection(resource.HarvestItem, resource.HarvestQuantity),
        });

        return entity;
    }

    private IEntity CreateBasicResourceProcessor
    (
        Vector2 position,
        string textureName,
        float scale,
        ResourceType resource
    )
    {
        IEntity entity = CreateBasicEntity(position, textureName, scale);

        entity.AddComponent<Inventory>();
        entity.AddComponent(new ResourceProcessor()
        {
            InputItem = resource.HarvestItem,
            InputQuantity = 1,
            OutputItem = resource.HarvestItem.ProcessedItem,
            OutputQuantity = 1,
            ProcessingTime = resource.HarvestItem.TimeToProcess,
        });

        return entity;
    }
    #endregion
}