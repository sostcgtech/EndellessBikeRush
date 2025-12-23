Shader "Unlit/CurvedUnlitFixed"
{ 
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CurveStrength ("Curve Strength", Float) = 0.001
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
			#pragma multi_compile_fog
				
			// Use the fixed include file
			#include "CurvedCodeFixed.cginc"
			ENDCG
		}
	}
}