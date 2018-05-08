// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Dixit/MainMenu/Profile/AchievementV2"
{
    Properties
    {
		_MainTex ("Texture", 2D) = "white" {}
		_MainTexStarBackground ("Texture", 2D) = "white" {}
		_MainTexGaugeEmpty ("Texture", 2D) = "white" {}
		_MainTexGaugeFilled ("Texture", 2D) = "white" {}
		_Gauge ("Gauge", Range(0,1)) = 0.5
		_ForegroundColor("Foreground Color", Color) = (1,1,1,1)
		_ForegroundCutoff("Foreground Cutoff", Range(0,1)) = 0.5
		_BackgroundCutoff("Background Cutoff", Range(0,1)) = 0.5

		_Smoothness("Smoothness", Range(0,1)) = 0

        //[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
			
			#define PI 3.14159265359
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MainTexStarBackground;
			float4 _MainTexStarBackground_ST;
			sampler2D _MainTexGaugeEmpty;
			float4 _MainTexGaugeEmpty_ST;
			sampler2D _MainTexGaugeFilled;
			float4 _MainTexGaugeFilled_ST;

			uniform float _Gauge;
			fixed4 _ForegroundColor;
			half _ForegroundCutoff;
			half _BackgroundCutoff;

			uniform float _Smoothness;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                //OUT.texcoord = v.texcoord;
				OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTexGaugeEmpty);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTexGaugeFilled);

                OUT.color = v.color * _Color;
                return OUT;
            }


            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTexGaugeEmpty, IN.texcoord));
                half4 color2 = (tex2D(_MainTexGaugeFilled, IN.texcoord));
				
				float4 tex = tex2D(_MainTex, IN.texcoord*1.2f-0.1);
				float4 texStar = tex2D(_MainTexStarBackground,IN.texcoord);

				fixed x = (-0.5 + IN.texcoord.x) * 2;
				fixed y = (-0.5 + IN.texcoord.y) * 2;
				
				fixed radius = 1 - sqrt(x*x + y*y);
				radius = 1 - length(float2(x,y));


				float2 uvCircular =IN.texcoord;
				uvCircular = uvCircular * 2 - 1;

				float gauge = atan2(-uvCircular.x, -uvCircular.y) / PI * 0.5 + 0.5;
				//gauge += sin((IN.texcoord.x + _Time.y) * 3.14 * 5) * 0.0025;
				//gauge += sin(_Time.y * 3.14 * 2) * 0.005;
				gauge = smoothstep(_Gauge, _Gauge + _Smoothness, gauge);
				gauge = 1 - gauge;
				
				color.rgb = lerp(color.rgb, texStar.rgb, texStar.a);
				color.rgb = lerp(color.rgb, color2.rgb, gauge);
				color.rgb = lerp(color.rgb, tex.rgb, tex.a);
				color.a = max(color2.a, tex.a);
				//color.a = max(tex.a, texStar.a);


                #ifdef UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
                #endif

                return color;
            }
        ENDCG
        }
    }
}
