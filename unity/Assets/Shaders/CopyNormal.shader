Shader "Hidden/CopyNormal"
{
    Properties
    {
    }
    SubShader
    {
        Tags {}
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_GBuffer2); SAMPLER(sampler_GBuffer2);
            
            struct attributes
            {
                float4 position : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct varyings
            {
                float4 position : SV_POSITION;
                half2 uv : TEXCOORD1;
            };

            varyings vert(attributes attr)
            {
                varyings var;
                var.position = TransformObjectToHClip(attr.position.xyz);
                var.uv = attr.texcoord;
                return var;
            }

            half4 frag(varyings var) : SV_Target
            {
                return SAMPLE_TEXTURE2D(_GBuffer2, sampler_GBuffer2, var.uv) * 0.5 + 0.5;
            }
            ENDHLSL
        }
    }
}
