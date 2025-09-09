Shader "Custom/Test2"
{
    Properties
    {
        _MainTex ("Source", 2D) = "white" {}
        _A ("Line A (0-1)", Range(0,1)) = 0.3
        _B ("Line B (0-1)", Range(0,1)) = 0.7
    }
    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _A;
            float _B;
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            float3 ToGray(float3 col)
            {
                float g = dot(col, float3(0.299, 0.587, 0.114));
                return float3(g, g, g);
            }
            
            float3 SwapRGB(float3 col)
            {
                return float3(col.g, col.b, col.r);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 col = tex2D(_MainTex, i.uv).rgb;
                float x = i.uv.x;

                float3 grayCol = ToGray(col);
                float3 swapCol = SwapRGB(col);

                float3 result;
                
                if (x < _A)
                {
                    result = grayCol;
                }
                else if (x > _B)
                {
                    result = swapCol;
                }
                else
                {
                    float t = smoothstep(_A, _B, x);
                    result = lerp(grayCol, swapCol, t);
                }

                return float4(result, 1.0);
            }
            ENDCG
        }
    }
}
