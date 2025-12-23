// Fixed version that preserves original texture colors
#include "UnityCG.cginc"

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
	// Removed: float4 color : COLOR;  <- This was causing the problem!
};

struct v2f
{
    float2 uv : TEXCOORD0;
	UNITY_FOG_COORDS(1)
    float4 vertex : SV_POSITION;
	// Removed: float4 color : TEXCOORD2;  <- Don't need this anymore!
};

sampler2D _MainTex;
float4 _MainTex_ST;
float _CurveStrength;

v2f vert(appdata v)
{
    v2f o;
    float _Horizon = 100.0f;
    float _FadeDist = 50.0f;
	
    o.vertex = UnityObjectToClipPos(v.vertex);
    float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(o.vertex.z);
    o.vertex.y -= _CurveStrength * dist * dist * _ProjectionParams.x;
    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
	
	// Removed: o.color = v.color;  <- This line was passing the problematic color!
	
    UNITY_TRANSFER_FOG(o, o.vertex);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
	// Sample texture WITHOUT multiplying by vertex color
	// This preserves your original texture colors perfectly!
    fixed4 col = tex2D(_MainTex, i.uv);
	
	// Apply fog
    UNITY_APPLY_FOG(i.fogCoord, col);
	
    return col;
}