using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;
using WorldSimulator.Extensions;

namespace WorldSimulator.GameStates.Level;
/// <summary>
/// Factory for level state entities.
/// </summary>
public class LevelFactory
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
        this.levelState = gameState;

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
    public IEntity CreateBasicEntity(Texture2D texture)
    {
        IEntity entity = basicEntity.Build();

        entity.GetComponent<Appearance>().Sprite.Texture = texture;
        entity.GetComponent<Appearance>().Sprite.Origin = texture.GetSize() / 2.0f;

        return entity;
    }
}
