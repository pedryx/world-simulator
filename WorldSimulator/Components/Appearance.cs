using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using WorldSimulator.ECS.AbstractECS;

namespace WorldSimulator.Components;
[Component]
internal struct Appearance
{
    public Texture2D Texture;
    /// <summary>
    /// Origin point of a <see cref="Texture"/>  in normalized format, using a coordinates within the range [0, 1].
    /// </summary>
    public Vector2 Origin;
    /// <summary>
    /// Scaling factor applied to the <see cref="Texture"/> during rendering.
    /// </summary>
    public float Scale = 1.0f;
    public SpriteEffects Effects;

    public Appearance() { }
}
