#include "terrain.fx"

extern float2 worldSize;
extern int gridDistance;

extern int seed;

extern RWStructuredBuffer<int> terrainBuffer;
extern RWStructuredBuffer<float2> resourceBuffer;
extern RWStructuredBuffer<int> sizeBuffer;

uint random(uint seed, uint exclusive)
{
    seed ^= (seed << 13);
    seed ^= (seed >> 17);
    seed ^= (seed << 5);

    return (seed * 1664525 + 1013904223) % exclusive;
}

[numthreads(1024, 1, 1)]
void MainCS(int id : SV_DispatchThreadID)
{
    for (int i = 0; i < gridDistance; i++)
    {
        int index = id * gridDistance + i;
        uint2 pos = float2(index % worldSize.x, index / worldSize.x);

        float height = CalcNoise(pos, seed);
        Terrain terrain = GetTerrain(height);

        if (random(index, chancesOrder) < terrain.resourceSpawnChance)
        {
            int bufferIndex;
            InterlockedAdd(sizeBuffer[0], 1, bufferIndex);
            resourceBuffer[bufferIndex] = pos;
        }

        if (i == 0)
        {
            terrainBuffer[id] = terrain.id;
        }
    }
}

technique TerrainGen
{
    pass Pass1
    {
        ComputeShader = compile cs_5_0 MainCS();
    }
};