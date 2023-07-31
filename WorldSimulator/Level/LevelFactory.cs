using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.Level;
/// <summary>
/// Factory for level state entities.
/// </summary>
internal class LevelFactory
{
    private readonly Game game;
    private readonly LevelState levelState;

    private readonly IEntityBuilder terrainBuilder;
    private readonly IEntityBuilder treeBuilder;
    private readonly IEntityBuilder rockBuilder;
    private readonly IEntityBuilder depositeBuilder;
    private readonly IEntityBuilder deerBuilder;
    private readonly IEntityBuilder villagerBuilder;

    public LevelFactory(Game game, LevelState gameState)
    {
        this.game = game;
        levelState = gameState;

        terrainBuilder = CreateTerrainBuilder();
        treeBuilder = CreateStaticBuilder("tree", 0.4f);
        rockBuilder = CreateStaticBuilder("boulder", 0.1f);
        depositeBuilder = CreateStaticBuilder("iron deposite", 0.1f);
        deerBuilder = CreateAnimalBuilder("deer", 0.1f);
        villagerBuilder = CreateAnimalBuilder("villager", 0.1f);
    }

    #region builders
    private IEntityBuilder CreateTerrainBuilder()
    {
        IEntityBuilder builder = game.Factory.CreateEntityBuilder(levelState.ECSWorld);

        builder.AddComponent<Transform>();
        builder.AddComponent<Appearance>();

        return builder;
    }

    private IEntityBuilder CreateStaticBuilder(string textureName, float scale, Rectangle? sourceRectangle = null)
    {
        IEntityBuilder builder = game.Factory.CreateEntityBuilder(levelState.ECSWorld);
        Texture2D texture = game.GetResourceManager<Texture2D>()[textureName];

        builder.AddComponent<Transform>();
        builder.AddComponent(new Appearance(new Sprite()
        {
            Texture = texture,
            Origin = (sourceRectangle == null ? texture.GetSize() : sourceRectangle.Value.Size.ToVector2())
                * new Vector2(0.5f, 1.0f),
            Scale = scale,
            SourceRectangle = sourceRectangle,
        }));
        builder.AddComponent<LayerUpdate>();

        return builder;
    }

    private IEntityBuilder CreateAnimalBuilder(string textureName, float scale, Rectangle? sourceRectangle = null)
    {
        IEntityBuilder builder = CreateStaticBuilder(textureName, scale, sourceRectangle);

        builder.AddComponent(new Movement()
        {
            Speed = 30.0f,
        });
        builder.AddComponent<PathFollow>();
        builder.AddComponent<AnimalController>();

        return builder;
    }
    #endregion

    #region shared create methods
    private static IEntity CreateEntity(IEntityBuilder builder, Vector2 position)
    {
        IEntity entity = builder.Build();

        entity.GetComponent<Transform>().Position = position;

        return entity;
    }
    #endregion

    public IEntity CreateVillager(Vector2 position)
        => CreateEntity(villagerBuilder, position);

    public IEntity CreateResource(Resource resource, Vector2 position)
    {
        if (resource == Resources.Tree)
            return CreateTree(position);
        else if (resource == Resources.Rock)
            return CreateRock(position);
        else if (resource == Resources.Deposite)
            return CreateDeposite(position);
        else if (resource == Resources.Deer)
            return CreateDeer(position);

        throw new InvalidOperationException("Entity for this resoure not exist!");
    }

    public IEntity CreateTree(Vector2 position)
        => CreateEntity(treeBuilder, position);

    public IEntity CreateRock(Vector2 position)
        => CreateEntity(rockBuilder, position);

    public IEntity CreateDeposite(Vector2 position)
        => CreateEntity(depositeBuilder, position);

    public IEntity CreateDeer(Vector2 position)
        => CreateEntity(deerBuilder, position);

    public IEntity CreateTerrain(Texture2D texture)
    {
        IEntity entity = terrainBuilder.Build();

        entity.GetComponent<Appearance>().Sprite.Texture = texture;

        return entity;
    }
}
