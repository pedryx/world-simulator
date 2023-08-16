using Microsoft.Xna.Framework;

namespace WorldSimulator.Level;
internal class GameWorld
{
    /// <summary>
    /// Name of the terrain draw shader.
    /// </summary>
    public const string TerrainDrawShader = "terrainDraw";
    /// <summary>
    /// Name of the terrain generation shader.
    /// </summary>
    public const string TerrainGenShader = "terrainGen";

    /// <summary>
    /// Width and height of game world in pixels.
    /// </summary>
    public static readonly Point Size = new(8192);
    public static readonly Rectangle Bounds = new(Point.Zero, Size);
}
