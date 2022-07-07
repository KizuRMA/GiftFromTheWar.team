Shader "Custom/HeelShader"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        Pass
        {
            CGPROGRAM

            //#pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD;
            };

            uniform sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            uniform float _distanceFactor = 60.0;
            uniform float _timeFactor = -30;
            uniform float _totalFactor = 1;
            uniform float _waveWidth = 0.05;
            uniform float _curWaveDis = 0.33;
            uniform float4 _startPos;

            fixed4 frag(v2f i) : SV_Target
            {
                //Ž²‹t“]–â‘è‚ð‰ðŒˆ
                #if UNITY_UV_STARTS_AT_TOP
                if (_MainTex_TexelSize.y < 0)
                    _startPos.y = 1 - _startPos.y;
                #endif

                float2 dv = _startPos.xy - i.uv;
                dv = dv * float2(_ScreenParams.x / _ScreenParams.y, 1);
                float dis = sqrt(dv.x * dv.x + dv.y * dv.y);
                float sinFactor = sin(dis * _distanceFactor + _Time.y * _timeFactor) * _totalFactor * 0.01;
                float discardFactor = clamp(_waveWidth - abs(_curWaveDis - dis), 0, 1) / _waveWidth;
                float2 dv1 = normalize(dv);
                float2 offset = dv1 * sinFactor * discardFactor;
                float2 uv = offset + i.uv;
                fixed4 Tex = tex2D(_MainTex, uv);

                return Tex;
            }

            ENDCG
        }
    }
}
