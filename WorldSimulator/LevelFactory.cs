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

    public LevelFactory(Game game, LevelState gameState)
    {
        this.game = game;
        levelState = gameState;

        basicBuilder = CreateBasicBuilder();
        resourceBuilder = CreateResourceBuilder();
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

    private IEntity CreateResource(Texture2D texture, float scale, Vector2 position)
    {
        IEntity entity = resourceBuilder.Build();

        entity.GetComponent<Appearance>().Sprite.Texture = texture;
        entity.GetComponent<Appearance>().Sprite.Origin = texture.GetSize() * new Vector2(0.5f, 1.0f);
        entity.GetComponent<Appearance>().Sprite.Scale = scale;

        entity.GetComponent<Transform>().Position = position;

        return entity;
    }

    public IEntity CreateResource(Resource resource, Vector2 position)
    {
        if (resource == Resources.Tree)
            return CreateTree(position);
        else if (resource == Resources.Rock)
            return CreateRock(position);

        throw new InvalidOperationException("Entity for this resoure not exist!");
    }

    public IEntity CreateTree(Vector2 position)
        => CreateResource(game.GetResourceManager<Texture2D>()["pine tree"], 0.5f, position);

    public IEntity CreateRock(Vector2 position)
        => CreateResource(game.GetResourceManager<Texture2D>()["rock pile"], 0.1f, position);

    public IEntity CreateTerrain(Texture2D texture)
    {
        IEntity entity = basicBuilder.Build();

        entity.GetComponent<Appearance>().Sprite.Texture = texture;
        entity.GetComponent<Appearance>().Sprite.Origin = texture.GetSize() / 2.0f;

        return entity;
    }
}
