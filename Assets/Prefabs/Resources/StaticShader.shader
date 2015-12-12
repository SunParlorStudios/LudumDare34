﻿Shader "Hidden/StaticShader"
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
			#pragma target 3.0
			
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
			half _Shift;
			half _Frequency;

			half rand(half2 co) 
			{
				return frac(sin(dot(co.xy, half2(12.989, 78.23))) * 4375.545);
			}

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				half r = rand(half2(_SinTime.r + sin(_Time.r * 2.0f), _CosTime.r));
				if (r <= _Frequency)
				{
					half2 coords = half2(i.uv.x + sin(i.uv.y * 20.0f + _Time.r * 700.0f) * 0.01f, i.uv.y);
					col *= 0.25f;
					col += (tex2D(_MainTex, coords - half2(_Shift * 5.0f, _Shift))) * half4(1.0f, 0.0f, 0.0f, 1.0f);
					col += (tex2D(_MainTex, coords - half2(-_Shift * 5.0f, _Shift))) * half4(0.0f, 1.0f, 0.0f, 1.0f);
					col += (tex2D(_MainTex, coords - half2(_Shift, _Shift * 5.0f))) * half4(0.0f, 0.0f, 1.0f, 1.0f);

				
				}

				r = rand(i.uv + _CosTime);
				col += pow(r, 0.25f) * 0.15f;

				col += fmod(i.uv.y, 0.02f) * 0.65f;
				return col;
			}
			ENDCG
		}
	}
}
