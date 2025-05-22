Shader "Unlit/Transparent Colored SDF Outline" 
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
            GpuProgramID 4064
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _Color;
            float _Base;
            float _Smooth;
            float _Outline;
            float xlat_mutable_Outline;
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
                float4 tmpvar_1;
                tmpvar_1.w = 1.0;
                tmpvar_1.xyz = v.vertex.xyz;
                o.vertex = UnityObjectToClipPos(float4(v.vertex.xyz, 1.0));
                o.texcoord0 = v.texcoord0.xy;
                o.color = v.color;
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                xlat_mutable_Outline = (_Base + ((768.0 / _ScreenParams.y) * _Outline));
                float4 tmpvar_1;
                tmpvar_1 = tex2D (_MainTex, i.texcoord0);
                float edge0_2;
                edge0_2 = (_Base - _Smooth);
                float tmpvar_3;
                tmpvar_3 = clamp (((tmpvar_1.w - edge0_2) / (
                (_Base + _Smooth)
                - edge0_2)), 0.0, 1.0);
                float edge0_4;
                edge0_4 = (xlat_mutable_Outline - _Smooth);
                float tmpvar_5;
                tmpvar_5 = clamp (((tmpvar_1.w - edge0_4) / (
                (xlat_mutable_Outline + _Smooth)
                - edge0_4)), 0.0, 1.0);
                float4 tmpvar_6;
                tmpvar_6.xyz = _Color.xyz;
                tmpvar_6.w = (((tmpvar_3 * 
                (tmpvar_3 * (3.0 - (2.0 * tmpvar_3)))
                ) * float(
                (_Base < xlat_mutable_Outline)
                )) - (tmpvar_5 * (tmpvar_5 * 
                (3.0 - (2.0 * tmpvar_5))
                )));
                float4 tmpvar_7;
                tmpvar_7 = (tmpvar_6 * i.color);
                return tmpvar_7;
            }
            ENDCG
        }
    }
    
}