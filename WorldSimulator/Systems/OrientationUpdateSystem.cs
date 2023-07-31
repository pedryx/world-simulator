using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
/// <summary>
/// Dynamically changes orientation of entity based on its moving direction.
/// </summary>
internal readonly struct OrientationUpdateSystem : IEntityProcessor<Transform, Movement, Appearance>
{
    public void Process(ref Transform transform, ref Movement movement, ref Appearance appearance, float deltaTime)
    {
        Vector2 direction = movement.Destination - transform.Position;

        if (direction.X == 0)
            return;

        if (direction.X < 0.0f)
            appearance.Sprite.Effects = SpriteEffects.FlipHorizontally;
        else
            appearance.Sprite.Effects = SpriteEffects.None;
    }
}
