#include "lygia/generative/snoise.hlsl"

struct Octave
{
    float frequency;
    float weight;
};

struct Terrain
{
    int id;
    float height;
    uint resourceSpawnChance;
    float3 color;
};

static const float noiseScale = 1000.0;

static const int octaveCount = 3;
static const Octave octaves[] = 
{
    { 1.0, 1.0 },
    { 2.0, 0.5 },
    { 4.0, 0.25}
};

static const int chancesOrder = 10000000;

static const int terrainCount = 6;
static const Terrain terrains[] =
{
    // water
    { 0, 0.35,   0, float3(0.278, 0.725, 1.000) },
    // beach
    { 1, 0.40,   0, float3(1.000, 0.992, 0.620) },
    // plain
    { 2, 0.50,   500, float3(0.333, 0.788, 0.353) },
    // forest
    { 3, 0.70,  2000, float3(0.098, 0.522, 0.118) },
    // mountain
    { 4, 0.80,  5000, float3(0.561, 0.561, 0.561) },
    // high mountain
    { 5, 1.00, 10000, float3(1.000, 1.000, 1.000) },
};

float CalcNoise(float2 pos)
{
    float value = 0.0;
    float sum = 0.0;

    for (int i = 0; i < octaveCount; i++)
    {
        value += octaves[i].weight * snoise((pos / noiseScale) * octaves[i].frequency);
        sum += octaves[i].weight;
    }

    return (value + sum) / (2.0 * sum);
}

Terrain GetTerrain(float height)
{
    for (int i = 0; i < terrainCount - 1; i++)
    {
        if (height < terrains[i].height)
        {
            return terrains[i];
        }
    }

    return terrains[terrainCount - 1];
}