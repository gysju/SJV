Shader "Custom/StandardWithDecals" 
{
	Properties 
	{
		_Color ("Color", Color) = (1,1,1,1)
		_RGB_Nx("RGB_Nx", 2D) = "white" {}
		[NoScaleOffset]_MRA_Ny("MRA_Ny", 2D) = "white" {}
		[NoScaleOffset]_Emissive("Emissive", 2D) = "black" {}

		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_EmissiveIntensity("Emissive intensity", Range(0,10)) = 1
		_NormalIntensity("normal intensity", Range(1,5)) = 1
	}
	SubShader 
	{
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry+1" "ForceNoShadowCasting" = "True" }
		LOD 200
		Offset -1, -1

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows decal:blend
		#pragma target 3.0

		sampler2D _RGB_Nx, _MRA_Ny, _Emissive;

		struct Input 
		{
			float2 uv_RGB_Nx;
		};

		half _Glossiness, _Metallic, _NormalIntensity, _EmissiveIntensity;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 RGB_Nx = tex2D(_RGB_Nx, IN.uv_RGB_Nx);
			fixed4 MRA_Ny = tex2D(_MRA_Ny, IN.uv_RGB_Nx);
			fixed3 Emissive = tex2D(_Emissive, IN.uv_RGB_Nx).rgb;

			half3 N = UnpackScaleNormal(float4(0, RGB_Nx.a, 0, MRA_Ny.a), _NormalIntensity);

			o.Emission = Emissive * _EmissiveIntensity;
			o.Alpha = MRA_Ny.b;
			o.Normal = N;
			o.Albedo = RGB_Nx.rgb;
			o.Metallic = _Metallic * MRA_Ny.r;
			o.Smoothness = _Glossiness * (1 - MRA_Ny.g);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
