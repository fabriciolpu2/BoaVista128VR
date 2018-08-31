// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "uSky/uSkymap" 
{
Properties {
	[HideInInspector] CloudSampler ("Cloud RenderTexture", 2D) = "black" {}
}
SubShader 
	{
		Tags {"RenderType"="Opaque"}
//		ZTest Always
		ZWrite Off  
//		Fog { Mode Off }

		CGINCLUDE
		#include "UnityCG.cginc"
		
		struct v2f 
		{
			float4	pos 		: SV_POSITION;
			fixed3	color		: COLOR;
			float2	texcoord 	: TEXCOORD0;
			float2	cloudTC 	: TEXCOORD1;
		};
		
		// x = sunset, y = day, z = cloud opacity
		uniform half3 _SkyMultiplier;
		
		sampler2D CloudSampler;
		
		uniform half2	_colorCorrection;
		uniform half3	_betaR, _betaM, _miePhase_g,_mieConst; 
		uniform half4	_NightHorizonColor,_NightZenithColor;
		uniform float3	_SunDir;

		v2f vert(appdata_base v)
		{
			v2f OUT;
			OUT.pos = UnityObjectToClipPos(v.vertex);
			OUT.texcoord = (v.texcoord.xy-0.5)*2.2;	
			OUT.cloudTC = float2(v.texcoord.x, 1 - v.texcoord.y) ;
			return OUT;
		}
		
		half4 frag(v2f IN) : SV_Target
		{
			float2 u = IN.texcoord;
			float l = dot(u, u);

			// inverse stereographic projection,
			// from skymap coordinates to world space directions
			float3 r = float3(2.0 * u, 1.0 - l) / (1.0 + l);
			
			r.z = max(8e-2, r.z);
			half rZ = r.z;
			
			float cosTheta = dot( r.xzy, _SunDir); 
			half cosine = cosTheta;
			
			// optical depth
			half sR = 8.0 / rZ ;
			half sM = 1.2 / rZ ;

			// gradient
			half3 gr = _NightZenithColor.xyz * sR;
			gr *= (2 - gr);
			
			// sky color
			half3 extinction = exp(-( _betaR * sR + _betaM * sM ));
			
			half3 rayleigh = lerp( extinction * gr, 1 - extinction, _SkyMultiplier.x );
			half3 mie = rayleigh * sM / rayleigh.r * _mieConst.xyz;

			// scattering phase
			half miePhase =  _miePhase_g.x * pow( _miePhase_g.y - _miePhase_g.z * cosine, -1.5 );

			half3 inScatter = ( rayleigh * 0.75 + mie * miePhase ); 
			inScatter *= ( 1.0 + cosine * cosine ) * _SkyMultiplier.y ; // little brigther looks better?
				
			// night sky horizontal gradient
			inScatter += _NightHorizonColor.rgb * gr;

// --------------------------------------------------------------------------------		

			// color correction
			inScatter = pow(inScatter * _colorCorrection.x, _colorCorrection.y);
						
			// cloud texture
			half4 cloudTex = tex2D( CloudSampler, IN.cloudTC.xy );
			
			inScatter += (cloudTex.rgb  * _SkyMultiplier.z); 


			return half4(inScatter, 1); 
		}
		ENDCG
// --------------------------------------------------------------------------------			
	Pass{	
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma target 3.0
			ENDCG
    	}
	}
	
	Fallback Off
}