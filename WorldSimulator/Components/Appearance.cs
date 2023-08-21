using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
[Component]
internal struct Appearance
{
    public Texture2D Texture;
    /// <summary>
    /// The origin point of the <see cref="Texture"/>  in a normalized format, using coordinates within the range
    /// [0, 1].
    /// </summary>
    public Vector2 Origin;
    /// <summary>
    /// The scaling factor applied to the <see cref="Texture"/> during rendering.
    /// </summary>
    public float Scale = 1.0f;
    /// <summary>
    /// Effects applied to the <see cref="Texture"/> during rendering.
    /// </summary>
    public SpriteEffects Effects;

    public Appearance() { }
}
