Shader "Hidden/CopyNormal"
{
    Properties
    {
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "LightMode" = "UniversalGBuffer"
            "UniversalMaterialType" = "Lit"
        }
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/Utils/Deferred.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            struct attributes
            {
                float4 position : POSITION;
                half3 normal : NORMAL;
            };

            struct varyings
            {
                float4 position : SV_POSITION;
                half3 normal : TEXCOORD0;
            };

            #define GBUFFER0 0
            
            FRAMEBUFFER_INPUT_HALF(GBUFFER0);
 
            varyings vert(attributes attr)
            {
                varyings var;
                var.position = TransformObjectToHClip(attr.position.xyz);
                var.normal = attr.normal * 0.5 + 0.5;
                return var;
            }

            half4 frag(varyings var) : SV_Target
            {
                half4 gbuffer0 = LOAD_FRAMEBUFFER_INPUT(GBUFFER0, var.position.xyz);
                return gbuffer0;
            }
            ENDHLSL
        }
    }
}
