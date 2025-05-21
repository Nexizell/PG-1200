Shader "Custom/SpeedRunShader2" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo2 (RGB)", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,1)
		_Color2 ("Color2", Vector) = (1,1,1,1)
		_Shadow ("GlowPower", Range(0, 1)) = 1
		_MaxDIstance ("EndDistance", Float) = 100
	}
	SubShader
	{
		LOD 100
		Tags { "Queue" = "Geometry+200" "RenderType" = "Opaque" }

		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			ZClip Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _MainTex2;

			float4 _Color;
			float4 _Color2;
			float _Shadow;
			float _MaxDIstance;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 blendColor : COLOR;
				float4 pos : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;

				float3 worldNormal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
				float3 viewPos = mul(UNITY_MATRIX_MV, v.vertex).xyz;

				float dist = length(viewPos);
				float ffade = 1.0 - saturate(max(dist - _MaxDIstance, 0.0) * 0.01);
				ffade *= ffade;

				float diff = max(0.5, dot(worldNormal, float3(0.0, 1.5, 0.0)));

				float4 blendCol = float4(diff, diff, diff, v.color.a) * ffade;

				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.blendColor = blendCol;

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 tex2 = tex2D(_MainTex2, i.uv) * _Color * _Shadow;
				fixed4 tex1 = tex2D(_MainTex, i.uv) * _Color2 * i.blendColor;

				fixed4 result;
				result.rgb = tex1.rgb + tex2.rgb * tex2.a * 1.2;
				result.a = tex1.a;

				return result;
			}
			ENDCG
		}
	}
}