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

    public LevelFactory(Game game, LevelState gameState)
    {
        this.game = game;
        levelState = gameState;

        terrainBuilder = CreateTerrainBuilder();
        treeBuilder = CreateResourceBuilder("pine tree", 0.5f);
        rockBuilder = CreateResourceBuilder("rock pile", 0.1f);
        depositeBuilder = CreateResourceBuilder("ore", 0.6f, new Rectangle(32, 0, 32, 32));
        deerBuilder = CreateAnimalBuilder("deer", 0.8f, new Rectangle(0, 0, 32, 32));
    }

    #region builders
    private IEntityBuilder CreateTerrainBuilder()
    {
        IEntityBuilder builder = game.Factory.CreateEntityBuilder(levelState.ECSWorld);

        builder.AddComponent<Transform>();
        builder.AddComponent<Appearance>();

        return builder;
    }

    private IEntityBuilder CreateResourceBuilder(string textureName, float scale, Rectangle? sourceRectangle = null)
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

    private IEntityBuilder CreateAnimalBuilder(string textureName, float scale, Rectangle? sourceRectangle)
    {
        IEntityBuilder builder = CreateResourceBuilder(textureName, scale, sourceRectangle);

        builder.AddComponent(new Movement()
        {
            Speed = 30.0f,
        });
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

    private static IEntity CreateAnimal(IEntityBuilder builder, Vector2 position)
    {
        IEntity entity = builder.Build();

        entity.GetComponent<Transform>().Position = position;
        entity.GetComponent<Movement>().Destination = position;

        return entity;
    }
    #endregion

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
        => CreateAnimal(deerBuilder, position);

    public IEntity CreateTerrain(Texture2D texture)
    {
        IEntity entity = terrainBuilder.Build();

        entity.GetComponent<Appearance>().Sprite.Texture = texture;
        entity.GetComponent<Appearance>().Sprite.Origin = texture.GetSize() / 2.0f;

        return entity;
    }
}
