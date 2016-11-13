// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/StantardShader_Opti" {
	Properties {
		_RGB_Nx ("RGB_Nx", 2D) = "white" {}
		[NoScaleOffset]_MRE_Ny ("MRE_Ny", 2D) = "white" {}
		_EmissiveColor ("EmissiveColor", Color) = (1,1,1,1)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Emission ("Emission", Range(1,10)) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard Lambert vertex:vert

		#pragma target 5.0

		sampler2D _RGB_Nx, _MRE_Ny;
		half4 _RGB_Nx_ST;

		struct Input 
		{
            half4 normal_U;
            half4 tangent_V;
            half3 binormal;
        };

		half _Glossiness;
		half _Metallic;
		half _Emission;

		fixed4 _EmissiveColor;

		void vert( inout appdata_full v, out Input o )
        {
            float2 UV = v.texcoord;
            o.normal_U = half4(normalize(mul(unity_ObjectToWorld, half4(v.normal,0)).xyz), UV.x);
            o.tangent_V = half4(normalize(mul(unity_ObjectToWorld, half4(v.tangent.xyz,0)).xyz), UV.y);
            o.binormal = cross( o.normal_U.xyz, o.tangent_V.xyz ) * v.tangent.w;          
        }

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 RGB_Nx = tex2D (_RGB_Nx, float2(IN.normal_U.a, IN.tangent_V.a) * _RGB_Nx_ST.xy + _RGB_Nx_ST.wz);
			fixed4 MRE_Ny = tex2D (_MRE_Ny, float2(IN.normal_U.a, IN.tangent_V.a) * _RGB_Nx_ST.xy + _RGB_Nx_ST.wz);

            half3 N = UnpackNormal(float4(0,MRE_Ny.a,0,RGB_Nx.a));

  			o.Albedo = RGB_Nx.rgb;
			o.Normal = N;
			o.Metallic = _Metallic * MRE_Ny.x;
			o.Smoothness = _Glossiness;
			o.Emission = MRE_Ny.z * (_EmissiveColor * _Emission );
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
