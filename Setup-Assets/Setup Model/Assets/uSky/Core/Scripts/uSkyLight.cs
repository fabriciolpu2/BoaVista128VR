using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[AddComponentMenu("uSky/uSky Light")]
[RequireComponent (typeof (uSkyManager))]
public class uSkyLight : MonoBehaviour {
	
	private uSkyManager m_uSM;
	
	[Range(0f,4f)]
	public float SunIntensity = 1.0f;
	
	public Gradient LightColor = new Gradient();
	
	public bool EnableMoonLighting = true;
	
	[Range(0f,2f)]
	public float MoonIntensity = 0.4f;
	
	[Header("Ambient")]
	[Range(0f,1f)]
	public float AmbientIntensity = 1.0f;

	public Gradient SkyColor = new Gradient();
	#if UNITY_5
	public Gradient EquatorColor = new Gradient();
	public Gradient GroundColor = new Gradient();
	#endif
	
	private float currentTime;
	private float dayTime;
	private float nightTime;
	
	protected uSkyManager uSM {
		get{
			if (m_uSM == null) {
				m_uSM = this.gameObject.GetComponent<uSkyManager>();
				if (m_uSM == null)
					Debug.Log(" Can't not find uSkyManager");
			}
			return m_uSM;
		}
	}
	// check sun light
	protected GameObject sunLight{
		get{
			if(uSM != null){
				return (uSM.m_sunLight != null)? uSM.m_sunLight : null;
			}else 
				return null;
		}
	}
	// check moon light
	protected GameObject moonLight{
		get{
			if(uSM != null){
				return (uSM.m_moonLight != null)? uSM.m_moonLight : null;
			}else 
				return null;
		}
	}

	// Gradient Alpha Key for all Gradient setting. In general alpha value is not using at all
	private GradientAlphaKey[] gak {
		get{
			GradientAlphaKey[] _gak = new GradientAlphaKey[2];
			_gak [0].alpha = 1f;
			_gak [0].time = 0f;
			_gak [1].alpha = 1f;
			_gak [1].time = 1f;
			return _gak;
		}
	}
	void SetLightColorKey(){
		
		GradientColorKey[] lck = new GradientColorKey[7];
		
		lck [0].color = new Color (0.22f, 0.26f, 0.3f, 1f);
		lck [0].time = 0.23f;
		lck [1].color = new Color (0.96f, 0.678f, 0.33f, 1f);
		lck [1].time = 0.26f;
		lck [2].color = new Color (0.976f, 0.816f, 0.565f, 1f);
		lck [2].time = 0.32f;
		lck [3].color = new Color (0.984f, 0.871f, 0.729f, 1f);
		lck [3].time = 0.50f;
		lck [4].color = new Color (0.976f, 0.816f, 0.565f, 1f);
		lck [4].time = 0.68f;
		lck [5].color = new Color (0.96f, 0.678f, 0.33f, 1f);
		lck [5].time = 0.74f;
		lck [6].color = new Color (0.22f, 0.26f, 0.3f, 1f);
		lck [6].time = 0.77f;
		
		LightColor.SetKeys (lck, gak);
	}
	
	void SetSkyColorKey (){
		GradientColorKey[] sck = new GradientColorKey[6];
		sck [0].color = new Color (0.11f, 0.125f, 0.157f, 1f);
		sck [0].time = 0.225f;
		sck [1].color = new Color (0.215f, 0.255f, 0.247f, 1f);
		sck [1].time = 0.25f;
		sck [2].color = new Color (0.58f, 0.7f, 0.86f, 1f);
		sck [2].time = 0.28f;
		sck [3].color = new Color (0.58f, 0.7f, 0.86f, 1f);
		sck [3].time = 0.72f;
		sck [4].color = new Color (0.215f, 0.255f, 0.247f, 1f);
		sck [4].time = 0.75f;
		sck [5].color = new Color (0.11f, 0.125f, 0.157f, 1f);
		sck [5].time = 0.775f;
		
		SkyColor.SetKeys (sck, gak);
	}
	#if UNITY_5
	void SetEquatorColorKey (){
		GradientColorKey[] eck = new GradientColorKey[6];
		eck [0].color = new Color (0.08f, 0.098f, 0.14f, 1f);
		eck [0].time = 0.225f;
		eck [1].color = new Color (0.38f, 0.32f, 0.098f, 1f);
		eck [1].time = 0.25f;
		eck [2].color = new Color (0.4f, 0.54f, 0.66f, 1f);
		eck [2].time = 0.28f;
		eck [3].color = new Color (0.4f, 0.54f, 0.66f, 1f);
		eck [3].time = 0.72f;
		eck [4].color = new Color (0.38f, 0.32f, 0.098f, 1f);
		eck [4].time = 0.75f;
		eck [5].color = new Color (0.08f, 0.098f, 0.14f, 1f);
		eck [5].time = 0.775f;
	
		EquatorColor.SetKeys (eck, gak);
	}
	
	void SetGroundColorKey (){
		GradientColorKey[] gck = new GradientColorKey[4];
		gck [0].color = new Color (0.08f, 0.08f, 0.08f, 1f);
		gck [0].time = 0.21f;
		gck [1].color = new Color (0.2f, 0.2f, 0.2f, 1f);
		gck [1].time = 0.25f;
		gck [2].color = new Color (0.2f, 0.2f, 0.2f, 1f);
		gck [2].time = 0.75f;
		gck [3].color = new Color (0.08f, 0.08f, 0.08f, 1f);
		gck [3].time = 0.79f;

		GroundColor.SetKeys (gck, gak);
	}
	#endif
	
	void OnEnable (){
		// Check: if the gradient has only white color (blank), then load the predefined gradient key setting.
		if (LightColor.Evaluate (0f).Equals (Color.white) && LightColor.Evaluate (0.5f).Equals (Color.white) && LightColor.Evaluate (1f).Equals (Color.white))
			SetLightColorKey();
		if (SkyColor.Evaluate (0f).Equals (Color.white) && SkyColor.Evaluate (0.5f).Equals (Color.white) && SkyColor.Evaluate (1f).Equals (Color.white))
			SetSkyColorKey ();
		#if UNITY_5
		if (EquatorColor.Evaluate (0f).Equals (Color.white) && EquatorColor.Evaluate (0.5f).Equals (Color.white) && EquatorColor.Evaluate (1f).Equals (Color.white))
			SetEquatorColorKey ();
		if (GroundColor.Evaluate (0f).Equals (Color.white) && GroundColor.Evaluate (0.5f).Equals (Color.white) && GroundColor.Evaluate (1f).Equals (Color.white))
			SetGroundColorKey ();
		#endif
	}
	
	void Start () {
		if (uSM != null){
			InitUpdate ();
		}
	}
	
	void Update (){
		if (uSM != null) {
			if (uSM.SkyUpdate) {
				InitUpdate ();
				
			}
		}
	}
	
	void InitUpdate (){
		SunAndMoonLightUpdate ();
		#if UNITY_5
		if (RenderSettings.ambientMode == AmbientMode.Trilight)
			AmbientGradientUpdate ();
		else
			if (RenderSettings.ambientMode == AmbientMode.Flat)
				RenderSettings.ambientLight = CurrentSkyColor;
		RenderSettings.ambientIntensity = AmbientIntensity;
		#else
			AmbientUpdateU4 (); // Unity 4 ambient
		#endif
	}
	
	void SunAndMoonLightUpdate (){
		currentTime = uSM.Timeline01;
		dayTime = uSM.DayTime;
		nightTime = uSM.NightTime;

		// TODO: clean up and optimize this codes
		if (sunLight != null)
		if (sunLight.GetComponent<Light> () != null) {
			sunLight.GetComponent<Light> ().intensity = uSM.Exposure * SunIntensity * dayTime;
			sunLight.GetComponent<Light> ().color = CurrentLightColor * dayTime;
			// enable at Day, disable at Night
			if ( currentTime < 0.24f || currentTime > 0.76f )
				sunLight.GetComponent<Light> ().enabled = false;
			else 
				sunLight.GetComponent<Light> ().enabled = true;
		}
		if ( moonLight != null) {
			if (moonLight.GetComponent<Light> () != null) {
				moonLight.GetComponent<Light> ().intensity =  uSM.Exposure * MoonIntensity * nightTime;
				moonLight.GetComponent<Light> ().color = CurrentLightColor * nightTime;
				// enable at Night, disable at Day
				if ( currentTime > 0.26f && currentTime < 0.74f || EnableMoonLighting == false)
					moonLight.GetComponent<Light> ().enabled = false;
				else if(EnableMoonLighting)
					moonLight.GetComponent<Light> ().enabled = true;
			}
		}
	}
	
	public Color CurrentLightColor{
		get{
			return LightColor.Evaluate (currentTime);
		}
	}
	
	public Color CurrentSkyColor{
		get{
			return SkyColor.Evaluate (currentTime) * uSM.Exposure ;
		}
	}

	// For unity 4 ambient light function
	void AmbientUpdateU4 (){
		RenderSettings.ambientLight = CurrentSkyColor * AmbientIntensity;
	}

	#if UNITY_5
	void AmbientGradientUpdate (){

		RenderSettings.ambientSkyColor = CurrentSkyColor;
		RenderSettings.ambientEquatorColor = CurrentEquatorColor;
		RenderSettings.ambientGroundColor = CurrentGroundColor;
	}
	
	public Color CurrentEquatorColor{
		get{
			return EquatorColor.Evaluate (currentTime) * uSM.Exposure;
		}
	}
	public Color CurrentGroundColor{
		get{
			return GroundColor.Evaluate (currentTime) * uSM.Exposure;
		}
	}
	#endif
}
