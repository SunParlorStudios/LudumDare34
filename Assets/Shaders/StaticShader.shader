Shader "Hidden/StaticShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Shift ("Image Distortion", Range(0.0, 1.0)) = 0.001
		_Frequency ("Image Distortion Frequency", Range(0.0, 1.0)) = 0.05
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Shift;
			float _Frequency;

			float rand(float2 co) 
			{
				return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				float r = rand(float2(_SinTime.r + sin(_Time.r * 2.0f), _CosTime.r));
				if (r <= _Frequency)
				{
					float2 coords = float2(i.uv.x + sin(i.uv.y * 20.0f + _Time.r * 700.0f) * 0.01f, i.uv.y);
					col *= 0.25f;
					col += (tex2D(_MainTex, coords - float2(_Shift * 5.0f, _Shift))) * float4(1.0f, 0.0f, 0.0f, 1.0f);
					col += (tex2D(_MainTex, coords - float2(-_Shift * 5.0f, _Shift))) * float4(0.0f, 1.0f, 0.0f, 1.0f);
					col += (tex2D(_MainTex, coords - float2(_Shift, _Shift * 5.0f))) * float4(0.0f, 0.0f, 1.0f, 1.0f);

				
				}
				r = rand(i.uv + _CosTime);
				col += pow(r, 0.25f) * 0.125f;
				return col;
			}
			ENDCG
		}
	}
}
