// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Dixit/MainMenu/Profile/Stats"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Gauge1 ("Gauge 1", Range(0,1)) = 0.5
		_Gauge1ColorFilled ("Gauge 1 Color Filled", Color) = (0,0,0,0)
		_Gauge1ColorEmpty ("Gauge 1 Color Empty", Color) = (0,0,0,0)
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
			uniform float4 _Gauge1ColorFilled;
			uniform float4 _Gauge1ColorEmpty;
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

				// sample the texture
				fixed4 col;
				float gauge1 = i.uv.y * 0.333 + 0.666;				
				gauge1= step(_Gauge1, gauge1);
				gauge1 = 1-gauge1;
				col = tex2D(_MainTex, i.uv);
				float alpha = col.a;
				col = gauge1;
				col = _Gauge1ColorEmpty;
				col = lerp(col, _Gauge1ColorFilled, gauge1);
				col -= alpha*0.15;
				//col.y = ceil(i.uv.x-0.33);
				//col.z = ceil(i.uv.x-0.66);
				//col.x = clamp(col.y,col.x,col.z);
				//col.x = 0.9*sin(_Time.y * 3.14)*0.5+0.5;
				//col.y = 0.7;
				//col.z =0.7;

				//col.y = i.uv.x;
				//col.x = 0;
				return col;
			}
			ENDCG
		}
	}
}
