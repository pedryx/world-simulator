#include "snoise.fx"

float scale = 2.0;

struct PixelShaderInput
{
    float4 Position : SV_POSITION;
};

float4 MainPS(PixelShaderInput input) : COLOR
{
    float value = snoise(input.Position.xy * 0.01);
    return float4(value, value, value, 1.0);
}

technique Terrain
{
    pass P0
    {
        PixelShader = compile ps_3_0 MainPS();
    }
};