Shader "Mobile/Diffuse-Color" {
	Properties {
		_ColorRili ("Rili Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}
	
	SubShader {
		Name "FORWARD"
		Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass {
			ZWrite On
			ColorMask 0
		}

		CGPROGRAM
		#pragma surface surf Lambert alpha:fade
	
		sampler2D _MainTex;
		fixed4 _ColorRili;
	
		struct Input {
			float2 uv_MainTex;
		};
	
		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _ColorRili;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	
	Fallback "Mobile/VertexLit"
	}