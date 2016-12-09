Shader "VertexPainter/SplatBlend_3Layer" 
{
	Properties 
	{
		_RGB_Nx1 ("Albedo + Nx", 2D) = "white" {}
		[NoScaleOffset]_REH_Ny1("Roughness + Emissive + Height + Ny", 2D) = "white" {}

		_Tint1 ("Tint", Color) = (1, 1, 1, 1)
		_Roughness1 ("Smoothness", Range(0,1)) = 0.5
		_EmissiveMult1("Emissive Multiplier", Float) = 1
		_EmissiveColor1("EmissiveColor", Color) = (1, 1, 1, 1)
		_TexScale1 ("Texture Scale", Float) = 1 
      
		_RGB_Nx2("Albedo + Nx", 2D) = "white" {}
		[NoScaleOffset]_REH_Ny2("Roughness + Emissive + Height + Ny", 2D) = "white" {}
		_Tint2 ("Tint", Color) = (1, 1, 1, 1)
		_Roughness2 ("Smoothness", Range(0,1)) = 0.5
		_Emissive2  ("Emissive", 2D) = "black" {}
		_EmissiveMult2("Emissive Multiplier", Float) = 1
		_EmissiveColor2("EmissiveColor", Color) = (1, 1, 1, 1)
		_TexScale2 ("Texture Scale", Float) = 1
		_Contrast2("Contrast", Range(0,0.99)) = 0.5
      
		_RGB_Nx3("Albedo + Nx", 2D) = "white" {}
		[NoScaleOffset]_REH_Ny3("Roughness + Emissive + Height + Ny", 2D) = "white" {}
		_Tint3 ("Tint", Color) = (1, 1, 1, 1)
		_Roughness3 ("Smoothness", Range(0,1)) = 0.5
		_EmissiveMult3("Emissive Multiplier", Float) = 1
		_EmissiveColor3("EmissiveColor", Color) = (1, 1, 1, 1)
		_TexScale3 ("Texture Scale", Float) = 1
		_Contrast3("Contrast", Range(0,0.99)) = 0.5
      
		_Normal("NormalIntensity", Range(1,5)) = 1
		_FlowSpeed ("Flow Speed", Float) = 0
		_FlowIntensity ("Flow Intensity", Float) = 1
		_FlowAlpha ("Flow Alpha", Range(0, 1)) = 1
		_FlowRefraction("Flow Refraction", Range(0, 0.3)) = 0.04

		_DistBlendMin("Distance Blend Begin", Float) = 0
		_DistBlendMax("Distance Blend Max", Float) = 100
		_DistUVScale1("Distance UV Scale", Float) = 0.5
		_DistUVScale2("Distance UV Scale", Float) = 0.5
		_DistUVScale3("Distance UV Scale", Float) = 0.5
	}
	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
      
		CGPROGRAM
      
		#pragma surface surf Standard vertex:vert fullforwardshadows

		// flow map keywords. 
		#pragma shader_feature __ _FLOW1 _FLOW2 _FLOW3
		#pragma shader_feature __ _FLOWDRIFT
		#pragma shader_feature __ _FLOWREFRACTION
		#pragma shader_feature __ _DISTBLEND
		#include "SplatBlend_Shared.cginc"
      
		void vert (inout appdata_full v, out Input o) 
		{
			SharedVert(v,o);
		}
      
		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			COMPUTEDISTBLEND

			float2 uv1 = IN.uv_RGB_Nx1 * _TexScale1;
			float2 uv2 = IN.uv_RGB_Nx1 * _TexScale2;
			float2 uv3 = IN.uv_RGB_Nx1 * _TexScale3;
			
			INIT_FLOW
			#if _FLOWDRIFT 
			fixed4 RGB_Nx1 = FETCH_TEX1(_RGB_Nx1, uv1);
			fixed4 RGB_Nx2 = FETCH_TEX2(_RGB_Nx2, uv2);
			fixed4 RGB_Nx3 = FETCH_TEX3(_RGB_Nx3, uv3);

			fixed4 REH_Ny1 = FETCH_TEX1(_REH_Ny1, uv1);
			fixed4 REH_Ny2 = FETCH_TEX2(_REH_Ny2, uv2);
			fixed4 REH_Ny3 = FETCH_TEX3(_REH_Ny3, uv3);
			#elif _DISTBLEND
			fixed4 RGB_Nx1 = lerp(tex2D(_RGB_Nx1, uv1), tex2D(_RGB_Nx1, uv1*_DistUVScale1), dist);
			fixed4 RGB_Nx2 = lerp(tex2D(_RGB_Nx2, uv2), tex2D(_RGB_Nx2, uv2*_DistUVScale2), dist);
			fixed4 RGB_Nx3 = lerp(tex2D(_RGB_Nx3, uv3), tex2D(_RGB_Nx3, uv3*_DistUVScale3), dist);

			fixed4 REH_Ny1 = lerp(tex2D(_REH_Ny1, uv1), tex2D(_REH_Ny1, uv1*_DistUVScale1), dist);
			fixed4 REH_Ny2 = lerp(tex2D(_REH_Ny2, uv2), tex2D(_REH_Ny2, uv2*_DistUVScale2), dist);
			fixed4 REH_Ny3 = lerp(tex2D(_REH_Ny3, uv3), tex2D(_REH_Ny3, uv3*_DistUVScale3), dist);
			#else
			fixed4 RGB_Nx1 = tex2D(_RGB_Nx1, uv1);
			fixed4 RGB_Nx2 = tex2D(_RGB_Nx2, uv2);
			fixed4 RGB_Nx3 = tex2D(_RGB_Nx3, uv3);

			fixed4 REH_Ny1 = tex2D(_REH_Ny1, uv1);
			fixed4 REH_Ny2 = tex2D(_REH_Ny2, uv2);
			fixed4 REH_Ny3 = tex2D(_REH_Ny3, uv3);
			#endif
         
			half b1 = HeightBlend(REH_Ny1.g, REH_Ny1.g, IN.color.r, _Contrast2);
			fixed h1 =  lerp(REH_Ny1.g, REH_Ny2.g, b1);
			half b2 = HeightBlend(h1, REH_Ny3.g, IN.color.g, _Contrast3);

			//#if _FLOW2
			//b1 *= _FlowAlpha;
			//#if _FLOWREFRACTION
			//	half4 rn = FETCH_TEX2 (_Normal2, uv2) - 0.5;
			//	uv1 += rn.xy * b1 * _FlowRefraction;
			//#endif
			//#endif
			//#if _FLOW3
			//b2 *= _FlowAlpha;
			//#if _FLOWREFRACTION
			//	half4 rn = FETCH_TEX3 (_Normal3, uv3) - 0.5;
			//	uv1 += rn.xy * b1 * _FlowRefraction;
			//	uv2 += rn.xy * b2 * _FlowRefraction;
			//#endif
			//#endif
			
			RGB_Nx1.rgb *= _Tint1;
			RGB_Nx2.rgb *= _Tint2;
			RGB_Nx3.rgb *= _Tint3;
			REH_Ny1.g *= _EmissiveMult1;
			REH_Ny2.g *= _EmissiveMult2;
			REH_Ny3.g *= _EmissiveMult3;
			REH_Ny1.r *= _Roughness1;
			REH_Ny2.r *= _Roughness2;
			REH_Ny3.r *= _Roughness3;

			fixed4 RGB_Nx = lerp(lerp( RGB_Nx1, RGB_Nx2, b1), RGB_Nx3, b2);
			fixed4 REH_Ny = lerp(lerp(REH_Ny1, REH_Ny2, b1), REH_Ny3, b2);

			float emissiveColor = lerp(lerp(_EmissiveColor1, _EmissiveColor2, b1), _EmissiveColor3, b2);
			o.Normal = UnpackNormal(float4(0, RGB_Nx.a, 0, REH_Ny.a)) * _NormalIntensity;
			
			o.Smoothness = 1 - REH_Ny.r;
			o.Metallic = 0;		         
			o.Emission = REH_Ny.g *= emissiveColor;
         	o.Albedo = RGB_Nx.rgb;
		
		}
		ENDCG
	} 
	CustomEditor "SplatMapShaderGUI"
	FallBack "Diffuse"
}