Shader "Custom/monoTone" {
    Properties{
        _MainTex("MainTex", 2D) = ""{}
    }

        SubShader{
            Pass {
                CGPROGRAM

                #include "UnityCG.cginc"

                #pragma vertex vert_img
                #pragma fragment frag

                sampler2D _MainTex;

                fixed4 frag(v2f_img i) : COLOR {
                    fixed4 c = tex2D(_MainTex, i.uv);
                    float gray = c.r * 0.3 + c.g * 0.6 + c.b * 0.1;
                    c.rgb = fixed3(gray * abs(sin(_Time.x * 10.0f)),gray,gray * 0.1);
                    return c;
                }

                ENDCG
            }
    }
}