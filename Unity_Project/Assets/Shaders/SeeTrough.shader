Shader "Custom/SeeTrough"
{
	Properties
	{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Cutoff("Alpha cutoff", Range(0,1)) = 0.5
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
				float3 viewDir	: TEXCOORD1;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Cutoff;

			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				o.viewDir = WorldSpaceViewDir(v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord);
				float3 vertexView = normalize(i.viewDir);
				float3 viewDir = UNITY_MATRIX_IT_MV[2].xyz;

				float zlv = 1 - dot(vertexView, viewDir);
				//clip(1 - dot(vertexView, viewDir));
				clip(col.a - _Cutoff);
				return float4(zlv, zlv, zlv, 1.0f);
			}
		ENDCG
		}
	}
}
