Shader "UI/jvk"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _GreenThreshold ("GreenThreshold", Float) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        Pass
        {
            Name "UI Pass"
            Tags { "LightMode" = "UniversalForward" }

            // Disable culling and depth writing for UI rendering
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile __ UNITY_UI_CLIP_RECT

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Shader properties
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _GreenThreshold;

            // Vertex structure
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            // Output structure
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                #ifdef UNITY_UI_CLIP_RECT
                float2 worldPosition : TEXCOORD1;
                #endif
            };

            // Vertex shader
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float4 posWS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionHCS = posWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color * _Color;
                return OUT;
            }

            // Fragment shader
            half4 frag(Varyings IN) : SV_Target
            {
                // Sample the texture using explicit sampler
                half4 col = tex2D(_MainTex, IN.uv) * IN.color;
                // Green screen keying: fade green pixels to transparent
                float greenAmount = col.g;
                // Use step function to check if the green channel exceeds threshold
                float mask = step(_GreenThreshold, greenAmount); 
                col.a *= mask;

                return col;
            }
            ENDHLSL
        }
    }
    Fallback "UI/Default"
}
