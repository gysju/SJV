Shader "Custom/VertexColor" {
	Properties {
		[Header(Tex 1 (R))]
		_ColorOne ("Color", Color) = (1,1,1,1)
		_RGB_Nx_One ("Albedo (RGB) Normal X (A)", 2D) = "white" {}
		[NoScaleOffset]_MRE_Ny_One ("Metalness (R) Roughness (G) Emissive (B)  Normal Y (A)", 2D) = "black" {}
		_NormalIntensityOne ("Normal intensity", Range ( 0, 5)) = 1
		_RoughnessOne ("Roughness", Range(0,1)) = 0.5
		_MetallicOne ("Metallic", Range(0,1)) = 0.0

		[Space(10)]
		_EmissiveColorOne ("Emissive color", Color) = (1,1,1,1)
		_EmissiveIntensityOne ("Emissive Intensity", Range(0,10)) = 0.0

		[Header(Tex 2 (G))]
		_ColorTwo ("Color", Color) = (1,1,1,1)
		_RGB_Nx_Two ("Albedo (RGB) Normal X (A)", 2D) = "white" {}
		[NoScaleOffset]_MRE_Ny_Two ("Metalness (R) Roughness (G) Emissive (B)  Normal Y (A)", 2D) = "black" {}
		_NormalIntensityTwo ("Normal intensity", Range ( 0, 5)) = 1
		_RoughnessTwo ("Roughness", Range(0,1)) = 0.5
		_MetallicTwo ("Metallic", Range(0,1)) = 0.0

		[Space(10)]
		_EmissiveColorTwo ("Emissive color", Color) = (1,1,1,1)
		_EmissiveIntensityTwo ("Emissive Intensity", Range(0,10)) = 0.0

		[Header(Tex 3 (B))]
		_ColorThree ("Color", Color) = (1,1,1,1)
		_RGB_Nx_Three ("Albedo (RGB) Normal X (A)", 2D) = "white" {}
		[NoScaleOffset]_MRE_Ny_Three ("Metalness (R) Roughness (G) Emissive (B)  Normal Y (A)", 2D) = "black" {}
		_NormalIntensityThree ("Normal intensity", Range ( 0, 5)) = 1
		_RoughnessThree ("Roughness", Range(0,1)) = 0.5
		_MetallicThree ("Metallic", Range(0,1)) = 0.0

		[Space(10)]
		_EmissiveColorThree ("Emissive color", Color) = (1,1,1,1)
		_EmissiveIntensityThree ("Emissive Intensity", Range(0,10)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 5.0

		sampler2D _RGB_Nx_One, _RGB_Nx_Two, _RGB_Nx_Three, _MRE_Ny_One, _MRE_Ny_Two, _MRE_Ny_Three;

		struct Input 
		{
			float2 uv_RGB_Nx_One;
			float2 uv_RGB_Nx_Two;
			float2 uv_RGB_Nx_Three;
			float4 color : COLOR;
		};

		half4 _ColorOne, _ColorTwo, _ColorThree, _EmissiveColorOne, _EmissiveColorTwo, _EmissiveColorThree;
		half _NormalIntensityOne, _NormalIntensityTwo, _NormalIntensityThree;
		half _RoughnessOne, _RoughnessTwo, _RoughnessThree;
		half _MetallicOne, _MetallicTwo, _MetallicThree;
		half _EmissiveIntensityOne, _EmissiveIntensityTwo, _EmissiveIntensityThree;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 RGB_Nx_One = tex2D (_RGB_Nx_One, IN.uv_RGB_Nx_One);
			fixed4 RGB_Nx_Two = tex2D (_RGB_Nx_Two, IN.uv_RGB_Nx_Two);
			fixed4 RGB_Nx_Three = tex2D (_RGB_Nx_Three, IN.uv_RGB_Nx_Three);

			fixed4 MRE_Ny_One = tex2D (_MRE_Ny_One, IN.uv_RGB_Nx_One);
			fixed4 MRE_Ny_Two = tex2D (_MRE_Ny_Two, IN.uv_RGB_Nx_Two);
			fixed4 MRE_Ny_Three = tex2D (_MRE_Ny_Three, IN.uv_RGB_Nx_Three);

			// albedo
			RGB_Nx_One.rgb *= _ColorOne * IN.color.r;
			RGB_Nx_Two.rgb *= _ColorTwo * IN.color.g;
			RGB_Nx_Three.rgb *= _ColorThree * IN.color.b;

			o.Albedo = (RGB_Nx_One.rgb + RGB_Nx_Two + RGB_Nx_Three);

			// normal 
			float normX = ( RGB_Nx_One.a * IN.color.r ) + ( RGB_Nx_Two.a * IN.color.g ) + ( RGB_Nx_Three.a * IN.color.b )  ;
			float normY = ( MRE_Ny_One.a * IN.color.r ) + ( MRE_Ny_Two.a * IN.color.g ) + ( MRE_Ny_Three.a * IN.color.b )  ;
			float normIntensity = ( _NormalIntensityOne * IN.color.r ) + ( _NormalIntensityTwo * IN.color.g ) + ( _NormalIntensityThree * IN.color.b )  ;

			o.Normal = UnpackScaleNormal(float4(0,normX, 0, normY), normIntensity);

			// Metalness
			MRE_Ny_One.r *= IN.color.r * _MetallicOne;
			MRE_Ny_Two.r *= IN.color.g * _MetallicTwo;
			MRE_Ny_Three.r *= IN.color.b * _MetallicThree;

			o.Metallic = MRE_Ny_One.r + MRE_Ny_Two.r + MRE_Ny_Two.r;

			// Rougness

			MRE_Ny_One.g *= _RoughnessOne * IN.color.r;
			MRE_Ny_Two.g *= _RoughnessTwo * IN.color.g;
			MRE_Ny_Three.g *= _RoughnessThree * IN.color.b;

			o.Smoothness = 1 - ( MRE_Ny_One.g + MRE_Ny_Two.g + MRE_Ny_Three.g);

			// Emissive
			half3 EmissiveOne = MRE_Ny_One.b * _EmissiveColorOne * _EmissiveIntensityOne * IN.color.r;
			half3 EmissiveTwo = MRE_Ny_Two.b * _EmissiveColorTwo * _EmissiveIntensityTwo * IN.color.g;
			half3 EmissiveThree = MRE_Ny_Three.b * _EmissiveColorThree * _EmissiveIntensityThree * IN.color.b;

			o.Emission = EmissiveOne + EmissiveTwo + EmissiveThree;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
