using Microsoft.Xna.Framework;

namespace WorldSimulator.Level;
internal record GameWorldData(int[] TerrainData, Vector2[] ResourcePositions, int Seed);
