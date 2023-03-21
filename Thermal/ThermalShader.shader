

Shader "Custom/ThermalShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _ThermalGradient ("Thermal Gradient Texture", 2D) = "white" {}
        _Temperature("Temperature", Float) = 0.0
        _DiffusionFactor("Diffusion Factor",Float) = 0.0
        _Sensitivity("Sensitivity (max temperature differerence)",Float) = 30.0
        _Offset("Heat Generating Point Offset",Vector) = (0.0,0.0,0.0,0.0)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            half _Glossiness;
            half _Metallic;
            fixed4 _Color;

            sampler2D _ThermalGradient;
            float4 _ThermalGradient_ST;
            float _Temperature;
            float _DiffusionFactor;
            float _Sensitivity;
            float4 _Offset;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float temperature : WEIGHT;
                float depth : DEPTH;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            // Temperature of point
            float TempOfPoint(float4 pt)
            {
                return _Temperature - distance(pt, _Offset) * _DiffusionFactor;
            }

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.temperature = TempOfPoint(v.vertex);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = length(ObjSpaceViewDir(v.vertex))  * _ProjectionParams.w;
                return o;
            }


            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float invert_depth = 0.5 + 0.5 * (1 - (i.depth));
                float tint = clamp(i.temperature / _Sensitivity,0.05,1.0);
                fixed4 col = tex2D(_ThermalGradient, float2(tint,tint)) * invert_depth;
                UNITY_OPAQUE_ALPHA(col.a);
                return col;
            }
            ENDCG
        }
    }
}
