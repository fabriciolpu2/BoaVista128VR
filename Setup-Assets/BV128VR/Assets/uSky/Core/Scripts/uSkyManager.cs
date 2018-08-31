using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
[AddComponentMenu("uSky/uSky Manager")]
public class uSkyManager : MonoBehaviour 
{

	public bool SkyUpdate = true; // TODO: Update mode : Off, All_Settings, Timeline_Only
//	public bool useSlider = true;
	[Range(0.0f, 24.0f)]
	public float Timeline = 17.0f;
	[Range(0.0f, 360.0f)]
	public float Longitude = 0.0f;

	[Space(10)]
//	[Header("Day Sky")]
	[Range(0.0f, 5.0f)]
	public float Exposure = 1.0f;
	[Range(0.0f, 5.0f)]
	public float RayleighScattering = 1.0f;
	[Range(0.0f, 5.0f)]
	public float MieScattering = 1.0f;
	[Range (0.0f,0.9995f)]
	public float SunAnisotropyFactor = 0.76f;
	[Range (1e-3f,10.0f)]
	public float SunSize = 1.0f;
	
	// Wavelengths for visible light ray from 380 to 780 
	public Vector3 Wavelengths = new Vector3(680f, 550f, 440f); // sea level mie

	public Color SkyTint = new Color(0.5f, 0.5f, 0.5f, 1f);
	public Color GroundColor = new Color(0.369f, 0.349f, 0.341f, 1f);
	public GameObject m_sunLight;

	[Space (10)]
//	[Header ("Night Sky")]
	public bool EnableNightSky = true; // TODO: Night Sky Mode: Off, Night Sky only (No star No Space cubemap), Enable All
//	public Color NightZenithColor = new Color(0.29f,0.42f,0.58f,1f);
	public Gradient NightZenithColor = new Gradient ();
	public Color NightHorizonColor = new Color(0.43f,0.47f,0.5f,1f);

	[Range(0.0f, 5.0f)]
	public float StarIntensity = 1.0f;
	[Range(0.0f, 2.0f)]
	public float OuterSpaceIntensity = 0.25f;

	public Color MoonInnerCorona = new Color(1f, 1f, 1f, 0.5f);
	public Color MoonOuterCorona = new Color(0.25f,0.39f,0.5f,0.5f);

	[Range(0.0f, 1.0f)]
	public float MoonSize = 0.15f;
	public GameObject m_moonLight;
	public Material SkyboxMaterial;
	public bool AutoApplySkybox = true;

	[HideInInspector][SerializeField]
	public bool LinearSpace; //  Auto Detection
	public bool Tonemapping = false; // TODO : Auto Detect Main Camera?

	private Vector3 euler;
	private Matrix4x4 moon_wtl;

	private StarField Stars ;
	private Mesh starsMesh;
	private Material m_starMaterial;


	// NOTE: "Stars.Shader" need to be placed in Resources folder for mobile build!
	protected Material starMaterial {
		get {
			if (m_starMaterial == null) {
				m_starMaterial = new Material(Shader.Find("Hidden/uSky/Stars"));
				m_starMaterial.hideFlags = HideFlags.DontSave;
			}
			return m_starMaterial;
		} 
	}

	protected void InitStarsMesh (){
		Stars = new StarField();
		starsMesh = Stars.InitializeStarfield();
		starsMesh.hideFlags = HideFlags.DontSave;
		if (starsMesh == null)
			Debug.Log(" Can't find or read <b>StarsData.bytes</b> file.");
	}

	protected void OnEnable() {
		if(m_sunLight == null)
			Debug.Log("Please apply the <b>Directional Light</b> to uSkyManager");
//		if (SkyboxMaterial == null)
//			Debug.Log("Please apply the <b>Skybox Material</b> to uSkyManager");

		if (NightZenithColor.Evaluate (0f).Equals (Color.white) && NightZenithColor.Evaluate (0.5f).Equals (Color.white) && NightZenithColor.Evaluate (1f).Equals (Color.white))
			SetNightZenithColorKey ();
			
		if (EnableNightSky && starsMesh == null) {
			InitStarsMesh ();
		}
		#if UNITY_EDITOR
			detectColorSpace ();
		#else
			InitMaterial (SkyboxMaterial);
		#endif
	}
	
	protected void OnDisable() {
		if(starsMesh) {
			DestroyImmediate(starsMesh);
		}
		if (m_starMaterial){
			DestroyImmediate(m_starMaterial);
		}
	}

	private void detectColorSpace (){
//			LinearSpace = QualitySettings.activeColorSpace == ColorSpace.Linear;
		#if UNITY_EDITOR
			LinearSpace = UnityEditor.PlayerSettings.colorSpace == ColorSpace.Linear;		// Editor
		#endif
		#if UNITY_IPHONE || UNITY_ANDROID
			LinearSpace = false; // Gamma only on mobile
		#endif
		if( SkyboxMaterial != null )
		InitMaterial (SkyboxMaterial);
	}

	private void Start() 
	{
//		if(useSlider)
			InitSun();

		if(SkyboxMaterial != null ){
			InitMaterial(SkyboxMaterial);
			if (AutoApplySkybox)
				ApplySkybox(SkyboxMaterial);
		}
//		if (EnableNightSky && starsMesh == null) {
//			InitStarsMesh ();
//		}
		if (EnableNightSky && starsMesh != null)
			starTab ();
	}

//	private void OnLevelWasLoaded(int level) {
//		if(useSlider)
//			InitSun();
//		if(SkyboxMaterial){
//			InitMaterial(SkyboxMaterial);
//			if (AutoAsignSkyBox)
//				ApplySkybox(SkyboxMaterial);
//		}
//	}

	private void ApplySkybox(Material mat){
		RenderSettings.skybox = mat;
	}

	public float Timeline01 {
		get{
			return Timeline / 24;
		}
	}

	void Update()
	{
		if (SkyUpdate) {
			// reset Timeline slider
			if (Timeline >= 24.0f)
				Timeline = 0.0f;

			// Update every frame for all shader Paramaters
			if (SkyboxMaterial != null) {
//				if(useSlider)
				InitSun ();
				InitMaterial (SkyboxMaterial);
			}
		}

		#if UNITY_EDITOR
		if (AutoApplySkybox && RenderSettings.skybox != SkyboxMaterial)
			ApplySkybox(SkyboxMaterial);
		if (EnableNightSky)
		{
//			if (starsMesh == null)
//				InitStarsMesh ();
			starTab ();
		}
			detectColorSpace ();
		#endif

		// Draw Star field
		if (EnableNightSky && starsMesh != null && starMaterial != null && SunDir.y < 0.2f)
			Graphics.DrawMesh (starsMesh, Vector3.zero, Quaternion.identity, starMaterial, 0 );
	}

	// rotate and align the sun direction with Timeline slider
	void InitSun()
	{
		euler.x = Timeline * 360.0f / 24.0f - 90.0f;
		euler.y = Longitude;
		if(m_sunLight != null)
			m_sunLight.transform.localEulerAngles = euler;
	}

	void InitMaterial(Material mat)
	{
		mat.SetVector ("_SunDir", SunDir); 
		mat.SetMatrix ("_Moon_wtl", getMoonMatrix);
		
		mat.SetVector ("_betaR", BetaR);
		mat.SetVector ("_betaM", BetaM);

		// x = Sunset, y = Day, z = Night 
		mat.SetVector ("_SkyMultiplier", skyMultiplier);

		mat.SetFloat ("_SunSize", 32.0f / SunSize);
		mat.SetVector ("_mieConst", mieConst);
		mat.SetVector ("_miePhase_g", miePhase_g);
		mat.SetVector ("_GroundColor", bottomTint);
		mat.SetVector ("_NightHorizonColor", getNightHorizonColor);
		mat.SetVector ("_NightZenithColor", getNightZenithColor);
		mat.SetVector ("_MoonInnerCorona", getMoonInnerCorona);
		mat.SetVector ("_MoonOuterCorona", getMoonOuterCorona); 
		mat.SetFloat ("_MoonSize", MoonSize);
		mat.SetVector ("_colorCorrection", ColorCorrection);

		mat.shaderKeywords = hdrMode.ToArray ();

		if (EnableNightSky)
			mat.DisableKeyword("NIGHTSKY_OFF");
		else
			mat.EnableKeyword("NIGHTSKY_OFF");

		mat.SetFloat ("_OuterSpaceIntensity", OuterSpaceIntensity);
		if(starMaterial != null )
		starMaterial.SetFloat ("StarIntensity", starBrightness);

	}

	public Vector3 SunDir {
		get {
			return (m_sunLight != null)? m_sunLight.transform.forward * -1: new Vector3(0.321f,0.766f,-0.557f);
		}
	}

	private Matrix4x4 getMoonMatrix {
		get {
			if (m_moonLight == null) {
					// predefined Moon Direction
					moon_wtl = Matrix4x4.TRS (Vector3.zero, new Quaternion (-0.9238795f, 8.817204e-08f, 8.817204e-08f, 0.3826835f), Vector3.one);
			} else if (m_moonLight != null) {
					moon_wtl = m_moonLight.transform.worldToLocalMatrix;
					moon_wtl.SetColumn (2, Vector4.Scale (new Vector4 (1, 1, 1, -1), moon_wtl.GetColumn (2)));
			}
			return moon_wtl;
		}
	}
//	public Vector3 MoonDir {
//		get {
//			return getMoonMatrix.GetColumn(2);
//		}
//	}
	
	private Vector3 VariableRangeWavelengths {
		get { 
			return new Vector3 (Mathf.Lerp (Wavelengths.x - 150, Wavelengths.x + 150, 1 - (SkyTint.r)),
			                    Mathf.Lerp (Wavelengths.y - 150, Wavelengths.y + 150, 1 - (SkyTint.g)),
			                    Mathf.Lerp (Wavelengths.z - 150, Wavelengths.z + 150, 1 - (SkyTint.b)));
		}
	}

	public Vector3 BetaR{
		get {
			// Evaluate Beta Rayleigh function based on A.J.Preetham

			Vector3 WL = VariableRangeWavelengths * 1e-9f;

			const float Km = 1000.0f;
			const float n = 1.0003f;		// the index of refraction of air
			const float N = 2.545e25f;		// molecular density at sea level
			const float pn = 0.035f;		// depolatization factor for standard air

			Vector3 waveLength4 = new Vector3 (Mathf.Pow (WL.x, 4), Mathf.Pow (WL.y, 4), Mathf.Pow (WL.z, 4));
			Vector3 theta = 3.0f * N * waveLength4 * (6.0f - 7.0f * pn);
			float ray = (8 * Mathf.Pow (Mathf.PI, 3) * Mathf.Pow (n * n - 1.0f, 2) * (6.0f + 3.0f * pn));
			return Km * new Vector3 (ray / theta.x, ray / theta.y, ray / theta.z) * Mathf.Max (1e-3f, RayleighScattering);
		}
	}
	
	public Vector3 BetaM{
		get {
			// Beta Mie (simplified) function based on Cryengine
			return new Vector3 (Mathf.Pow (Wavelengths.x, -0.84f), Mathf.Pow (Wavelengths.y, -0.84f), Mathf.Pow (Wavelengths.z, -0.84f));
		}
	}
	// 0 ~ 2.0
	public float uMuS {
		get {
			// Sun fall ratio function based on Eric Bruneton 
			return Mathf.Atan (Mathf.Max (SunDir.y, -0.1975f) * 5.35f) / 1.1f + 0.74f;
		}
	}
	// 0 ~ 1.0
	public float DayTime {
		get {
			return Mathf.Min (1, uMuS);
		}
	}

	public float NightTime {
		get {
			return 1 - DayTime;
		}
	}

	public Vector3 miePhase_g {
		get{
			// partial mie phase : approximated with the Cornette Shanks phase function
			float g2 = SunAnisotropyFactor * SunAnisotropyFactor;
			float cs = LinearSpace && Tonemapping? 2f : 1.25f;
			return new Vector3 ( cs * ((1.0f - g2) / (2.0f + g2)), 1.0f + g2, 2.0f * SunAnisotropyFactor);
		}
	}
	public Vector3 mieConst {
		get {
			return new Vector3 (1f, BetaR.x/ BetaR.y, BetaR.x/ BetaR.z) * 4e-3f * (MieScattering);// / Mathf.Max (1.0f, RayleighScattering)); 
		}
	}

	// x = Sunset, y = Day, z = Night
	public Vector3 skyMultiplier {
		get{
			return new Vector3 (Mathf.Clamp01 ((uMuS - 1.0f) * (2.0f / Mathf.Pow(RayleighScattering, 2.5f))),
			                    Exposure * 4 * DayTime, NightTime) ;// * Mathf.Pow(RayleighScattering,0.5f);
		}
	}

	private Vector3 bottomTint{
		get {
			float cs = LinearSpace ? 1e-2f : 2e-2f;
			return new Vector3 (BetaR.x / (GroundColor.r * cs ),
			                    BetaR.y / (GroundColor.g * cs ),
			                    BetaR.z / (GroundColor.b * cs ));
		}
	}

	public Vector2 ColorCorrection {
		get{
			return (LinearSpace && Tonemapping) ? new Vector2 (0.38317f, 1.413f): // (0.5f, 1.5f) :
				// using 2.0 instead of 2.2
				LinearSpace ? new Vector2 (1f, 2.0f) : Vector2.one; 
		}
	}

	public Color getNightHorizonColor{
		get{
			return NightHorizonColor * NightTime;
		}
	}

	public Color getNightZenithColor{
		get{
			return NightZenithColor.Evaluate(Timeline01) * 1e-2f;
		}
	}
	
	private void SetNightZenithColorKey (){
		GradientColorKey[] nzck = new GradientColorKey[4];
		nzck [0].color = new Color (0.196f,0.28f,0.39f,1f);
		nzck [0].time = 0.225f;
		nzck [1].color = new Color (0.29f, 0.42f,0.58f,1f);
		nzck [1].time = 0.25f;
		nzck [2].color = new Color (0.29f, 0.42f,0.58f,1f);
		nzck [2].time = 0.75f;
		nzck [3].color = new Color (0.196f,0.28f,0.39f,1f);
		nzck [3].time = 0.775f;
		
		GradientAlphaKey[] nzak = new GradientAlphaKey[2];
		nzak [0].alpha = 1f;
		nzak [0].time = 0f;
		nzak [1].alpha = 1f;
		nzak [1].time = 1f;
		
		NightZenithColor.SetKeys (nzck, nzak);
	}

	private Vector4 getMoonInnerCorona {
		get {
			return new Vector4 (MoonInnerCorona.r * NightTime,
					            MoonInnerCorona.g * NightTime,
					            MoonInnerCorona.b * NightTime,
			                    4e2f / MoonInnerCorona.a);
		}
	}

	private Vector4 getMoonOuterCorona {
		get {
			float cs = LinearSpace?  Tonemapping ? 16f : 12f: 8f;
			return new Vector4 (MoonOuterCorona.r * 0.25f * NightTime,
			                    MoonOuterCorona.g * 0.25f * NightTime,
			                    MoonOuterCorona.b * 0.25f * NightTime,
			                    cs / MoonOuterCorona.a); 
		}
	}

	private List<string> hdrMode {
		get {
			return new List<string> { Tonemapping ? "USKY_HDR_ON" : "USKY_HDR_OFF" };
		}
	}

	// Stars shader setting
	private float starBrightness {
		get {
			float cs = LinearSpace ? 0.5f : 1.5f;
			return StarIntensity * NightTime * cs;
		}
	}
	protected static readonly Vector2[] tab = 
	{
		new Vector2(0.897907815f,-0.347608525f),new Vector2(0.550299290f, 0.273586675f),
		new Vector2(0.823885965f, 0.098853070f),new Vector2(0.922739035f,-0.122108860f),
		new Vector2(0.800630175f,-0.088956800f),new Vector2(0.711673375f, 0.158864420f),
		new Vector2(0.870537795f, 0.085484560f),new Vector2(0.956022355f,-0.058114540f)
	};
	
	void starTab (){
			
		if (starMaterial != null)
		for (int i = 0; i < 8; i++) {
			string tabArray = "_tab" + i;
			starMaterial.SetVector(tabArray,tab[i]);
		}
	}


}

