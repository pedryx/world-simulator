using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
/// <summary>
/// System which dynamically changes orientation of entities based on their moving direction.
/// </summary>
internal readonly struct OrientationUpdateSystem : IEntityProcessor<Position, Movement, Appearance>
{
    public void Process(ref Position position, ref Movement movement, ref Appearance appearance, float deltaTime)
    {
        Vector2 direction = movement.Destination - position.Coordinates;

        if (direction.X < movement.Speed * deltaTime)
            return;

        appearance.Effects = direction.X > 0.0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }
}
