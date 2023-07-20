using Microsoft.Xna.Framework.Graphics;

namespace WorldSimulator.Components;
internal struct Appearance
{
    public Sprite Sprite;

    public Appearance(Sprite sprite)
    {
        Sprite = sprite;
    }

    public Appearance(Texture2D texture, float scale = 1.0f)
    {
        Sprite = new Sprite(texture, scale);
    }
}
