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
    private IEntityBuilder basicEntity;

    public LevelFactory(Game game, LevelState gameState)
    {
        this.game = game;
        levelState = gameState;

        CreateBasicEntityBuilder();
    }

    private void CreateBasicEntityBuilder()
    {
        basicEntity = game.Factory.CreateEntityBuilder(levelState.ECSWorld);
        basicEntity.AddComponent<Transform>();
        basicEntity.AddComponent<Appearance>();
    }

    /// <summary>
    /// Create basic entity with only transform and appearance components.
    /// </summary>
    public IEntity CreateBasicEntity(Texture2D texture, Vector2 origin, float scale = 1.0f, Vector2 position = default)
    {
        IEntity entity = basicEntity.Build();
        ref Appearance appearance = ref entity.GetComponent<Appearance>();

        appearance.Sprite.Texture = texture;
        appearance.Sprite.Origin = origin;
        appearance.Sprite.Scale = scale;
        entity.GetComponent<Transform>().Position = position;

        return entity;
    }

    /// <summary>
    /// Create basic entity with only transform and appearance components.
    /// </summary>
    public IEntity CreateBasicEntity(Texture2D texture, float scale = 1.0f, Vector2 position = default)
        => CreateBasicEntity(texture, texture.GetSize() / 2.0f, scale, position);

    private IEntity CreateResource(Texture2D texture, float scale = 1.0f, Vector2 position = default)
        => CreateBasicEntity(texture, texture.GetSize() * new Vector2(0.5f, 1.0f), scale, position);

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


}
