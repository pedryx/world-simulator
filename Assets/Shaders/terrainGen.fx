#include "terrain.fx"

extern RWStructuredBuffer<int> terrainBuffer;

[numthreads(32, 32, 1)]
void MainCS(uint2 id : SV_DispatchThreadID)
{
    uint index = id.y * worldSize.x + id.x;
    float2 pos = float2(id.x, worldSize.y - id.y);

    float value = CalcNoise(pos);
    Terrain terrain = GetTerrain(value);

    terrainBuffer[index] = terrain.id;
}

technique TerrainGen
{
    pass Pass1
    {
        ComputeShader = compile cs_5_0 MainCS();
    }
};