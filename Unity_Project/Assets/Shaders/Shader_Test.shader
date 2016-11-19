// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Shader_Test"
{
	Properties
	{
		_RGB_nX ("RGB_nX", 2D) = "" {}
		[NoScaleOffset]	_MRE_nY ("MRE_nY", 2D) = "" {}

		_EmissiveColor ("EmissiveColor", Color) = (1,1,1,1)
		_EmissiveIntensity ("EmissiveIntensity", Range(1,10)) = 1.0
		_EmissivePower ("Emissive Power", Range(1.0, 3.0)) = 1
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque"}
		Pass
		{
			Tags { "LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_fwdbase // car on va rajouter une passe pour generer de la shadow
			#include "UnityCG.cginc"
			#include "Lighting.cginc" // on peut recup les info directment d'unity'
			#include "AutoLight.cginc"
			
			sampler2D _RGB_nX, _MRE_nY;
			float4 _RGB_nX_ST, _EmissiveColor;
			float _EmissiveIntensity, _EmissivePower;

			struct appData	
			{
				float4 vertex	: POSITION;
				half3 normal	: NORMAL;
				half4 tangent	: TANGENT;
				float2 texcoord	: TEXCOORD0;
			};

			struct v2f	
			{
				float4 pos		: SV_POSITION;
				half4 T_wPosX	: ATTR0;
				half4 B_wPosY	: ATTR1;
				half4 N_wPosZ	: ATTR2;
				float2 UV		: ATTR3;
				LIGHTING_COORDS(4, 5)
			};

			v2f vert(appData v)
			{
				v2f o;
				o.UV = v.texcoord;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.N_wPosZ.xyz = normalize(mul(unity_ObjectToWorld, float4(v.normal,0)).xyz);
				o.T_wPosX.xyz = normalize(mul(unity_ObjectToWorld, half4(v.tangent.xyz,0)).xyz);
				o.B_wPosY.xyz = cross(o.N_wPosZ.xyz, o.T_wPosX.xyz) * v.tangent.w;

				float3 wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.T_wPosX.w = wPos.x;
				o.B_wPosY.w = wPos.y;
				o.N_wPosZ.w = wPos.z;
				
				TRANSFER_VERTEX_TO_FRAGMENT(o)

				return o;
			}

			float4 frag(v2f i) : COLOR
			{	
				float4 o;
				half4 RGB_nX = tex2D(_RGB_nX, (i.UV * _RGB_nX_ST.xy) + _RGB_nX_ST.wz).rgba;
				half4 MRE_nY = tex2D(_MRE_nY, (i.UV * _RGB_nX_ST.xy) + _RGB_nX_ST.wz).rgba;
				half2 Nxy = half2(RGB_nX.w, MRE_nY.w)*2-1;
				half3 N = normalize(i.B_wPosY.xyz * Nxy.y + (i.T_wPosX.xyz * Nxy.x + i.N_wPosZ.xyz));
				half3 L = _WorldSpaceLightPos0;
				half3 wPos = float3(i.T_wPosX.w,i.B_wPosY.w,i.N_wPosZ.w);
				half3 V = normalize(_WorldSpaceCameraPos-wPos);
				half3 H = normalize(V+L);
				half3 R = reflect(-V,N);
				half NdotL = saturate(dot(N,L));
				half NdotH = saturate(dot(N,H));
				half LdotH = saturate(dot(L,H));
				half NdotV = saturate(dot(N,V));
				half3 baseColor = RGB_nX.rgb;
				half gloss = MRE_nY.g;
				half e = exp2(gloss*12);
				half metalness = MRE_nY.r;
				
				//diffuse term
				half3 diffuse = NdotL * baseColor * (1-metalness) ;

				//analytic spec
				half specDistrib = NdotL * pow(NdotH,e) * (e *.125 + 1);

				half3 minSpec = float3(.05,.05,.05);
				half3 F0 = metalness * ( baseColor - minSpec) + minSpec;
				half3 invF0 = 1-F0;
				half fCurve = 1-LdotH;
				fCurve *= fCurve;
				fCurve *= fCurve;
				half3 fresnel = fCurve * (invF0) + F0;
				
				//env spec
				half envCurve = 1-NdotV;
				envCurve*=envCurve;
				envCurve*=envCurve;
				half3 envFresnel = envCurve * (invF0) + F0;
		
				half mipOffset = 1-saturate(gloss+envCurve);
				half4 envData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, R, mipOffset*7);
				half3 env = DecodeHDR(envData, unity_SpecCube0_HDR);

				//emissive
				float3 emissive = MRE_nY.z * pow(_EmissiveIntensity, _EmissivePower) * _EmissiveColor.rgb;

				//geovis
				half geoVis = NdotV * (1-gloss) + gloss;

				//spec term
				half3 specular = specDistrib * fresnel * geoVis;
				
				//ambient specular term
				half3 ambSpec = env * envFresnel * geoVis;
				
				//ambient diffuse term
				half4 ambData = UNITY_SAMPLE_TEXCUBE_LOD(unity_SpecCube0, N, 7);
				half3 ambient = DecodeHDR ( ambData, unity_SpecCube0_HDR) * baseColor * ( 1 - metalness);
				
				//final compo
				o.rgb =( diffuse + specular + emissive) * _LightColor0 * LIGHT_ATTENUATION(i) + ambSpec + ambient;
				o.a = 1;
				
				return o;
			}
			ENDCG
		}
		Pass
		{
			Name "ShadowCaster"
			Tags {"LightMode" = "ShadowCaster"}
			Cull Back
			Offset 0,-4 // PS4 oditty
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0

			#define UNITY_PASS_SHADOWCASTER
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#pragma multi_compile_shadowcaster

			struct appData
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				V2F_SHADOW_CASTER;
			};

			v2f vert(appData v)
			{
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
				TRANSFER_SHADOW_CASTER(o)
				return o;
			};

			half4 frag(v2f i ) : SV_Target
			{
				SHADOW_CASTER_FRAGMENT(i)
			}
			ENDCG
		}
	}
}
