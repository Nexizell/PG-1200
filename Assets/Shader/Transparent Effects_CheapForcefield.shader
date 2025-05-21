Shader "Transparent Effects/CheapForcefield" {
	Properties {
		_Color ("_Color", Vector) = (0,1,0,1)
		_Inside ("_Inside", Range(0, 0.2)) = 0
		_Rim ("_Rim", Range(1, 2)) = 1.2
		_Texture ("_Texture", 2D) = "white" {}
		_Tile ("_Tile", Range(1, 10)) = 5
		_Strength ("_Strength", Range(0, 5)) = 1.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}