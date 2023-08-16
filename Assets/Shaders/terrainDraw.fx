#include "terrain.fx"

extern float2 resolution;

extern float2 resolutionScale;
extern float2 cameraPos;
extern float cameraScale;

float4 MainPS(float2 screenPos : SV_Position) : COLOR
{   
    float2 screenOffset = screenPos - 0.5 * resolution * resolutionScale;
    float2 scale = cameraScale * resolutionScale;
    // in MonoGame y coordinate is flipped when using default render target
    float2 cameraOffset = float2(cameraPos.x, -cameraPos.y + worldSize.y);
    float2 noisePos = screenOffset / scale + cameraOffset;

    if (any(noisePos < float2(0.0, 0.0) || noisePos >= worldSize))
    {
        return float4(terrains[0].color, 1.0);
    }

    float value = CalcNoise(noisePos);
    float3 color = GetTerrain(value).color;

    return float4(color, 1.0);
}

technique TerrainDraw
{
    pass Pass0
    {
        PixelShader = compile ps_3_0 MainPS();
    }
};