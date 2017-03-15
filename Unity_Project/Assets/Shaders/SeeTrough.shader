// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/SeeTrough"
{
	Properties
	{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_EffectTex("Effect", 2D) = "black" {}
		[HDR]_EffectColor("EffectColor", Color) = (1,1,1,1)
		_SpeedEffect("SpeedEffect", Range(0,1)) = 0.5

		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5

		[Header(Hit)]
		[HDR]_HitColor("HitColor", Color) = (1,1,1,1)
		_HitOutlineSize("HitOutlineSize", Range(0,0.1)) = 0.1
		_RadiusMax("RadiusMax", Range(0,1)) = 0.5
		_HitSpeed("HitSpeed", Range(0,1)) = 0.5
	}
		
	SubShader
	{
		Tags{ "Queue" = "AlphaTest" "IgnoreProjector" = "True" "RenderType" = "TransparentCutout" }
		LOD 100

		Lighting Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 5.0

			#include "UnityCG.cginc"

			struct appdata_t 
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float4 viewDir	: TEXCOORD1;
				float3 worldPos : TEXCOORD2;

				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex, _EffectTex;
			float4 _MainTex_ST;
			float3 _HitPos, _HitColor, _EffectColor;
			fixed _Cutoff, _HitOutlineSize, _RadiusMax, _HitSpeed, _CurrentHitTime, _SpeedEffect;
			 
			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.viewDir = mul(unity_WorldToObject, float4( WorldSpaceViewDir(v.vertex), 0.0f));
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{

				float len = length(_HitPos.xyz - i.worldPos);
				float3 vertexView = normalize(i.viewDir.xyz);
				float3 viewDir = UNITY_MATRIX_IT_MV[2].xyz;

				float outline = step( len, _RadiusMax + _HitOutlineSize);
				float inside = step( _RadiusMax, len);

				float3 hit = _HitColor * outline * inside;
				fixed4 col = tex2D(_MainTex, i.texcoord);

				fixed timeEffect = tex2D(_EffectTex, i.texcoord + float2( 0.0f, _Time.y * _SpeedEffect)).r;
				fixed zoneEffect = tex2D(_EffectTex, i.texcoord).g;

				col.xyz -= outline * inside;
				col.xyz = saturate(col.xyz) + hit;
				col.xyz = _EffectColor * zoneEffect * timeEffect;

				float zlv = 1 - dot(vertexView, viewDir);

				//clip(zlv + outline * inside);
				clip(col.a - _Cutoff + outline * inside);
				return col;
			}
			ENDCG
		}
	}
}
