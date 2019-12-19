// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/HurtShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Value("Value", Float) = 0
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
Blend One OneMinusSrcAlpha
					Pass
					{
					CGPROGRAM
						#pragma vertex vert
						#pragma fragment frag
						#pragma multi_compile _ PIXELSNAP_ON
						#include "UnityCG.cginc"

						struct appdata_t
						{
							float4 vertex   : POSITION;
							float4 color    : COLOR;
							float2 texcoord : TEXCOORD0;
						};

						struct v2f
						{
							float4 vertex   : SV_POSITION;
							fixed4 color : COLOR;
							half2 texcoord  : TEXCOORD0;
						};

						fixed4 _Color;
						float _Value;

						v2f vert(appdata_t IN)
						{
							v2f OUT;
							OUT.vertex = UnityObjectToClipPos(IN.vertex);
							OUT.texcoord = IN.texcoord;
							OUT.color = IN.color* _Color;
							#ifdef PIXELSNAP_ON
							OUT.vertex = UnityPixelSnap(OUT.vertex);
							#endif

							return OUT;
						}

						sampler2D _MainTex;

						fixed4 frag(v2f IN) : SV_Target
						{
							fixed4 col = tex2D(_MainTex, IN.texcoord);
							col.rgb = lerp(col.rgb, _Color.rgb, _Value);
							col.rgb *= col.a;
							
							return col;
						}
					ENDCG
					}
		}

}
