Shader "Hidden/StaticShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Shift ("Image Distortion", Range(0.0, 1.0)) = 0.001
		_Frequency ("Image Distortion Frequency", Range(0.0, 1.0)) = 0.05
		_Bloom ("Bloom Magnitude", Range(0.0, 1.0)) = 0.5
		_BloomBlur ("Bloom Blur Distance", Range(0.0, 1.0)) = 0.01
		_BloomPasses ("Bloom Blur Passes", Range(1.0, 3.0)) = 1.0
		_DoBloom("Do Bloom", Range(0.0, 1.0)) = 1.0
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			
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
			half _Bloom;
			half _BloomBlur;
			half _DoBloom;
			half _BloomPasses;

			half rand(half2 co) 
			{
				return frac(sin(dot(co.xy, half2(12.989, 78.23))) * 4375.545);
			}

			half4 GetBlurTexture(half2 uv, half blur, half passes)
			{
				half4 blurred = 0.0f;

				half pi = 3.14f;

				half step_angle = pi * 2.0f / passes;
				half step = blur / passes;

				half count = 0.0f;

				for (half a = 0.0f; a < 6.28f; a += 2.09f)
				{
					for (half s = 0.0f; s < 0.025f; s += 0.0083)
					{
						count += 0.5f;
						blurred += tex2D(_MainTex, half2(uv.x + cos(a) * s, uv.y + sin(a) * s * 1.5f));
					}
				}

				blurred /= count;

				return blurred;
			}

			half ToGray(half4 c)
			{
				return dot(c.rgb, half3(0.3, 0.59, 0.11));
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half2 coords = i.uv;
				fixed4 col = tex2D(_MainTex, coords);
				
				half r = rand(half2(_SinTime.r + sin(_Time.r * 2.0f), _CosTime.r));
				if (r <= _Frequency)
				{
					half2 shake = half2(coords.x + sin(coords.y * 20.0f + _Time.r * 700.0f) * 0.01f, coords.y);
					col *= 0.25f;
					col += (tex2D(_MainTex, shake - half2(_Shift * 5.0f, _Shift))) * half4(1.0f, 0.0f, 0.0f, 1.0f);
					col += (tex2D(_MainTex, shake - half2(-_Shift * 5.0f, _Shift))) * half4(0.0f, 1.0f, 0.0f, 1.0f);
					col += (tex2D(_MainTex, shake - half2(_Shift, _Shift * 5.0f))) * half4(0.0f, 0.0f, 1.0f, 1.0f);
				}

				half gray = ToGray(col);

				half4 bloom = 0.0f;
				half bloomMagnitude = 0.0f;

				if (_DoBloom == 1.0f)
				{
					bloom = GetBlurTexture(coords, _BloomBlur, _BloomPasses);
					bloomMagnitude = ToGray(saturate((col - _Bloom) / (1.0f - _Bloom)));
				}

				half2 centered = coords * 2.0f - 1.0f;

				half length = sqrt(centered.y * centered.y + centered.x * centered.x);

				float rampStart = 0.4f;
				float ratio = (length * (1.0f - rampStart) + rampStart);

				col += fmod(coords.y, 0.02f) * 2.0f * ratio;
				r = rand(coords + _CosTime);
				col += pow(r, 0.25f) * 0.2f * ratio;

				return lerp(col, bloom, bloomMagnitude); //* (0.5f + sin(_Time.r * 10.0f) * 0.5f));
			}
			ENDCG
		}
	}
}
