Shader "Universal Render Pipeline/CurvedUnlit_Debug"
{
    Properties
    {
        [MainTexture] _BaseMap("Texture", 2D) = "white" {}
        _CurveStrength("Curve Strength", Float) = 0.001
        [Toggle] _ShowUVs("Show UVs (Debug)", Float) = 0
        [Toggle] _DisableCurve("Disable Curve (Test)", Float) = 0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;  // Keep this to check if vertex colors exist
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 vertexColor : TEXCOORD1;
                float fogFactor : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float _CurveStrength;
                float _ShowUVs;
                float _DisableCurve;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;

                // Transform to clip space
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);

                // Apply curve effect (can be disabled for testing)
                if (_DisableCurve < 0.5)
                {
                    float dist = output.positionCS.z;
                    output.positionCS.y -= _CurveStrength * dist * dist * _ProjectionParams.x;
                }

                // Transform UVs - this should preserve texture coordinates
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                // Pass through vertex color for debugging
                output.vertexColor = input.color;

                // Fog
                output.fogFactor = ComputeFogFactor(output.positionCS.z);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half4 color;

                // Debug mode - show UVs as colors
                if (_ShowUVs > 0.5)
                {
                    // Red = U coordinate, Green = V coordinate
                    color = half4(input.uv.x, input.uv.y, 0, 1);
                }
                else
                {
                    // Normal mode - sample texture
                    color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.uv);
                    
                    // DO NOT multiply by vertex color - this destroys textures!
                    // color *= input.vertexColor;  // <- NEVER do this!
                }

                // Apply fog
                color.rgb = MixFog(color.rgb, input.fogFactor);

                return color;
            }
            ENDHLSL
        }
    }

    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}