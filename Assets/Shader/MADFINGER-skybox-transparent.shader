Shader "MADFINGER/Environment/Skybox - transparent - no fog" {

    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Color ("HACK: temporary to fix lightmap bouncing light (will be fixed in RC1)", Color) = (1,1,1,1)
    }

    SubShader {
        Tags { "Queue"="Transparent-20" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100

        // Disable depth writing and set blending mode
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _Color;

            // Vertex shader
            v2f vert (appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment shader
            fixed4 frag (v2f i) : SV_Target {
                // Sample the texture and multiply by the color
                fixed4 texColor = tex2D(_MainTex, i.uv);
                return texColor * _Color;
            }
            ENDCG
        }
    }

    FallBack Off
}
