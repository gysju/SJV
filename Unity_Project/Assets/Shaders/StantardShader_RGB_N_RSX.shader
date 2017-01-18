﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/StantardShader_RGB_N_RSX" {
	Properties {
		_RGB_Nx ("RGB_Nx", 2D) = "white" {}
		[NoScaleOffset]_RSE_Ny ("RSE_Ny", 2D) = "white" {}
		_Specular ("Specular", Range(0,1)) = 0
		_NormalIntensity("NormalIntensity", Range(1,5)) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf BlinnPhong fullforwardshadows  vertex:vert

		#pragma target 5.0

		sampler2D _RGB_Nx, _RSE_Ny;
		half4 _RGB_Nx_ST;

		struct Input 
		{
            half4 normal_U;
            half4 tangent_V;
            half3 binormal;
        };

		half _Specular, _NormalIntensity;

		void vert( inout appdata_full v, out Input o )
        {
            float2 UV = v.texcoord;
            o.normal_U = half4(normalize(mul(unity_ObjectToWorld, half4(v.normal,0)).xyz), UV.x);
            o.tangent_V = half4(normalize(mul(unity_ObjectToWorld, half4(v.tangent.xyz,0)).xyz), UV.y);
            o.binormal = cross( o.normal_U.xyz, o.tangent_V.xyz ) * v.tangent.w;          
        }

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 RGB_Nx = tex2D (_RGB_Nx, float2(IN.normal_U.a, IN.tangent_V.a) * _RGB_Nx_ST.xy + _RGB_Nx_ST.wz);
			fixed4 RSE_Ny = tex2D (_RSE_Ny, float2(IN.normal_U.a, IN.tangent_V.a) * _RGB_Nx_ST.xy + _RGB_Nx_ST.wz);

            half3 N = UnpackScaleNormal(float4(0,RSE_Ny.a,0,RGB_Nx.a), _NormalIntensity);

  			o.Albedo = RGB_Nx.rgb;
			o.Normal = N;
			o.Specular = _Specular * RSE_Ny.y;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
