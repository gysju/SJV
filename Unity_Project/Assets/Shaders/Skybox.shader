Shader "Custom/Skybox" {
Properties {
	_Tint ("Tint Color", Color) = (.5, .5, .5, .5)
	[Gamma] _Exposure ("Exposure", Range(0, 8)) = 1.0
	_Rotation ("Rotation", Range(0, 360)) = 0

	[Header(Sky)]
	[NoScaleOffset] _FrontSkyTex ("Front [+Z]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _BackSkyTex ("Back [-Z]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _LeftSkyTex ("Left [+X]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _RightSkyTex ("Right [-X]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _UpSkyTex ("Up [+Y]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _DownSkyTex ("Down [-Y]   (HDR)", 2D) = "grey" {}

	[Header(Clouds)]
	[NoScaleOffset] _FrontCloudsTex ("Front [+Z]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _BackCloudsTex ("Back [-Z]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _LeftCloudsTex ("Left [+X]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _RightCloudsTex ("Right [-X]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _UpCloudsTex ("Up [+Y]   (HDR)", 2D) = "grey" {}
	[NoScaleOffset] _DownCloudsTex ("Down [-Y]   (HDR)", 2D) = "grey" {}
}

SubShader {
	Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
	Cull Off ZWrite Off
	
	CGINCLUDE
	#include "UnityCG.cginc"

	half4 _Tint;
	half _Exposure;
	float _Rotation;

	float3 RotateAroundYInDegrees (float3 vertex, float degrees)
	{
		float alpha = degrees * UNITY_PI / 180.0;
		float sina, cosa;
		sincos(alpha, sina, cosa);
		float2x2 m = float2x2(cosa, -sina, sina, cosa);
		return float3(mul(m, vertex.xz), vertex.y).xzy;
	}
	
	struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_INPUT_INSTANCE_ID
	};
	struct v2f {
		float4 vertex : SV_POSITION;
		float2 texcoord : TEXCOORD0;
		UNITY_VERTEX_OUTPUT_STEREO
	};
	v2f vert (appdata_t v)
	{
		v2f o;
		UNITY_SETUP_INSTANCE_ID(v);
		UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
		float3 rotated = RotateAroundYInDegrees(v.vertex, _Rotation);
		o.vertex = UnityObjectToClipPos(rotated);
		o.texcoord = v.texcoord;
		return o;
	}
	half4 skybox_frag (v2f i, sampler2D smp, half4 smpDecode)
	{
		half4 tex = tex2D (smp, i.texcoord);
		half3 c = DecodeHDR (tex, smpDecode);
		c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
		c *= _Exposure;
		return half4(c, 1);
	}

	half4 Cloud_frag (v2f i, sampler2D smp, half4 smpDecode)
	{
		half4 tex = tex2D (smp, i.texcoord);
		half3 c = DecodeHDR (tex, smpDecode);
		c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
		c *= _Exposure;
		return half4(c, tex.a);
	}
	ENDCG
	
	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		sampler2D _FrontSkyTex;
		half4 _FrontSkyTex_HDR;
		half4 frag (v2f i) : SV_Target { return skybox_frag(i,_FrontSkyTex, _FrontSkyTex_HDR); }
		ENDCG 
	}
	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		sampler2D _BackSkyTex;
		half4 _BackSkyTex_HDR;
		half4 frag (v2f i) : SV_Target { return skybox_frag(i,_BackSkyTex, _BackSkyTex_HDR); }
		ENDCG 
	}
	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		sampler2D _LeftSkyTex;
		half4 _LeftSkyTex_HDR;
		half4 frag (v2f i) : SV_Target { return skybox_frag(i,_LeftSkyTex, _LeftSkyTex_HDR); }
		ENDCG
	}
	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		sampler2D _RightSkyTex;
		half4 _RightSkyTex_HDR;
		half4 frag (v2f i) : SV_Target { return skybox_frag(i,_RightSkyTex, _RightSkyTex_HDR); }
		ENDCG
	}	
	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		sampler2D _UpSkyTex;
		half4 _UpSkyTex_HDR;
		half4 frag (v2f i) : SV_Target { return skybox_frag(i,_UpSkyTex, _UpSkyTex_HDR); }
		ENDCG
	}	
	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		sampler2D _DownSkyTex;
		half4 _DownSkyTex_HDR;
		half4 frag (v2f i) : SV_Target { return skybox_frag(i,_DownSkyTex, _DownSkyTex_HDR); }
		ENDCG
	}
	Pass{
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0
		sampler2D _FrontCloudsTex;
		half4 _FrontCloudsTex_HDR;
		half4 frag (v2f i) : SV_Target { return Cloud_frag(i,_FrontCloudsTex, _FrontCloudsTex_HDR); }
		ENDCG
	}
}
}
