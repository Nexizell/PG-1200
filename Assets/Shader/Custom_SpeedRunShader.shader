Shader "Custom/SpeedRunShader" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo (RGB)", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,1)
		_Color2 ("Color2", Vector) = (1,1,1,1)
		_Shadow ("ShadowPowe", Range(0, 1)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 150
	
	CGPROGRAM
	#pragma surface surf Lambert noforwardadd
	
	sampler2D _MainTex, _MainTex2;
	
	struct Input {
		float2 uv_MainTex;
		float2 uv2_MainTex2;
		float3 worldNormal;
	};

	float4 _Color, _Color2;
	float _Shadow;
	
	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 c2 = tex2D(_MainTex2, IN.uv2_MainTex2) * _Color * _Shadow;
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color2 + (c2 * c2.w);
		float diff = max(0.05, dot(IN.worldNormal, _WorldSpaceLightPos0.xyz));
		o.Albedo = c.rgb + (c.rgb * _LightColor0 * (diff * 1.2));
		o.Alpha = c.a;
	}
	ENDCG
	}
	
	Fallback "Mobile/VertexLit"
	}