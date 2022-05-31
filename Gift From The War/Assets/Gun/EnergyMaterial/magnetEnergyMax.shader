Shader "Custom/magnetEnergyMax"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		[HDR] _EmissionColor("Emission", Color) = (0, 0, 0, 0)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Geometry+2"}

		Stencil
	{
		Ref 1
		Comp Equal
	}

		ColorMask RGB
		Cull Front
		ZTest Always

		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float3 _EmissionColor;

		UNITY_INSTANCING_BUFFER_START(Props)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = _Color;
			c.rgb += _EmissionColor;
			o.Albedo = c.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
		FallBack "Diffuse"
}
