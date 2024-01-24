Shader "Custom/MeshNormal"
{
    Properties
    { }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "LightMode" = "UniversalGBuffer" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            struct g_buffers
            {
                half4 albedo : SV_Target0;
                half4 specular : SV_Target1;
                half4 normal : SV_Target2;
                half4 gi : SV_Target3;
                float depth : SV_Depth;
            };

            varyings vert(attributes attr)
            {
                varyings var;
                var.position = TransformObjectToHClip(attr.position.xyz);
                var.normal = attr.normal;
                return var;
            }

            g_buffers frag(varyings var) : SV_Target
            {
                g_buffers o;
                o.albedo = half4(0, 0, 0, 0);
                o.specular = half4(0, 0, 0, 0);
                o.normal = half4(var.normal, 0);
                o.gi = half4(var.normal * 0.5 + 0.5, 0);
                o.depth = var.position.z;
                return o;
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
