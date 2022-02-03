﻿Shader "Custom/Stencil Writer"
{	
	Properties
	{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		_Smoothness("Smoothness", Range(0, 1)) = 0
		_Metallic("Metallic", Range(0, 1)) = 0
		[HDR] _Emission("Emission", color) = (0,0,0)
		[IntRange] _StencilRef("Stencil Reference", Range(0,255)) = 1
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Transparent" }

		Pass
		{			
			ColorMask 0			
			ZWrite Off
			ZTest Always

			Stencil
			{
				Ref [_StencilRef]
				Comp Always
				Pass Replace
			}
		}

		CGPROGRAM

		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;

		half _Smoothness;
		half _Metallic;
		half3 _Emission;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input i, inout SurfaceOutputStandard o)
		{
			fixed4 col = tex2D(_MainTex, i.uv_MainTex);

			col *= _Color;

			o.Albedo = col.rgb;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Emission = _Emission;
		}

		ENDCG
	}

	FallBack "Standard"
}