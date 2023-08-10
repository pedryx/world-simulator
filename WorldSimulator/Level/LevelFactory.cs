using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Level;
/// <summary>
/// Factory for level state entities.
/// </summary>
internal class LevelFactory
{
    private readonly Game game;
    private readonly LevelState levelState;

    private readonly IEntityBuilder terrainBuilder;
    private readonly IEntityBuilder villagerBuilder;

    private readonly IEntityBuilder treeBuilder;
    private readonly IEntityBuilder rockBuilder;
    private readonly IEntityBuilder depositeBuilder;
    private readonly IEntityBuilder deerBuilder;

    private readonly IEntityBuilder mainBuildingBuilder;

    public LevelFactory(Game game, LevelState gameState)
    {
        this.game = game;
        levelState = gameState;

        terrainBuilder = CreateTerrainBuilder();
        villagerBuilder = CreateVillagerBuilder();

        treeBuilder = CreateBasicBuilder("tree", 0.4f);
        rockBuilder = CreateBasicBuilder("boulder", 0.1f);
        depositeBuilder = CreateBasicBuilder("iron deposite", 0.1f);
        deerBuilder = CreateAnimalBuilder("deer", 0.2f);

        mainBuildingBuilder = CreateBasicBuilder("main building", 0.6f);
    }

    #region builders
    private IEntityBuilder CreateTerrainBuilder()
    {
        IEntityBuilder builder = game.Factory.CreateEntityBuilder(levelState.ECSWorld);

        builder.AddComponent<Position>();
        builder.AddComponent<Appearance>();

        return builder;
    }

    private IEntityBuilder CreateBasicBuilder(string textureName, float scale)
    {
        IEntityBuilder builder = game.Factory.CreateEntityBuilder(levelState.ECSWorld);

        builder.AddComponent<Position>();
        builder.AddComponent(new Appearance()
        {
            Texture = game.GetResourceManager<Texture2D>()[textureName],
            Origin = new Vector2(0.5f, 1.0f),
            Scale = scale,
        });
        builder.AddComponent<Owner>();

        return builder;
    }

    private IEntityBuilder CreateAnimalBuilder(string textureName, float scale)
    {
        IEntityBuilder builder = CreateBasicBuilder(textureName, scale);

        builder.AddComponent(new Movement()
        {
            Speed = 30.0f,
        });
        builder.AddComponent(new AnimalController()
        {
            ResourceType = ResourceTypes.Deer,
        });

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

        return builder;
    }
    #endregion

    #region shared create methods
    private static IEntity CreateStaticEntity(IEntityBuilder builder, Vector2 position)
    {
        IEntity entity = builder.Build();

        entity.GetComponent<Position>().Coordinates = position;
        entity.GetComponent<Owner>().Entity = entity;

        return entity;
    }

    private static IEntity CreateDynamicEntity(IEntityBuilder builder, Vector2 position)
    {
        IEntity entity = builder.Build();

        entity.GetComponent<Position>().Coordinates = position;
        entity.GetComponent<Owner>().Entity = entity;
        entity.GetComponent<Movement>().Destination = position;

        return entity;
    }
    #endregion

    public IEntity CreateVillager(Vector2 position)
        => CreateDynamicEntity(villagerBuilder, position);

    public IEntity CreateMainBuilding(Vector2 position)
        => CreateStaticEntity(mainBuildingBuilder, position);

    public IEntity CreateResource(ResourceType resource, Vector2 position)
    {
        if (resource == ResourceTypes.Tree)
            return CreateTree(position);
        else if (resource == ResourceTypes.Rock)
            return CreateRock(position);
        else if (resource == ResourceTypes.Deposite)
            return CreateDeposite(position);
        else if (resource == ResourceTypes.Deer)
            return CreateDeer(position);

        throw new InvalidOperationException("Entity for this resoure not exist!");
    }

    public IEntity CreateTree(Vector2 position)
        => CreateStaticEntity(treeBuilder, position);

    public IEntity CreateRock(Vector2 position)
        => CreateStaticEntity(rockBuilder, position);

    public IEntity CreateDeposite(Vector2 position)
        => CreateStaticEntity(depositeBuilder, position);

    public IEntity CreateDeer(Vector2 position)
        => CreateDynamicEntity(deerBuilder, position);

    public IEntity CreateTerrain(Texture2D texture, Vector2 position)
    {
        IEntity entity = terrainBuilder.Build();

        entity.GetComponent<Position>().Coordinates = position;
        entity.GetComponent<Appearance>().Texture = texture;

        return entity;
    }
}
