using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

using WorldSimulator.Components;
using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Systems;
/// <summary>
/// A system that dynamically changes the orientation of entities based on their moving direction.
/// </summary>
internal readonly struct OrientationUpdateSystem : IEntityProcessor<Location, Movement, Appearance>
{
    public void Process(ref Location location, ref Movement movement, ref Appearance appearance, float deltaTime)
    {
        Vector2 direction = movement.Destination - location.Position;

        if (MathF.Abs(direction.X) < movement.Speed * deltaTime)
            return;

        appearance.Effects = direction.X > 0.0f ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
    }
}
