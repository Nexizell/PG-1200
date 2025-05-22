// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "Unlit/Transparent Colored SDF" 
{
    Properties
    {
        _MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
        _Color ("Color", Vector) = (1,1,1,1)
        _Base ("InRange", Range(0, 1)) = 0.6
        _Smooth ("Smooth", Range(0, 1)) = 0.5
        _Outline ("OutRange", Range(0, 1)) = 0.6
    }
    SubShader
    {
        LOD 200
        Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
        Pass
        {
            LOD 200
            Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
            ZClip Off
            ZWrite Off
            Cull Off
            Fog {
            Mode Off
            }
            GpuProgramID 40295
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _Color;
            float _Base;
            float _Smooth;
            float _Outline;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float2 texcoord0 : TEXCOORD0;
                float4 color : COLOR;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(float4(v.vertex.xyz, 1.0));
                o.texcoord0 = v.texcoord0.xy;
                o.color = v.color;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                float4 tmpvar_1;
                float xlat_vardistance_2;
                float tmpvar_3;
                tmpvar_3 = tex2D (_MainTex, i.texcoord0).w;
                xlat_vardistance_2 = tmpvar_3;
                float edge0_4;
                edge0_4 = (_Base - _Smooth);
                float tmpvar_5;
                tmpvar_5 = clamp (((xlat_vardistance_2 - edge0_4) / (
                (_Base + _Smooth)
                - edge0_4)), 0.0, 1.0);
                float edge0_6;
                edge0_6 = (_Outline - _Smooth);
                float tmpvar_7;
                tmpvar_7 = clamp (((xlat_vardistance_2 - edge0_6) / (
                (_Outline + _Smooth)
                - edge0_6)), 0.0, 1.0);
                float4 tmpvar_8;
                tmpvar_8.xyz = _Color.xyz;
                tmpvar_8.w = ((tmpvar_7 * (tmpvar_7 * 
                (3.0 - (2.0 * tmpvar_7))
                )) - ((tmpvar_5 * 
                (tmpvar_5 * (3.0 - (2.0 * tmpvar_5)))
                ) * float(
                (_Base > _Outline)
                )));
                float4 tmpvar_9;
                tmpvar_9 = (tmpvar_8 * i.color);
                tmpvar_1 = tmpvar_9;
                return tmpvar_1;
            }
            ENDCG
        }
    }
    
}