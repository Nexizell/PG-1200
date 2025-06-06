Shader "Custom/SpeedRunSky" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex3 ("Albedo3 (RGB)", 2D) = "black" {}
		_Color ("Color", Vector) = (1,1,1,1)
		_Scroll ("Scroll", Range(0, 1)) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}