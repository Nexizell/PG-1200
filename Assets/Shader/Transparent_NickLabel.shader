// Upgrade NOTE: replaced 'glstate_matrix_mvp' with 'UNITY_MATRIX_MVP'

Shader "Transparent/NickLabel" 
{
    Properties
    {
        _Color ("Color", Vector) = (1,1,1,1)
        _Alpha ("Alpha (A)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "QUEUE" = "Transparent+20" "RenderType" = "Transparent" }
        Pass
        {
            Tags { "QUEUE" = "Transparent+20" "RenderType" = "Transparent" }
            Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
            ColorMask RGB -1
            ZClip Off
            ZWrite Off
            Cull Off
            Fog {
            Mode Off
            }
            GpuProgramID 3242
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float4 _Alpha_ST;
            sampler2D _Alpha;
            float4 _Color;
            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float4 texcoord0 : TEXCOORD0;
            };
            struct v2f
            {
                float4 color0 : COLOR0;
                float2 texcoord0 : TEXCOORD0;
                float4 vertex : POSITION;
            };
            v2f vert(appdata_t v)
            {
                v2f o;
                float4 tmpvar_1;
                float4 tmpvar_2;
                tmpvar_2 = clamp (v.color, 0.0, 1.0);
                tmpvar_1 = tmpvar_2;
                float4 tmpvar_3;
                tmpvar_3.w = 1.0;
                tmpvar_3.xyz = v.vertex.xyz;
                o.color0 = tmpvar_1;
                o.texcoord0 = ((v.texcoord0.xy * _Alpha_ST.xy) + _Alpha_ST.zw);
                o.vertex = UnityObjectToClipPos(tmpvar_3);
                return o;
            }
            float4 frag(v2f i) : SV_TARGET
            {
                float4 col_1;
                col_1.xyz = (i.color0 * _Color).xyz;
                col_1.w = (tex2D (_Alpha, i.texcoord0).w * _Color.w);
                return col_1;
            }
            ENDCG
        }
    }
    
}