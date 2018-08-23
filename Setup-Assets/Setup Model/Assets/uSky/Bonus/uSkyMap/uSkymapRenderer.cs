using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
[AddComponentMenu("uSky/uSkymap Renderer")]
public class uSkymapRenderer : MonoBehaviour {

	private RenderTexture m_skyMap;
	public Material m_skymapMaterial;
	public Material m_oceanMaterial;
	public int SkymapResolution = 256; // should be power of 2 : 64, 128, 256, 512. Default is 256 (should be good enough)
	[Range (0f,2f)]
	public float SkyReflection = 1f;
	[Range (0f,2f)]
	public float CloudReflection = 1f;
	public float CloudTextureScale = 590;
//	public bool DebugCloudRT = false;

	public bool DebugSkymap = false; // debug use, for Editor only

	int m_frameCount = 0;

	private uSkyManager m_uSM;
	private DistanceCloud m_DC;

	//cloud
	private GameObject m_cloudCamera;
	private RenderTexture m_cloudRT;
	private bool RenderCloud;

	protected uSkyManager uSM {
		get{
			if (m_uSM == null) {
				m_uSM = this.gameObject.GetComponent<uSkyManager>();
				if (m_uSM == null)
					Debug.Log("Can't not find uSkyManager");
			}
			return m_uSM;
		}
	}
	// need get the cloud layer value from DistanceCloud component
	private DistanceCloud DC{
		get{
			if (m_DC == null) {
				m_DC = this.gameObject.GetComponent<DistanceCloud>();
				if (m_DC == null)
					Debug.Log("Can't not find DistanceCloud");
			}
			return m_DC;
		}
	}

	void Start () {
		if (!SystemInfo.supportsRenderTextures) {
			Debug.LogWarning("RenderTexture is not supported with your Graphic Card");
			return;
		}
		if (uSM == null) {
			Debug.Log ("Can NOT find uSkyManager, Please asign this uSkymapRenderer script to uSkyManager gameObject.");
			enabled = false;
			return;
		}

		m_skyMap = new RenderTexture(SkymapResolution, SkymapResolution, 0, RenderTextureFormat.ARGBHalf);
		m_skyMap.filterMode = FilterMode.Trilinear;
		m_skyMap.wrapMode = TextureWrapMode.Clamp;
		m_skyMap.anisoLevel = 1; // 9
//		m_skyMap.useMipMap = true; // reflection seems fade to horizon better, faster without mip
		m_skyMap.Create();

		if (m_skymapMaterial != null){
			InitMaterial (m_skymapMaterial);
			Graphics.Blit (null, m_skyMap, m_skymapMaterial);
		}

		if (m_oceanMaterial != null) {
			m_oceanMaterial.SetTexture ("_SkyMap", m_skyMap);

			// This is a constant value that requires for sun reflection on the ocean material
			m_oceanMaterial.SetVector("EARTH_POS", new Vector3(0.0f, 6360010.0f, 0.0f));

			updateOceanMaterial (m_oceanMaterial);
		}

		// Cloud ----------------------------------------------------------------------

			RenderCloud = (DC != null)? true : false;

		if (RenderCloud ) {
			m_cloudRT = new RenderTexture (SkymapResolution, SkymapResolution, 0, RenderTextureFormat.ARGBHalf);
			m_cloudRT.filterMode = FilterMode.Trilinear;
			m_cloudRT.wrapMode = TextureWrapMode.Clamp;
			m_cloudRT.anisoLevel = 1;
			m_cloudRT.Create ();

			if (m_cloudCamera == null)
				m_cloudCamera = new GameObject ("cloudCamera", typeof(Camera));

			m_cloudCamera.hideFlags = HideFlags.HideInHierarchy;
			m_cloudCamera.transform.Rotate (new Vector3 (270, 0, 0)); // facing upward
			m_cloudCamera.GetComponent<Camera>().orthographic = true;
			m_cloudCamera.GetComponent<Camera>().orthographicSize = CloudTextureScale;
			m_cloudCamera.GetComponent<Camera>().aspect = 1f;
			m_cloudCamera.GetComponent<Camera>().backgroundColor = Color.black;
			m_cloudCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
			m_cloudCamera.GetComponent<Camera>().cullingMask &= 1 << DC.cloudLayer;
//			m_cloudCamera.camera.cullingMask ^= 1 << LayerMask.NameToLayer ("Ignore Raycast");
//			m_cloudCamera.camera.hdr = true ;
			m_cloudCamera.GetComponent<Camera>().targetTexture = m_cloudRT;

			m_skymapMaterial.SetTexture ("CloudSampler", m_cloudRT);
		}
	}
	/*	
	public bool IsCreated()
	{
		//Sometimes Unity will mark render textures as not created when they have been
		//This will check for that. These textures are the important ones.
		if(!m_skyMap.IsCreated()) return false;
		
		return true;
	}
*/
	
	void Update () {

		if (m_skyMap != null && m_skymapMaterial != null) {

			//These are work arounds for some bugs in Unity 4.0 - 4.2. If your running this in a later version they may have been fixed??
			//In a Unity dx9 build graphics blit does not seam to have any effect on the first frame.
			//The sky object uses graphics blit to initilize some render textures.
			//Call init() to do this but it must be called on the second frame. Strange.
			//This does not seem to be needed in a dx11 build
			if (m_frameCount == 1 )
				InitMaterial (m_skymapMaterial);
			
			m_frameCount++;
			
			Graphics.Blit (null, m_skyMap, m_skymapMaterial);

			InitMaterial (m_skymapMaterial); 

			if (m_oceanMaterial != null)
				updateOceanMaterial (m_oceanMaterial);

			#if UNITY_EDITOR
			m_cloudCamera.GetComponent<Camera>().orthographicSize = CloudTextureScale;
			#endif 
		}
	}

	void InitMaterial (Material mat){

		if (uSM != null) {

			mat.SetVector ("_SunDir", uSM.SunDir); 

			mat.SetVector ("_betaR", uSM.BetaR);
			mat.SetVector ("_betaM", uSM.BetaM);

			mat.SetVector ("_SkyMultiplier", new Vector3(uSM.skyMultiplier.x, uSM.skyMultiplier.y * SkyReflection, CloudReflection));
			mat.SetVector ("_mieConst", uSM.mieConst);
			mat.SetVector ("_miePhase_g", uSM.miePhase_g);

//			Vector4 st = uSM.skyTint;
//			st.w = Mathf.Max (uSM.SunAnisotropyFactor,st.w); // avoid the uSkymap artifact when the density goes too low
//			mat.SetVector ("_SkyTint", st);

			mat.SetVector ("_NightHorizonColor", uSM.getNightHorizonColor * SkyReflection);
			mat.SetVector ("_NightZenithColor", uSM.getNightZenithColor * SkyReflection);

			mat.SetVector ("_colorCorrection", uSM.ColorCorrection);
			              		
		}
	}
	
	void updateOceanMaterial (Material mat){
		// Extra ocean parameter
		mat.SetVector ("SUN_DIR", uSM.SunDir);

		// This is for sun reflection on the ocean
		mat.SetFloat ("SUN_INTENSITY", 100 * uSM.Exposure);
	}
	
	void OnDestroy()
	{
		m_skyMap.Release ();
		if (m_cloudRT != null )
			m_cloudRT.Release ();
	}

	#if UNITY_EDITOR
	void OnGUI(){
		if(DebugSkymap)
			GUI.DrawTexture(new Rect(Screen.width - SkymapResolution ,0 ,SkymapResolution, SkymapResolution), m_skyMap);
	}
	#endif
}
