// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Dixit/MainMenu/Profile/Stats"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Gauge1 ("Gauge 1", Range(0,1)) = 0.5
		_ForegroundColor("Foreground Color", Color) = (1,1,1,1)
		_ForegroundCutoff("Foreground Cutoff", Range(0,1)) = 0.5
		_BackgroundCutoff("Background Cutoff", Range(0,1)) = 0.5

		_Smoothness("Smoothness", Range(0,1)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#define PI 3.14159265359

			#include "UnityCG.cginc"

			struct VS_INPUT
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct FS_INPUT
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			uniform float _Gauge1;
			fixed4 _ForegroundColor;
			half _ForegroundCutoff;
			half _BackgroundCutoff;

			uniform float _Smoothness;
			
			FS_INPUT vert (VS_INPUT v)
			{
				FS_INPUT o = (FS_INPUT)0;

				// Vertex (position)

				//v.vertex.xy += sin(_Time.y * 3.14);
				o.vertex = UnityObjectToClipPos(v.vertex);

				// UV (coordonee)
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);


				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (FS_INPUT i) : SV_Target
			{
				float4 tex = tex2D(_MainTex, i.uv*1.5 -0.25);

				fixed4 col;
				fixed4 gauge_view;
				fixed x = (-0.5 + i.uv.x) * 2;
				fixed y = (-0.5 + i.uv.y) * 2;

				fixed radius = 1 - sqrt(x*x + y*y);
				radius = 1 - length(float2(x,y));

				float outCircle = radius;
				outCircle = smoothstep(1 - _BackgroundCutoff, 1 - _BackgroundCutoff + _Smoothness, outCircle);

				float inCircle = radius;
				inCircle = smoothstep(1 - _ForegroundCutoff, 1 - _ForegroundCutoff + _Smoothness, inCircle);

				float2 uvCircular = i.uv;
				uvCircular = uvCircular * 2 - 1;

				float gauge = atan2(-uvCircular.x, -uvCircular.y) / PI * 0.5 + 0.5;
				gauge = smoothstep(_Gauge1, _Gauge1 + _Smoothness, gauge);
				gauge = 1 - gauge;

				gauge *= (outCircle - inCircle);
				
				
				col.rgb = _ForegroundColor.rgb;
				col.rgb = lerp(col.rgb, tex.rgb, tex.a); ;
				col.a = (outCircle - inCircle) * gauge;
				col.a *= _ForegroundColor.a;
				col.a = max(col.a, tex.a);
				clip(col.a - 0.5f);
				

				return col;
			}
			ENDCG
		}
	}
}