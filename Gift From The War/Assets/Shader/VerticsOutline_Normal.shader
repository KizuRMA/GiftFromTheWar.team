Shader "ModelEffect/VerticsOutline_Normal"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_OutlineFactor("Outline Factor", Range(0, 1)) = 0.5
		_OutlineWidth("Outline Width", Range(0, 1)) = 0.002
		_BodyAlpha("Body Alpha", Range(0, 1)) = 1

		_Stencil("Stencil ID", Int) = 16

		[HideInInspector] _StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector] _StencilReadMask("Stencil Read Mask", Float) = 255
	}

		SubShader{
			Tags {
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
			}
			LOD 200

			Pass {
				Name "VerticsOutline_Body"

				Cull Back
				ZWrite On
				Blend SrcAlpha OneMinusSrcAlpha
				ColorMask RGBA

				Stencil {
					Ref[_Stencil]
					Comp Always
					Pass Replace
					ZFail Replace
					ReadMask[_StencilReadMask]
					WriteMask[_StencilWriteMask]
				}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.5
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float2 texcoord : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					UNITY_VERTEX_OUTPUT_STEREO
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				uniform float _BodyAlpha;

				v2f vert(appdata_t v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

					o.vertex = UnityObjectToClipPos(v.vertex);

					o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.texcoord);
					UNITY_APPLY_FOG(i.fogCoord, col);
					UNITY_OPAQUE_ALPHA(col.a);

					return fixed4(col.rgb, _BodyAlpha);
				}

				ENDCG
			}

			Pass {
				Name "VerticsOutline_Outline_Face1"

				Cull Front
				ZWrite On
					//ZTest Always
					ColorMask RGBA

					Stencil {
						Ref[_Stencil]
						Comp NotEqual
						Pass Keep
						ZFail Keep
						ReadMask[_StencilReadMask]
						WriteMask[_StencilWriteMask]
					}

					CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 3.5
					#pragma multi_compile_fog

					#include "UnityCG.cginc"

					struct appdata_t {
						float4 vertex : POSITION;
						float2 texcoord : TEXCOORD0;
						float3 normal : NORMAL;
						float3 tangent : TANGENT;
						UNITY_VERTEX_INPUT_INSTANCE_ID
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						float2 texcoord : TEXCOORD0;
						UNITY_FOG_COORDS(1)
						UNITY_VERTEX_OUTPUT_STEREO
					};

					sampler2D _MainTex;
					float4 _MainTex_ST;
					uniform fixed4 _OutlineColor;
					uniform float _OutlineWidth;

					float4 vert(appdata_base v) : SV_POSITION{
					  v2f o;
					  o.vertex = UnityObjectToClipPos(v.vertex);
					  float3 normal = mul((float3x3) UNITY_MATRIX_MV, v.normal);
					  normal.x *= UNITY_MATRIX_P[0][0];
					  normal.y *= UNITY_MATRIX_P[1][1];
					  o.vertex.xy += normal.xy * _OutlineWidth;
					  return o.vertex;
					}

					fixed4 frag(v2f i) : SV_Target
					{
						fixed4 col = _OutlineColor;

						/*UNITY_APPLY_FOG(i.fogCoord, col);
						UNITY_OPAQUE_ALPHA(col.a);*/
						return col;
					}

					ENDCG
				}
		}
}