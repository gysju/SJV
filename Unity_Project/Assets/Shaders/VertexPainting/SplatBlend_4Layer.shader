Shader "VertexPainter/SplatBlend_4Layer" 
{
	Properties 
	{
		_RGB_Nx1("Albedo + Nx", 2D) = "white" {}
		[NoScaleOffset]_MEH_Ny1("Metal + Emissive + Height + Ny", 2D) = "white" {}
		_Tint1("Tint", Color) = (1, 1, 1, 1)
		_Glossiness1("Smoothness", Range(0,1)) = 0.5
		_Metallic1("Metallic", Range(0,1)) = 0.0
		_EmissiveMult1("Emissive Multiplier", Float) = 1
		_EmissiveColor1("EmissiveColor", Color) = (1, 1, 1, 1)
		_TexScale1("Texture Scale", Float) = 1

		_RGB_Nx2("Albedo + Nx", 2D) = "white" {}
		[NoScaleOffset]_MEH_Ny2("Metal + Emissive + Height + Ny", 2D) = "white" {}
		_Tint2("Tint", Color) = (1, 1, 1, 1)
		_Glossiness2("Smoothness", Range(0,1)) = 0.5
		_Metallic2("Metallic", Range(0,1)) = 0.0
		_EmissiveMult2("Emissive Multiplier", Float) = 1
		_EmissiveColor2("EmissiveColor", Color) = (1, 1, 1, 1)
		_TexScale2("Texture Scale", Float) = 1
		_Contrast2("Contrast", Range(0,0.99)) = 0.5

		_RGB_Nx3("Albedo + Nx", 2D) = "white" {}
		[NoScaleOffset]_MEH_Ny3("Metal + Emissive + Height + Ny", 2D) = "white" {}
		_Tint3("Tint", Color) = (1, 1, 1, 1)
		_Glossiness3("Smoothness", Range(0,1)) = 0.5
		_Metallic3("Metallic", Range(0,1)) = 0.0
		_EmissiveMult3("Emissive Multiplier", Float) = 1
		_EmissiveColor3("EmissiveColor", Color) = (1, 1, 1, 1)
		_TexScale3("Texture Scale", Float) = 1
		_Contrast3("Contrast", Range(0,0.99)) = 0.5

		_RGB_Nx4("Albedo + Nx", 2D) = "white" {}
		[NoScaleOffset]_MEH_Ny4("Metal + Emissive + Height + Ny", 2D) = "white" {}
		_Tint4("Tint", Color) = (1, 1, 1, 1)
		_Glossiness4("Smoothness", Range(0,1)) = 0.5
		_Metallic4("Metallic", Range(0,1)) = 0.0
		_EmissiveMult4("Emissive Multiplier", Float) = 1
		_EmissiveColor4("EmissiveColor", Color) = (1, 1, 1, 1)
		_TexScale4("Texture Scale", Float) = 1
		_Contrast4("Contrast", Range(0,0.99)) = 0.5

		_FlowSpeed("Flow Speed", Float) = 0
		_FlowIntensity("Flow Intensity", Float) = 1
		_FlowAlpha("Flow Alpha", Range(0, 1)) = 1
		_FlowRefraction("Flow Refraction", Range(0, 0.3)) = 0.04

		_DistBlendMin("Distance Blend Begin", Float) = 0
		_DistBlendMax("Distance Blend Max", Float) = 100
		_DistUVScale1("Distance UV Scale", Float) = 0.5
		_DistUVScale2("Distance UV Scale", Float) = 0.5
		_DistUVScale3("Distance UV Scale", Float) = 0.5
		_DistUVScale4("Distance UV Scale", Float) = 0.5
	}
	SubShader 
	{
      Tags { "RenderType"="Opaque" }
      LOD 200
      
      CGPROGRAM
      
      #pragma surface surf Standard vertex:vert fullforwardshadows
      
      // flow map keywords. 
      #pragma shader_feature __ _FLOW1 _FLOW2 _FLOW3 _FLOW4 
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
			float2 uv4 = IN.uv_RGB_Nx1 * _TexScale4;

			INIT_FLOW
			#if _FLOWDRIFT
			fixed4 RGB_Nx1 = FETCH_TEX1(_RGB_Nx1, uv1);
			fixed4 RGB_Nx2 = FETCH_TEX2(_RGB_Nx2, uv2);
			fixed4 RGB_Nx3 = FETCH_TEX3(_RGB_Nx3, uv3);
			fixed4 RGB_Nx4 = FETCH_TEX4(_RGB_Nx4, uv4);

			fixed4 MEH_Ny1 = FETCH_TEX1(_MEH_Ny1, uv1);
			fixed4 MEH_Ny2 = FETCH_TEX2(_MEH_Ny2, uv2);
			fixed4 MEH_Ny3 = FETCH_TEX3(_MEH_Ny3, uv3);
			fixed4 MEH_Ny4 = FETCH_TEX4(_MEH_Ny4, uv4);

			#elif _DISTBLEND
			fixed4 RGB_Nx1 = lerp(tex2D(_RGB_Nx1, uv1), tex2D(_RGB_Nx1, uv1*_DistUVScale1), dist);
			fixed4 RGB_Nx2 = lerp(tex2D(_RGB_Nx2, uv2), tex2D(_RGB_Nx2, uv2*_DistUVScale2), dist);
			fixed4 RGB_Nx3 = lerp(tex2D(_RGB_Nx3, uv3), tex2D(_RGB_Nx3, uv3*_DistUVScale3), dist);
			fixed4 RGB_Nx4 = lerp(tex2D(_RGB_Nx4, uv4), tex2D(_RGB_Nx4, uv4*_DistUVScale4), dist);

			fixed4 MEH_Ny1 = lerp(tex2D(_MEH_Ny1, uv1), tex2D(_MEH_Ny1, uv1*_DistUVScale1), dist);
			fixed4 MEH_Ny2 = lerp(tex2D(_MEH_Ny2, uv2), tex2D(_MEH_Ny2, uv2*_DistUVScale2), dist);
			fixed4 MEH_Ny3 = lerp(tex2D(_MEH_Ny3, uv3), tex2D(_MEH_Ny3, uv3*_DistUVScale3), dist);
			fixed4 MEH_Ny4 = lerp(tex2D(_MEH_Ny4, uv4), tex2D(_MEH_Ny4, uv4*_DistUVScale4), dist);

			#else
			fixed4 RGB_Nx1 = tex2D(_RGB_Nx1, uv1);
			fixed4 RGB_Nx2 = tex2D(_RGB_Nx2, uv2);
			fixed4 RGB_Nx3 = tex2D(_RGB_Nx3, uv3);
			fixed4 RGB_Nx4 = tex2D(_RGB_Nx4, uv4);

			fixed4 MEH_Ny1 = tex2D(_MEH_Ny1, uv1);
			fixed4 MEH_Ny2 = tex2D(_MEH_Ny2, uv2);
			fixed4 MEH_Ny3 = tex2D(_MEH_Ny3, uv3);
			fixed4 MEH_Ny4 = tex2D(_MEH_Ny4, uv4);
			#endif

			half b1 = HeightBlend(0, 0, IN.color.r, _Contrast2);
			fixed h1 = lerp(0, 0, b1);
			half b2 = HeightBlend(h1, 0, IN.color.g, _Contrast3);
			fixed h2 = lerp(h1, 0, b1);
			half b3 = HeightBlend(h2, 0, IN.color.b, _Contrast4);

			#if _FLOW2
			b1 *= _FlowAlpha;
			#if _FLOWREFRACTION
				half4 rn = FETCH_TEX2 (_Normal2, uv2) - 0.5;
				uv1 += rn.xy * b1 * _FlowRefraction;
			#endif
			#endif
			#if _FLOW3
			b2 *= _FlowAlpha;
			#if _FLOWREFRACTION
				half4 rn = FETCH_TEX3 (_Normal3, uv3) - 0.5;
				uv1 += rn.xy * b1 * _FlowRefraction;
				uv2 += rn.xy * b2 * _FlowRefraction;
			#endif
			#endif
			#if _FLOW4
			b3 *= _FlowAlpha;
			#if _FLOWREFRACTION
				half4 rn = FETCH_TEX4 (_Normal4, uv4) - 0.5;
				uv1 += rn.xy * b1 * _FlowRefraction;
				uv2 += rn.xy * b2 * _FlowRefraction;
				uv3 += rn.xy * b3 * _FlowRefraction;
			#endif
			#endif
                  
			RGB_Nx1.rgb *= _Tint1.rgb;
			RGB_Nx2.rgb *= _Tint2.rgb;
			RGB_Nx3.rgb *= _Tint3.rgb;
			RGB_Nx4.rgb *= _Tint4.rgb;

			MEH_Ny1.g *= _EmissiveMult1;
			MEH_Ny2.g *= _EmissiveMult2;
			MEH_Ny3.g *= _EmissiveMult3;
			MEH_Ny4.g *= _EmissiveMult4;

			fixed4 RGB_Nx = lerp(lerp(lerp(RGB_Nx1, RGB_Nx2, b1), RGB_Nx3, b2), RGB_Nx4, b3);
			fixed4 MEH_Ny = lerp(lerp(lerp(MEH_Ny1, MEH_Ny2, b1), MEH_Ny3, b2), MEH_Ny4, b3);

			o.Normal = UnpackNormal(float4(0, RGB_Nx.a, 0, MEH_Ny.a));
			o.Smoothness = 0;
			o.Metallic = MEH_Ny.r;

			o.Emission = 0;
			o.Albedo = RGB_Nx.rgb;
		}
		ENDCG
	} 
	CustomEditor "SplatMapShaderGUI"
	FallBack "Diffuse"
}