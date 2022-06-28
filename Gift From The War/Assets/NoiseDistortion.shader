Shader "Custom/NoiseDistortion"
{
    Properties
    {
        _MainTex("MainTexture", 2D) = "white"{}
        _Range("Range",Range(0,0.05)) = 0.05
    }

        SubShader
    {
        Cull Off

        Tags
        {
            "Queue" = "AlphaTest"
            "RenderType" = "AlphaTest"
        }

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD;
            };

            sampler2D _MainTex;

            uniform float _Range;

            float _Border;
            float4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed2 samplePoint = i.uv;
                fixed4 Tex = tex2D(_MainTex, samplePoint);
                float sinv = sin(i.uv.y * 1 + (_Time.x * 100.0) * -0.1);
                float steped = step(1, sinv * sinv * sinv);
                Tex.rgb -= (1 - steped) * abs(sin(i.uv.y * 10.0 + (_Time.x * 100.0) * 2.0)) * _Range;
                Tex.rgb -= (1 - steped) * abs(sin(i.uv.y * 7.0 - (_Time.x * 100.0) * 2.0)) * _Range;
                //Tex.rgb += steped * 0.1;
                return Tex;
            }

            ENDCG
        }
    }
}