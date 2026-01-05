Shader "Hidden/BlurShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        
        _blurSize ("blur size", Float) = 8
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

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

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // x = 1/width, y = 1/height

            float _blurSize;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = v.uv;
                return o;
            }

            //gausian blur algorithm converted to hl. original: https://www.shadertoy.com/view/Xltfzj
            //gpt used to help convert

            float4 frag(v2f i) : SV_Target
            {
                float pi = 6.28318530718; // 2*PI

                // Settings
                float directions = 16.0;
                float quality = 6.0;

                float2 radius = _blurSize * _MainTex_TexelSize.xy;
                float4 col = float4(0, 0, 0, 0);

                for (float d = 0.0; d < pi; d += pi / directions)
                {
                    for (float q = 0.0; q <= 1.0; q += 1.0 / quality)
                    {
                        float2 offset = float2(cos(d), sin(d)) * radius * q;
                        col += tex2D(_MainTex, i.uv + offset);
                    }
                }

                col /= (quality + 1.0) * directions;
                return saturate(col * 1.35);
            }
            ENDHLSL
        }
    }
}
