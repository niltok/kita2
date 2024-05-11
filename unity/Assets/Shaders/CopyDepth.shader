Shader "Hidden/CopyDepth"
{
    Properties
    {
    }
    SubShader
    {
        Tags {}
        Cull Off ZWrite Off ZTest Always

//        Pass
//        {
//            HLSLPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//
//            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
//
//            TEXTURE2D(_CameraDepthTexture); SAMPLER(sampler_CameraDepthTexture);
//            
//            struct attributes
//            {
//                float4 position : POSITION;
//                float2 texcoord : TEXCOORD0;
//            };
//
//            struct varyings
//            {
//                float4 position : SV_POSITION;
//                half2 uv : TEXCOORD1;
//            };
//
//            varyings vert(attributes attr)
//            {
//                varyings var;
//                var.position = TransformObjectToHClip(attr.position.xyz);
//                var.uv = attr.texcoord;
//                return var;
//            }
//
//            half4 frag(varyings var) : SV_Target
//            {
//                return SAMPLE_TEXTURE2D(_CameraDepthTexture, sampler_CameraDepthTexture, var.uv);
//            }
//
//            inline float Linear01Depth( float z )
//            {
//                return 1.0 / (_ZBufferParams.x * z + _ZBufferParams.y);
//            }
//            ENDHLSL
//        }
        Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"UnityCG.cginc"

			sampler2D _CameraDepthTexture;

			struct VertexData
			{
				float4 pos:POSITION;
				float2 uv:TEXCOORD0;
			};

			struct V2F
			{
				float4 pos:POSITION;
				float2 uv : TEXCOORD0;
			};

			float4 _MainTex_TexelSize;

			V2F vert(VertexData v)
			{
				V2F res;
				res.pos = UnityObjectToClipPos(v.pos);
				res.uv = v.uv;
				// if (_MainTex_TexelSize.y < 0)
				// 	res.uv.y = 1 - res.uv.y;
				return res;
			}

			float4 _Color;

			fixed4 frag(V2F v) :SV_Target
			{
				float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture,  v.uv);
				depth = 1 - Linear01Depth(depth);
				return float4(depth, depth, depth, 1);
			}
			ENDCG
		}
    }
}
