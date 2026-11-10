#ifndef LUCES_INCLUDED
#define LUCES_INCLUDED

void MainLight_float(out float3 Direction, out float3 Color)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = float3(0.5, 0.5, 0);
    Color = 1;
#else
    Light l = GetMainLight();
    Direction = l.direction;
    Color = l.color;
#endif
}

void FarosLuz_float(float3 WorldPos, float3 WorldNormal, out float3 Diffuse)
{
    Diffuse = 0;
#ifndef SHADERGRAPH_PREVIEW
    int n = GetAdditionalLightsCount();
    for (int i = 0; i < n; i++)
    {
        Light l = GetAdditionalLight(i, WorldPos);
        Diffuse += saturate(dot(WorldNormal, l.direction)) * l.color * l.distanceAttenuation;
    }
#endif
}

#endif
