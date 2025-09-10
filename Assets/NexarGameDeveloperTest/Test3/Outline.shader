Shader "Custom/Outline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width (world units)", Range(0,0.05)) = 0.01
        _DistanceComp ("Distance Compensation (0..1)", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 100
        
        Pass
        {
            Name "Outline_Outer"
            Cull Front
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float  _OutlineWidth, _DistanceComp, _RefDistance, _ZOffset;

            struct appdata { float4 vertex:POSITION; float3 normal:NORMAL; };
            struct v2f { float4 pos:SV_POSITION; };

            v2f vert (appdata v)
            {
                v2f o;
                float3 nWS   = UnityObjectToWorldNormal(v.normal);
                float3 posWS = mul(unity_ObjectToWorld, v.vertex).xyz;

                float dist   = distance(_WorldSpaceCameraPos.xyz, posWS);
                float factor = lerp(1.0, max(dist / max(_RefDistance,1e-3),0.0), saturate(_DistanceComp));
                
                posWS += nWS * (_OutlineWidth * factor);
                
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - posWS);
                posWS += viewDir * (_ZOffset * _OutlineWidth);

                o.pos = UnityWorldToClipPos(posWS);
                return o;
            }

            fixed4 frag(v2f i):SV_Target { return _OutlineColor; }
            ENDCG
        }
        
        Pass
        {
            Name "Outline_Inner"
            Cull Back
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float  _OutlineWidth, _DistanceComp;

            struct appdata { float4 vertex:POSITION; float3 normal:NORMAL; };
            struct v2f { float4 pos:SV_POSITION; };

            v2f vert (appdata v)
            {
                v2f o;
                float3 nWS   = UnityObjectToWorldNormal(v.normal);
                float3 posWS = mul(unity_ObjectToWorld, v.vertex).xyz;

                float dist   = distance(_WorldSpaceCameraPos.xyz, posWS);
                float factor = lerp(1.0, max(dist ,0.0), saturate(_DistanceComp));
                
                posWS += (-nWS) * (_OutlineWidth * factor);

                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - posWS);
                posWS += viewDir * _OutlineWidth;

                o.pos = UnityWorldToClipPos(posWS);
                return o;
            }

            fixed4 frag(v2f i):SV_Target { return _OutlineColor; }
            ENDCG
        }
    }

    FallBack Off
}
