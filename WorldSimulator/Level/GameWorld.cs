namespace WorldSimulator.Level;
internal class GameWorld
{
    private readonly Terrain[][] terrainMap;

    public GameWorld(Terrain[][] terrainMap)
    {
        this.terrainMap = terrainMap;
    }

    public Terrain GetTerrain(int x, int y)
    {
        if (x < -terrainMap.Length / 2 || x >= terrainMap.Length / 2
            || y < -terrainMap.Length / 2 || y >= terrainMap.Length / 2)
        {
            return null;
        }

        return terrainMap[y + terrainMap.Length / 2][x + terrainMap.Length / 2];
    }
}
