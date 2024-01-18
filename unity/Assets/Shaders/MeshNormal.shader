Shader "Custom/MeshNormal"
{
    Properties
    { }

    SubShader
    {
        // SubShader Tags 定义何时以及在何种条件下执行某个 SubShader 代码块
        // 或某个通道。
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

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

            varyings vert(attributes attr)
            {
                varyings var;
                var.position = TransformObjectToHClip(attr.position.xyz);
                var.normal = attr.normal * 0.5 + 0.5;
                return var;
            }

            half4 frag(varyings var) : SV_Target
            {
                return half4(var.normal, 0.0);
            }
            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
