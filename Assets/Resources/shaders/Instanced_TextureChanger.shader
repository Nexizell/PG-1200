Shader "Instanced/TextureChanger" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 150
	
	CGPROGRAM
	#pragma surface surf Lambert noforwardadd
	
	sampler2D _MainTex;
	
	struct Input {
		float2 uv_MainTex;
		float4 color : COLOR;
	};
	
	void surf (Input IN, inout SurfaceOutput o) {
		float4 vc = IN.color;
		float2 uv = IN.uv_MainTex;
		float2 o_uv = uv;
		if ((vc.r < 0.1)) {
			if (abs((floor(uv.x) - uv.x)) > 0.5) {
			  o_uv.x += 0.5;
			};
			if (abs((floor(uv.y) - uv.y)) > 0.5) {
			  o_uv.y += 0.5;
			};
		}
		else if ((vc.r < 0.3)) {
			if (abs((floor(uv.x) - uv.x)) < 0.5) {
				o_uv.x += 0.5;
			};
			  if (abs((floor(uv.y) - uv.y)) > 0.5) {
				o_uv.y += 0.5;
			};
		}
		else if ((vc.r < 0.6)) {
			if (abs((floor(uv.x) - uv.x)) > 0.5) {
				o_uv.x += 0.5;
			};
			  if (abs((floor(uv.y) - uv.y)) < 0.5) {
				o_uv.y += 0.5;
			};
		}
		else {
			if (abs((floor(uv.x) - uv.x)) < 0.5) {
				o_uv.x += 0.5;
			};
			  if (abs((floor(uv.y) - uv.y)) < 0.5) {
				o_uv.y += 0.5;
			};
		};
		fixed4 c = tex2D(_MainTex, o_uv);
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
	}
	
	Fallback "Mobile/VertexLit"
	}