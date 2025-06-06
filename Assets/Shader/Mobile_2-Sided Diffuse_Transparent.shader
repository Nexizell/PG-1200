Shader "Mobile/2-Sided Diffuse_Transparent" {
    Properties {
        _Color ("Main Color", Vector) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D) = "" {}
        _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
    }

    SubShader {
        Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
		Cull Off
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert alphatest:_Cutoff

        sampler2D _MainTex;
        fixed4 _Color;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            //if (c.a < _Cutoff)
            //    discard;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }

    Fallback "Legacy Shaders/Transparent/VertexLit"
}
