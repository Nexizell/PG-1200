Shader "TestBed/Dissolve/Dissolve Unlit" {
	Properties {
		_Burn ("Burn Amount", Range(-0.25, 1.25)) = 0
		_LineWidth ("Burn Line Size", Range(0, 0.2)) = 0.1
		_BurnColor ("Burn Color", Vector) = (1,0,0,1)
		_MainTex ("Main Texture", 2D) = "white" {}
		_BurnMap ("Burn Map", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "VertexLit"
}