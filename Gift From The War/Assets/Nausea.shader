Shader "PostEffect/Nausea"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Amp("Amp", Float) = 0.1
        _T("T", Float) = 0.25
    }
        SubShader
        {
            // No culling or depth
            Cull Off ZWrite Off ZTest Always

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                sampler2D _MainTex;
                float _Amp;
                float _T;

                fixed4 frag(v2f i) : SV_Target
                {
                    float2 uv = i.uv;
                    uv.x += sin((i.uv.y + _Time.y) * 3.14 / _T) * _Amp;
                    uv.y += sin((i.uv.x + _Time.y) * 3.14 / _T) * _Amp;
                    fixed4 col = tex2D(_MainTex, uv);
                    return col;
                }
                ENDCG
            }
        }
}