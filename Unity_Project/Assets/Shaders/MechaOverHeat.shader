Shader "Custom/MechaOverHeat" 
{
	Properties 
	{
		_RGB_Nx ("RGB_Nx", 2D) = "white" {}
		[NoScaleOffset]_MRE_Ny ("MRE_Ny", 2D) = "white" {}
		[HDR]_EmissiveColor ("EmissiveColor", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_NormalIntensity("NormalIntensity", Range(0,5)) = 1

		[Header(OverHeat)]
		_Mask("Mask", 2D) = "black" {}
		[HDR]_OverHeatColor ("OverHeatColor", Color) = (1,1,1,1)
		_OverHeatRange ("OverHeatRange", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 5.0

		sampler2D _RGB_Nx, _MRE_Ny, _Mask;

		struct Input 
		{
			float2 uv_RGB_Nx;
        };

		half _Glossiness, _Metallic, _NormalIntensity, _OverHeatRange;

		fixed4 _EmissiveColor, _OverHeatColor;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 RGB_Nx = tex2D (_RGB_Nx, IN.uv_RGB_Nx);
			fixed4 MRE_Ny = tex2D (_MRE_Ny, IN.uv_RGB_Nx);
			fixed Mask = tex2D (_Mask, IN.uv_RGB_Nx);

            half3 N = UnpackScaleNormal(float4(0,RGB_Nx.a, 0, MRE_Ny.a), _NormalIntensity);

			o.Emission = (MRE_Ny.z * _EmissiveColor) + (Mask * _OverHeatColor * _OverHeatRange);
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
