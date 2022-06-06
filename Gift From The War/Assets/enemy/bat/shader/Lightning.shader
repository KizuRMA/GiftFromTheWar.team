Shader "Custom/Lightning"
{
    Properties
    {
        _MainTex("MainTex", 2D) = "white" {}
    }
        SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull off
        LOD 100

        Pass{
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0



            struct appdata {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 custom1 : TEXCOORD1;
                fixed4 custom2 : TEXCOORD2;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                float4 custom1 : TEXCOORD1;
                float4 custom2 : TEXCOORD2;

            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                o.custom1 = v.custom1;
                o.custom2 = v.custom2;
                return o;
            }

            fixed4 frag(v2f i) : SV_TARGET{
                i.uv.y = i.uv.y + _Time.y * i.custom1.y;
                fixed4 c = tex2D(_MainTex, i.uv) * i.color * i.custom2 * i.custom1.w;
                return c;
            }
            ENDCG
        }
    }
        FallBack "Diffuse"
}