#include "terrain.fx"

extern float2 worldSize;

extern float2 resolution;
extern float2 resolutionScale;

extern float2 texOffset;
extern float2 texOrigin;

extern float scale;
extern float2 offset;

float4 MainPS(float4 screenPos : SV_Position) : SV_Target
{
    // in HLSL origin is at bottom-left and y+ goes up
    screenPos.y = resolution.y - screenPos.y;
    float2 texPos = (screenPos.xy - texOffset);

    /*
     * 1. apply resolution scale
     * 2. set origin
     * 3. scale
     * 4. translate
     */
    float2 noisePos = texPos;
    noisePos /= resolutionScale;
    noisePos -= texOrigin;
    noisePos /= scale;
    noisePos += offset;

    if (any(noisePos < float2(0.0, 0.0) || noisePos >= worldSize))
    {
        return float4(terrains[0].color, 1.0);
    }

    float height = CalcNoise(noisePos);

    return float4(GetTerrain(height).color, 1.0);
}

technique TerrainDraw
{
    pass Pass0
    {
        PixelShader = compile ps_5_0 MainPS();
    }
};