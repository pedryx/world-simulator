#include "terrain.fx"

extern float2 resolution;
extern float2 worldSize;

extern float2 resolutionScale;
extern float2 cameraPos;
extern float cameraScale;

void MainPS(in float4 screenPos : SV_Position, out float4 color : SV_Target)
{   
    float2 screenOffset = screenPos.xy - 0.5 * resolution * resolutionScale;
    float2 scale = cameraScale * resolutionScale;
    // in MonoGame y coordinate is flipped when using default render target
    float2 cameraOffset = float2(cameraPos.x, -cameraPos.y + worldSize.y);
    float2 noisePos = screenOffset / scale + cameraOffset;

    if (any(noisePos < float2(0.0, 0.0) || noisePos >= worldSize))
    {
        color = float4(terrains[0].color, 1.0);
        return;
    }

    float height = CalcNoise(noisePos);
    color =  float4(GetTerrain(height).color, 1.0);
}

technique TerrainDraw
{
    pass Pass0
    {
        PixelShader = compile ps_5_0 MainPS();
    }
};