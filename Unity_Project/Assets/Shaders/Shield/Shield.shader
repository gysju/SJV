Shader "Custom/Shield" {
	Properties 
	{
		[HDR]
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Alpha", 2D) = "white" {}

		[Header(Effect)]
		_UVSpeed("UV Speed", Range(0,0.5)) = 0.25
		[Toggle] _UV_Direction("ChangeDirection", int) = 0
		_InvColorFade("Color Soft Factor", Range(5,50)) = 10.0
		_NormalIntensity("NormalIntensity", Range(1,5)) = 1
		_Alpha("Alpha", Range(0,1)) = 1
	}
	SubShader 
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		LOD 200
		ZWrite On

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert alpha noshadow 
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D_float _CameraDepthTexture;

		struct Input 
		{
			float2 uv_MainTex;
			float4 screenPos;
			float3 worldPos;
		};

		half _UV_Direction, _UVSpeed, _InvColorFade, _NormalIntensity, _Alpha;
		fixed4 _Color;
		float4 _CameraDepthTexture_TexelSize;

		void vert(inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input, data);
		}

		void surf (Input IN, inout SurfaceOutput o)
		{
			float eyeDepth = -mul(UNITY_MATRIX_V, float4(IN.worldPos, 1.0f)).z;
			float3 camViewDir = UNITY_MATRIX_IT_MV[2].xyz;
			float camAngle = saturate(dot(camViewDir, float3(0.0f, 1.0f, 0.0f)));

			float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos));
			float sceneZ = LinearEyeDepth(rawZ);
			float partZ = eyeDepth;
			float deltaZ = pow ((sceneZ - partZ) *lerp(0.1, 1.0, camAngle), 1.5);
			
			_UV_Direction = _UV_Direction * 2 - 1;
			_UVSpeed *= _UV_Direction;

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex + float2(0, _UVSpeed) * _Time.y);
			
			clip(_Alpha - c.x);

			o.Alpha = saturate(deltaZ * c.x);

			o.Albedo = min(deltaZ, _InvColorFade) * _Color;
			o.Normal = UnpackScaleNormal(float4(0, c.y, 0, c.z), _NormalIntensity);

		}
		ENDCG
	}
	FallBack "Diffuse"
}
