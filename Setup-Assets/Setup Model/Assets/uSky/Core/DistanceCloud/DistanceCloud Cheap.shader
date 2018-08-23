// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// TODO: This shader get too dim when using the tonemapping

Shader "uSky/DistanceCloud Cheap (Mobile)" {
Properties {

    [RangeSlider] Attenuation ("Attenuation", Range(0,1)) = 0.6
//    [RangeSlider] StepSize ("Step size", Range(0.001,0.01)) = 0.004
    [RangeSlider] AlphaSaturation("Alpha saturation", Range(1,10)) = 2.0
    [RangeSlider] SunColorMultiplier ("Sun Color multiplier", Range(0,8)) = 4
    [RangeSlider] SkyColorMultiplier("Sky Color multiplier", Range(0,8)) = 1.5
	[KeywordEnum(Rectangular,Polar)] USKY_MAPPING ("Mapping mode", Float) = 0	
	CloudSampler ("Texture (R)", 2D) = "white" {}
	
	// "Opacity mask" is for blocking the color behind the cloud, useful at night time.
	// if using "Opacity mask", then requires cloud texture's channel "g" for masking.
	// if not using "Opacity mask", then set the "Opacity mask" to 0.0 will gain some performance.
	[RangeSlider] Mask ("Opacity mask (G)", Range (0,1)) = 0.0
	// Range 0 ~ 360 for non-animated Rotation
    [RangeSlider] RotateSpeed("Rotate speed", Range (-1,1)) = 0.0 
//	[Toggle] USKY_MASK ("Use Opacity mask?", Float) = 0

// make sure following value store in the material if it is not focusing
[HideInInspector] ShadeColorFromSun ("ShadeColorFromSun",Vector) = (1,0.87,0.73,1)
[HideInInspector] ShadeColorFromSky ("ShadeColorFromSky",Vector) = (0.58,0.7,0.86,1)
}
SubShader {
Tags { "Queue"="Geometry+502" "RenderType"="Background" }

Blend  SrcAlpha OneMinusSrcAlpha

 Zwrite Off  

Pass {
		Name "BASE"
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#include "UnityCG.cginc"
		#pragma multi_compile USKY_MAPPING_RECTANGULAR USKY_MAPPING_POLAR
//		#pragma multi_compile USKY_MASK_OFF USKY_MASK_ON
		
		sampler2D CloudSampler;
		uniform float3 _SunDir; 
		float RotateSpeed;
		fixed Attenuation, AlphaSaturation, SunColorMultiplier, SkyColorMultiplier;
		fixed Mask;
		uniform fixed3 ShadeColorFromSun, ShadeColorFromSky;
		float4 CloudSampler_ST;
		
		struct appdata_t {
			float4	vertex		: POSITION;
    		float2  rectangular	: TEXCOORD0;
    		float2  polar		: TEXCOORD1;
		};
		
		struct v2f {
		    float4	pos 	: SV_POSITION;
		    fixed3	color	: COLOR;
		    half2	baseTC	: TEXCOORD0;
		};

		float4 RotateAroundYInDegrees (float4 vertex, float degrees)
		{
			float alpha = degrees * 3.1416 / 180.0;
			float sina, cosa;
			sincos(alpha, sina, cosa);
			float2x2 m = float2x2(cosa, -sina, sina, cosa);
			return float4(mul(m, vertex.xz), vertex.yw).xzyw;
		}
		
		v2f vert (appdata_t v)
		{
		    v2f o;

		    float3 t = RotateAroundYInDegrees(v.vertex, RotateSpeed *_Time.y + unity_DeltaTime.z).xyz; //  animate rotation 
		    t = t + _WorldSpaceCameraPos.xyz ; 
//		    t = t * _ProjectionParams.z + _WorldSpaceCameraPos.xyz ;
			o.pos = UnityObjectToClipPos (float4(t,1));
//			o.pos.z = o.pos.w; // avoid front face clipping?

			#ifdef USKY_MAPPING_RECTANGULAR
				o.baseTC = TRANSFORM_TEX (v.rectangular, CloudSampler);
			#else
				o.baseTC = v.polar ;
			#endif

			o.color = lerp( SkyColorMultiplier * ShadeColorFromSky.xyz, SunColorMultiplier * ShadeColorFromSun.xyz, Attenuation);
			
		    return o;
		}

		fixed4 frag (v2f i) : SV_Target
		{
			// r = cloud density , g = Opacity mask
			fixed2 opacity = tex2D( CloudSampler, i.baseTC.xy ).rg;
			
			// Alternative for better performance: just tweak the Mask in texture channel "G" directly,
			// and read the opacity.g directly without using "lerp function" between 2 channels.
			// so alpha will be:  a = pow( opacity.g, AlphaSaturation );
//			#ifdef USKY_MASK_ON 
			if (Mask > 0.02)
				opacity.r = lerp(opacity.r, opacity.g, Mask); 
//			#endif
					
//			fixed c = ( Attenuation ); // can use seperate texture channel "B" to define differet cloud density for sunset and night time color

			// Same as Alternative above, if tweak texture channel "R" directly witout using "pow function" will gain the performance
			fixed a = pow( opacity.r, AlphaSaturation );  
//			fixed3 col = lerp( SkyColorMultiplier * ShadeColorFromSky.xyz, SunColorMultiplier * ShadeColorFromSun.xyz, c );

			fixed4 Color = fixed4( i.color, a ) ;
			
			return Color;

		}
		ENDCG
    }
}
//Fallback "VertexLit"
} 