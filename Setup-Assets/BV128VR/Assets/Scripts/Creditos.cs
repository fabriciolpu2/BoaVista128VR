using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Creditos : MonoBehaviour {
	public GameObject uiObject;
	public GameObject credito;
	public Material SkyOrla;
	public Material SkyCreditos;

	// Use this for initialization
	void Start () {
		credito.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void reset(){
		credito.SetActive (false);
		uiObject.SetActive (true);
		RenderSettings.skybox = SkyOrla;

	}

	public void creditos () {
		print("Creditos");
		uiObject.SetActive(false);	
		credito.SetActive (true);
		RenderSettings.skybox = SkyCreditos;
	}

}
