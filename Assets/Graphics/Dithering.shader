Shader "Hidden/Dithering"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Downscaling("Downscaling", Int) = 4
		_ColorDepth("Color Depth", Int) = 4
		_DitherStrength("Dither Strength", Range(0, 1)) = 0.1
		_LSD("LSD", Range(0, 1)) = 0
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			int _ColorDepth;
			float _DitherStrength;
			float _Downscaling;
			float _LSD;

			/*static const float4x4 ditherTable = float4x4
			(
				8.0, -0.0, 6.0, -2.0,
				-4.0, 4.0, -6.0, 2.0,
				5.0, -3.0, 7.0, -1.0,
				-7.0, 1.0, -5.0, 3.0
			);*/
			static const float4x4 ditherTable = float4x4
			(
				-8.0, 0.0, -6.0, 2.0,
				4.0, -4.0, 6.0, -2.0,
				-5.0, 3.0, -7.0, 1.0,
				7.0, -1.0, 5.0, -3.0
			);
			/*static const float4x4 ditherTable = float4x4
			(
				3.0, 1.0, -1.0, -3.0,
				1.0, 3.0, -3.0, -1.0,
				-1.0, -3.0, 3.0, 1.0,
				-3.0, -1.0, 1.0, 3.0
			);*/

			//-4.0, 0.0, -3.0, 1.0,
			//2.0, -2.0, 3.0, -1.0,
			//-3.0, 1.0, -4.0, 0.0,
			//3.0, -1.0, 2.0, -2.0
			

			//3.0, 1.0, -1.0, -3.0,
			//1.0, 3.0, -3.0, -1.0,
			//-1.0, -3.0, 3.0, 1.0,
			//-3.0, -1.0, 1.0, 3.0

			fixed4 frag(v2f i) : SV_TARGET
			{
				uint2 pixelCoord = i.uv * _ScreenParams.xy;
				pixelCoord = floor(pixelCoord / _Downscaling);
				float2 cord = pixelCoord * _Downscaling / _ScreenParams.xy;

				fixed4 col = tex2D(_MainTex, cord);
				float brightness = 0.2126 * col.r + 0.7152 * col.g + 0.0722 * col.b;

				float dither = ditherTable[pixelCoord.x % 4][pixelCoord.y % 4];
				float newBrightness = brightness + dither * _DitherStrength;
				fixed4 color = col / brightness * round(newBrightness * _ColorDepth) / _ColorDepth;

				col += dither * _DitherStrength;
				fixed4 rainbowColor = round(col * _ColorDepth) / _ColorDepth;
				return color + (rainbowColor - color) * _LSD;
			}
			ENDCG
		}
	}
}
