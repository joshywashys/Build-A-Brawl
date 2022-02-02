#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
    #if (SHADERPASS != SHADERPASS_FORWARD)
        #undef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
    #endif
#endif

struct CustomLightingData {
    float3 positionWS;
    float3 normalWS;
    float3 viewDirectionWS;
    float4 shadowCoord;

    float3 albedo;
    float smoothness;
};

float GetSmoothnessPower(float smoothness) {
    return exp2(10 * smoothness + 1);
}

#ifndef SHADERGRAPH_PREVIEW
float3 CustomLightHandling(CustomLightingData d, Light light) {
    
    float3 radiance = light.color * (light.distanceAttenuation * light.shadowAttenuation);

    float diffuse = saturate(dot(d.normalWS, light.direction));
    float specularDot = saturate(dot(d.normalWS, normalize(light.direction + d.viewDirectionWS)));
    float specular = pow(specularDot, GetSmoothnessPower(d.smoothness)) * diffuse;
    
    float3 colour = d.albedo * radiance * (diffuse + specular);
    return colour;
}
#endif

float3 CalculateCustomLighting(CustomLightingData d) {
#ifdef SHADERGRAPH_PREVIEW

    float3 lightDir = float3(0.5, 0.5, 0);
    float intensity = saturate(dot(d.normalWS, lightDir)) + 
        pow(saturate(dot(d.normalWS, normalize(lightDir + d.viewDirectionWS))), GetSmoothnessPower(d.smoothness));
    return d.albedo * intensity;

#else

    Light mainLight = GetMainLight(d.shadowCoord, d.positionWS, 1);
    
    float3 colour = 0;
    colour += CustomLightHandling(d, mainLight);
    
    #ifdef _ADDITIONAL_LIGHTS
        uint numAdditionalLights = GetAdditionalLightsCount();
        for (uint i = 0; i < numAdditionalLights; i++) {
            Light light = GetAdditionalLight(i, d.positionWS, 1);
            colour += CustomLightHandling(d, light);
        }
    #endif

    return colour;

#endif
}

void CalculateCustomLighting_float(float3 Position, float3 Normal, float3 ViewDirection, 
    float3 Albedo, float Smoothness, 
    out float3 Colour) {

    CustomLightingData d;
    d.positionWS = Position;
    d.normalWS = Normal;
    d.viewDirectionWS = ViewDirection;
    d.albedo = Albedo;
    d.smoothness = Smoothness;

    Colour = CalculateCustomLighting(d);
}

// Unity Wrapper Function to get main light data from the scene
void GetMainLight_float(out float3 Colour, 
                        out float3 Direction, 
                        out float DistanceAttenuation, 
                        out float ShadowAttenuation)
{
    #ifdef SHADERGRAPH_PREVIEW

        Colour = float3(1, 1, 1);
        Direction = float3(0.5, 0.5, 0);
        DistanceAttenuation = 1;
        ShadowAttenuation = 1;

    #else

        Light light = GetMainLight();
        Colour = light.color;
        Direction = light.direction;
		DistanceAttenuation = light.distanceAttenuation;
        ShadowAttenuation = light.shadowAttenuation;

    #endif
}

void CalculateSecular_float(float4 Specular, float Smoothness, float Direction, float Colour, out float3 Out)
{
	
}

#endif