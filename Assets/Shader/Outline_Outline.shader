// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit shader. Simplest possible textured shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Outline/Outline" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "black" {}
	_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
	_Outline ("Outline Size", Float) = 0.01
}

SubShader {
    Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
    Blend SrcAlpha OneMinusSrcAlpha
    LOD 100
	Cull Front

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
				float4 normal : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _OutlineColor;
			float _Outline;
            
            float4 vert (float4 position:POSITION, float3 normal:NORMAL) : SV_POSITION
            {
				position.xyz += normal * _Outline/32.f;

                return UnityObjectToClipPos(position);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
        ENDCG
    }
}

}
