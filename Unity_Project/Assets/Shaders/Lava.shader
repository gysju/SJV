Shader "Custom/Lava" 
{
	Properties 
	{
		[Header(Tex 1 Red canal)]
		_ColorOne("Color", Color) = (1,1,1,1)
		_RGB_Nx("RGB_Nx", 2D) = "white" {}
		[NoScaleOffset]_MRE_Ny("MRE_Ny", 2D) = "white" {}
		[HDR]_EmissiveColor("EmissiveColor", Color) = (1,1,1,1)
		
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_NormalIntensity("NormalIntensity", Range(0,5)) = 1
		_UVSpeed("UV Speed", Range(0,2)) = 1

		[Space(10)]
		[Header(Tex 2 Blue canal)]
		_ColorTwo("Color", Color) = (1,1,1,1)
		_RGB2_Nx("RGB_Nx", 2D) = "white" {}
		[NoScaleOffset]_MRE2_Ny("MRE_Ny", 2D) = "white" {}
		[HDR]_EmissiveColor2("EmissiveColor", Color) = (1,1,1,1)

		_Glossiness2("Smoothness", Range(0,1)) = 0.5
		_Metallic2("Metallic", Range(0,1)) = 0.0
		_NormalIntensity2("NormalIntensity", Range(0,5)) = 1

		_UVSpeed2("UV Speed", Range(0,2)) = 1
		[Toggle] _UV_Direction("ChangeDirection", int) = 0
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 5.0

		sampler2D _RGB_Nx, _MRE_Ny;
		sampler2D _RGB2_Nx, _MRE2_Ny;

		struct Input 
		{
			float2 uv_RGB_Nx;
			float4 color : COLOR;
		};

		half _Glossiness, _Metallic, _NormalIntensity, _UVSpeed, _UV_Direction;
		half _Glossiness2, _Metallic2, _NormalIntensity2, _UVSpeed2;
		fixed4 _EmissiveColor, _EmissiveColor2, _ColorOne, _ColorTwo;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half dir = _UV_Direction * 2 - 1;

			IN.color.g = step(0, IN.color.g);

			fixed4 RGB_Nx = tex2D(_RGB_Nx, IN.uv_RGB_Nx + float2(0.0f, _UVSpeed * dir * IN.color.g) * _Time.y);
			fixed4 MRE_Ny = tex2D(_MRE_Ny, IN.uv_RGB_Nx + float2(0.0f, _UVSpeed * dir * IN.color.g) * _Time.y);
			
			fixed4 RGB2_Nx = tex2D(_RGB2_Nx, IN.uv_RGB_Nx + float2(0.0f, _UVSpeed2 * dir * IN.color.g) * _Time.y);
			fixed4 MRE2_Ny = tex2D(_MRE2_Ny, IN.uv_RGB_Nx + float2(0.0f, _UVSpeed2 * dir * IN.color.g) * _Time.y);

			RGB_Nx.rgb *= _ColorOne;
			RGB2_Nx.rgb *= _ColorTwo;

			half3 N = UnpackScaleNormal(float4(0, RGB_Nx.a, 0, MRE_Ny.a), _NormalIntensity);
			half3 N2 = UnpackScaleNormal(float4(0, RGB2_Nx.a, 0, MRE2_Ny.a), _NormalIntensity2);

			o.Emission = (MRE_Ny.z * _EmissiveColor * IN.color.r) + (MRE2_Ny.z * _EmissiveColor2 * IN.color.b);
			o.Alpha = 1;

			o.Albedo = RGB_Nx.rgb * IN.color.r + RGB2_Nx.rgb * IN.color.b;
			o.Normal = N * IN.color.r + N2 * IN.color.b;
			o.Metallic = (_Metallic * ( MRE_Ny.x * IN.color.r )) + (_Metallic2 * (MRE2_Ny.x * IN.color.b));
			o.Smoothness = (_Glossiness * (1 - MRE_Ny.y) * IN.color.r) + (_Glossiness2 * (1 - MRE2_Ny.y) * IN.color.b);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
