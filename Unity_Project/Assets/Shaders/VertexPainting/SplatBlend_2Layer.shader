Shader "VertexPainter/SplatBlend_2Layer" 
{
   Properties {
      _RGB_Nx1 ("Albedo + Nx", 2D) = "white" {}
      [NoScaleOffset]_MEH_Ny1 ("Metal + Emissive + Height + Ny", 2D) = "white" {}

      _Tint1 ("Tint", Color) = (1, 1, 1, 1)
      _Glossiness1 ("Smoothness", Range(0,1)) = 0.5
      _Metallic1 ("Metallic", Range(0,1)) = 0.0
      _EmissiveMult1("Emissive Multiplier", Float) = 1
      _EmissiveColor1 ("EmissiveColor", Color) = (1, 1, 1, 1)
      _TexScale1 ("Texture Scale", Float) = 1
      
      _RGB_Nx2 ("Albedo + Nx", 2D) = "white" {}
      [NoScaleOffset]_MEH_Ny2 ("Metal + Emissive + Height + Ny", 2D) = "white" {}

      _Tint2 ("Tint", Color) = (1, 1, 1, 1)
      _Glossiness2 ("Smoothness", Range(0,1)) = 0.5
      _Metallic2 ("Metallic", Range(0,1)) = 0.0
      _EmissiveMult2("Emissive Multiplier", Float) = 1
      _EmissiveColor2 ("EmissiveColor", Color) = (1, 1, 1, 1)
      _TexScale2 ("Texture Scale", Float) = 1
      _Contrast2("Contrast", Range(0,0.99)) = 0.5
      
      _FlowSpeed ("Flow Speed", Float) = 0
      _FlowIntensity ("Flow Intensity", Float) = 1
      _FlowAlpha ("Flow Alpha", Range(0, 1)) = 1
      _FlowRefraction("Flow Refraction", Range(0, 0.3)) = 0.04

      _DistBlendMin("Distance Blend Begin", Float) = 0
      _DistBlendMax("Distance Blend Max", Float) = 100
      _DistUVScale1("Distance UV Scale", Float) = 0.5
      _DistUVScale2("Distance UV Scale", Float) = 0.5
   }
   SubShader {
      Tags { "RenderType"="Opaque" }
      LOD 200
      
      CGPROGRAM
      
      #pragma surface surf Standard vertex:vert fullforwardshadows

      #pragma shader_feature __ _FLOW1 _FLOW2 
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

         INIT_FLOW
         #if _FLOWDRIFT 
         fixed4 RGB_Nx1 = FETCH_TEX1(_RGB_Nx1, uv1);
         fixed4 RGB_Nx2 = FETCH_TEX2(_RGB_Nx2, uv2);
		 
         fixed4 MEH_Ny1 = FETCH_TEX1(_MEH_Ny1, uv1);
         fixed4 MEH_Ny2 = FETCH_TEX2(_MEH_Ny2, uv2);
         #elif _DISTBLEND
         fixed4 RGB_Nx1 = lerp(tex2D(_RGB_Nx1, uv1), tex2D(_RGB_Nx1, uv1*_DistUVScale1), dist);
         fixed4 RGB_Nx2 = lerp(tex2D(_RGB_Nx2, uv2), tex2D(_RGB_Nx2, uv2*_DistUVScale2), dist);
		 
         fixed4 MEH_Ny1 = lerp(tex2D(_MEH_Ny1, uv1), tex2D(_MEH_Ny1, uv1*_DistUVScale1), dist);
         fixed4 MEH_Ny2 = lerp(tex2D(_MEH_Ny2, uv2), tex2D(_MEH_Ny2, uv2*_DistUVScale2), dist);
         #else
         fixed4 RGB_Nx1 = tex2D(_RGB_Nx1, uv1);
         fixed4 RGB_Nx2 = tex2D(_RGB_Nx2, uv2);

         fixed4 MEH_Ny1 = tex2D(_MEH_Ny1, uv1);
         fixed4 MEH_Ny2 = tex2D(_MEH_Ny2, uv2);
         #endif

         RGB_Nx1.rgb *= _Tint1.rgb;
         RGB_Nx2.rgb *= _Tint2.rgb;
         MEH_Ny1.g *=_EmissiveMult1;
         MEH_Ny2.g *=_EmissiveMult2;

         half b1 = HeightBlend(MEH_Ny1.g, MEH_Ny2.g, IN.color.r, _Contrast2);
         fixed4 RGB_Nx = lerp(RGB_Nx1, RGB_Nx2, b1);
         fixed4 MEH_Ny = lerp(MEH_Ny1, MEH_Ny2, b1);
		 fixed emissiveColor = lerp(_EmissiveColor1, _EmissiveColor2, b1);
         // flow refraction; use difference in depth to control refraction amount, refetch all previous color textures if not parallaxing
         #if _FLOW2
			
            b1 *= _FlowAlpha;
            #if _FLOWREFRACTION
               half4 rn = FETCH_TEX2 (_Normal2, uv2) - 0.5;
               uv1 += rn.xy * b1 * _FlowRefraction;
            #endif
         #endif
                           
         o.Normal =  UnpackNormal(float4(0, RGB_Nx.a,0, MEH_Ny.a));
         
         o.Smoothness = 0;
         o.Metallic = MEH_Ny.r;
		 o.Emission = MEH_Ny.g * emissiveColor;// * lerp(_EmissiveColor1, _EmissiveColor2, b1);
         o.Albedo = RGB_Nx.rgb;
         
      }
      ENDCG
   } 
   CustomEditor "SplatMapShaderGUI"
   FallBack "Diffuse"
}