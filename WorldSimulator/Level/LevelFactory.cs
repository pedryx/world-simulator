using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;
using System.Diagnostics;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Villages;

namespace WorldSimulator.Level;
/// <summary>
/// Factory for creating entities within the game's level state.
/// </summary>
internal class LevelFactory
{
    private readonly Game game;
    private readonly LevelState levelState;

    public IEntityBuilder TerrainBuilder { get; private init; }
    public IEntityBuilder VillagerBuilder { get; private init; }

    public IEntityBuilder TreeBuilder { get; private init; }
    public IEntityBuilder RockBuilder { get; private init; }
    public IEntityBuilder DepositBuilder { get; private init; }
    public IEntityBuilder DeerBuilder { get; private init; }

    public IEntityBuilder MainBuildingBuilder { get; private init; }
    public IEntityBuilder StockpileBuilder { get; private init; }

    public IEntityBuilder WoodcutterHutBuilder { get; private init; }
    public IEntityBuilder MinerHutBuilder { get; private init; }
    public IEntityBuilder SmithyBuilder { get; private init; }
    public IEntityBuilder HunterHutBuilder { get; private init; }

    public LevelFactory(Game game, LevelState levelState)
    {
        this.game = game;
        this.levelState = levelState;

        TerrainBuilder = CreateTerrainBuilder();
        VillagerBuilder = CreateVillagerBuilder();
        TreeBuilder = CreateResourceBuilder("tree", 0.4f, ResourceType.Tree);
        RockBuilder = CreateResourceBuilder("boulder", 0.15f, ResourceType.Rock);
        DepositBuilder = CreateResourceBuilder("iron deposit", 0.15f, ResourceType.Deposit);
        DeerBuilder = CreateAnimalBuilder("deer", 0.2f, ResourceType.Deer);

        MainBuildingBuilder = CreateBasicBuilder("main building", 0.9f);
        StockpileBuilder = CreateStorageBuilder("stockpile", 0.3f);
        WoodcutterHutBuilder = CreateBasicBuilder("woodcutter hut", 0.6f);
        MinerHutBuilder = CreateBasicBuilder("miner hut", 0.6f);
        SmithyBuilder = CreateBasicBuilder("smithy", 0.6f);
        HunterHutBuilder = CreateBasicBuilder("hunter hut", 0.6f);
    }

    /// <summary>
    /// Creates a non-moving entity using the specified entity builder and spawn position.
    /// </summary>
    /// <param name="builder">The entity builder used to create the entity.</param>
    /// <param name="spawnPosition">The position at which the entity will spawn.</param>
    /// <returns>The created entity.</returns>
    public static IEntity CreateStatic(IEntityBuilder builder, Vector2 spawnPosition)
    {
        IEntity entity = builder.Build();

        Debug.Assert(!entity.HasComponent<Movement>());

        entity.GetComponent<Location>().Position = spawnPosition;
        entity.GetComponent<Owner>().Entity = entity;

        return entity;
    }

    /// <summary>
    /// Creates a moving entity using the specified entity builder and spawn position.
    /// </summary>
    /// <param name="builder">The entity builder used to create the entity.</param>
    /// <param name="spawnPosition">The position at which the entity will spawn.</param>
    /// <returns>The created entity.</returns>
    public static IEntity CreateDynamic(IEntityBuilder builder, Vector2 spawnPosition)
    {
        IEntity entity = builder.Build();

        Debug.Assert(entity.HasComponent<Movement>());

        entity.GetComponent<Location>().Position = spawnPosition;
        entity.GetComponent<Owner>().Entity = entity;
        entity.GetComponent<Movement>().Destination = spawnPosition;

        return entity;
    }

    /// <summary>
    /// Creates an entity representing a specified resource type at the given spawn position.
    /// </summary>
    /// <param name="resourceType">
    /// The type of resource to create (e.g., Tree, Rock, Deposit, Deer).
    /// </param>
    /// <param name="spawnPosition">The position at which the resource entity will spawn.</param>
    /// <returns>The created resource entity.</returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when attempting to create an entity for an unsupported resource type.
    /// </exception>

    public IEntity CreateResource(ResourceType resourceType, Vector2 spawnPosition)
    {
        if (resourceType == ResourceType.Tree)
            return CreateStatic(TreeBuilder, spawnPosition);
        else if (resourceType == ResourceType.Rock)
            return CreateStatic(RockBuilder, spawnPosition);
        else if (resourceType == ResourceType.Deposit)
            return CreateStatic(DepositBuilder, spawnPosition);
        else if (resourceType == ResourceType.Deer)
            return CreateDynamic(DeerBuilder, spawnPosition);

        throw new InvalidOperationException("Entity for this resource not exist!");
    }

    /// <summary>
    /// Creates a basic entity builder with essential components: Location, Appearance, and Owner.
    /// The Appearance component's texture is set to the specified texture, its origin is set to the
    /// middle-bottom point, and its scale is adjusted to the specified value.
    /// </summary>
    /// <param name="textureName">The name of the texture for the Appearance component.</param>
    /// <param name="scale">The scale applied to the Appearance component.</param>
    private IEntityBuilder CreateBasicBuilder(string textureName, float scale)
    {
        IEntityBuilder builder = game.Factory.CreateEntityBuilder(levelState.ECSWorld);

        builder.AddComponent<Location>();
        builder.AddComponent(new Appearance()
        {
            Texture = game.GetResourceManager<Texture2D>()[textureName],
            Origin = new Vector2(0.5f, 1.0f),
            Scale = scale,
        });
        builder.AddComponent<Owner>();

        return builder;
    }

    private IEntityBuilder CreateResourceBuilder(string textureName, float scale, ResourceType resource)
    {
        IEntityBuilder builder = CreateBasicBuilder(textureName, scale);

        builder.AddComponent(new Health()
        {
            Amount = resource.HarvestTime
        });
        builder.AddComponent(new ItemDrop()
        {
            Items = new ItemCollection(resource.HarvestItem, resource.HarvestQuantity),
        });

        return builder;
    }

    private IEntityBuilder CreateAnimalBuilder(string textureName, float scale, ResourceType resource)
    {
        IEntityBuilder builder = CreateResourceBuilder(textureName, scale, resource);

        builder.AddComponent(new Movement()
        {
            Speed = 30.0f,
        });
        builder.AddComponent(new AnimalBehavior()
        {
            ResourceType = ResourceType.Deer,
        });

        return builder;
    }

    private IEntityBuilder CreateStorageBuilder(string textureName, float scale)
    {
        IEntityBuilder builder = CreateBasicBuilder(textureName, scale);

        builder.AddComponent<Inventory>();

        return builder;
    }

    private IEntityBuilder CreateVillagerBuilder()
    {
        IEntityBuilder builder = CreateBasicBuilder("villager", 0.2f);

        builder.AddComponent(new Movement()
        {
            Speed = 60.0f,
        });
        builder.AddComponent<VillagerBehavior>();
        builder.AddComponent<PathFollow>();
        builder.AddComponent<Inventory>();
        builder.AddComponent(new DamageDealer()
        {
            DamagePerSecond = 1.0f,
        });

        return builder;
    }

    private IEntityBuilder CreateTerrainBuilder()
    {
        IEntityBuilder builder = game.Factory.CreateEntityBuilder(levelState.ECSWorld);

        builder.AddComponent<Location>();
        builder.AddComponent<Appearance>();

        return builder;
    }
}
