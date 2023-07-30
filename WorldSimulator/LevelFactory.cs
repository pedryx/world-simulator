using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;
using WorldSimulator.GameStates;

namespace WorldSimulator;
/// <summary>
/// Factory for level state entities.
/// </summary>
internal class LevelFactory
{
    private readonly Game game;
    private readonly LevelState levelState;

    /// <summary>
    /// Builder for basic entities with only transform and appearance components.
    /// </summary>
    private readonly IEntityBuilder basicBuilder;
    /// <summary>
    /// Builder for resources.
    /// </summary>
    private readonly IEntityBuilder resourceBuilder;
    private readonly IEntityBuilder animalBuilder;

    public LevelFactory(Game game, LevelState gameState)
    {
        this.game = game;
        levelState = gameState;

        basicBuilder = CreateBasicBuilder();
        resourceBuilder = CreateResourceBuilder();
        animalBuilder = CreateAnimalBuilder();
    }

    private IEntityBuilder CreateBasicBuilder()
    {
        IEntityBuilder builder = game.Factory.CreateEntityBuilder(levelState.ECSWorld);

        builder.AddComponent<Transform>();
        builder.AddComponent<Appearance>();

        return builder;
    }

    private IEntityBuilder CreateResourceBuilder()
    {
        IEntityBuilder builder = CreateBasicBuilder();

        builder.AddComponent<LayerUpdate>();

        return builder;
    }

    private IEntityBuilder CreateAnimalBuilder()
    {
        IEntityBuilder builder = CreateResourceBuilder();

        builder.AddComponent(new Movement()
        {
            Speed = 30.0f,
        });
        builder.AddComponent<RandomMovement>();

        return builder;
    }

    private IEntity CreateResource
    (
        IEntityBuilder builder,
        Texture2D texture,
        float scale,
        Vector2 position,
        Rectangle? sourceRectangle = null
    )
    {
        IEntity entity = builder.Build();

        ref Appearance appearance = ref entity.GetComponent<Appearance>();
        appearance.Sprite.Texture = texture;
        appearance.Sprite.Origin = (sourceRectangle == null ? texture.GetSize() : sourceRectangle.Value.Size.ToVector2())
            * new Vector2(0.5f, 1.0f);
        appearance.Sprite.Scale = scale;
        appearance.Sprite.SourceRectangle = sourceRectangle;

        entity.GetComponent<Transform>().Position = position;

        return entity;
    }

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
        => CreateResource(resourceBuilder, game.GetResourceManager<Texture2D>()["pine tree"], 0.5f, position);

    public IEntity CreateRock(Vector2 position)
        => CreateResource(resourceBuilder, game.GetResourceManager<Texture2D>()["rock pile"], 0.1f, position);

    public IEntity CreateDeposite(Vector2 position)
    {
        return CreateResource
        (
            resourceBuilder,
            game.GetResourceManager<Texture2D>()["ore"],
            0.6f,
            position,
            new Rectangle(32, 0, 32, 32)
        );
    }

    public IEntity CreateDeer(Vector2 position)
    {
        return CreateResource
        (
            animalBuilder,
            game.GetResourceManager<Texture2D>()["deer"],
            0.8f,
            position,
            new Rectangle(0, 0, 32, 32)
        );
    }

    public IEntity CreateTerrain(Texture2D texture)
    {
        IEntity entity = basicBuilder.Build();

        entity.GetComponent<Appearance>().Sprite.Texture = texture;
        entity.GetComponent<Appearance>().Sprite.Origin = texture.GetSize() / 2.0f;

        return entity;
    }
}
