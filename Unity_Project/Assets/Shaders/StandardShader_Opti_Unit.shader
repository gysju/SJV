// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/StantardShader_RGB_N_MGE_Unit" {
	Properties {
		_RGB_Nx ("RGB_Nx", 2D) = "white" {}
		[NoScaleOffset]_MRE_Ny ("MRE_Ny", 2D) = "white" {}
		[HDR]_EmissiveColor ("EmissiveColor", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_NormalIntensity("NormalIntensity", Range(0,5)) = 1

		_AlphaClip ("AlphaClip", 2D) = "white" {}
		_AlphaValue ("Alpha value", Range(0,1)) = 1
		_Rim ("rim value", Range(0,0.2)) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows addshadow
		#pragma target 5.0

		sampler2D _RGB_Nx, _MRE_Ny, _AlphaClip;

		struct Input 
		{
			float2 uv_RGB_Nx;
			float2 uv_AlphaClip;
        };
	
		half _Glossiness, _Metallic, _NormalIntensity, _AlphaValue, _Rim;

		fixed4 _EmissiveColor;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			half AlphaClip = tex2D ( _AlphaClip, IN.uv_AlphaClip).a;

			clip( _AlphaValue - AlphaClip);

			fixed4 RGB_Nx = tex2D (_RGB_Nx, IN.uv_RGB_Nx);
			fixed4 MRE_Ny = tex2D (_MRE_Ny, IN.uv_RGB_Nx);

            half3 N = UnpackScaleNormal(float4(0,RGB_Nx.a, 0, MRE_Ny.a), _NormalIntensity);

			if( AlphaClip > _AlphaValue - _Rim )
			{
				o.Emission = _EmissiveColor;
			}
			else
			{
				o.Emission = MRE_Ny.z * _EmissiveColor;
			}

			o.Alpha = 1;
  			
			o.Albedo = RGB_Nx.rgb;
			o.Normal = N;
			o.Metallic = (_Metallic * MRE_Ny.x);
			o.Smoothness = _Glossiness * ( 1 - MRE_Ny.y);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
