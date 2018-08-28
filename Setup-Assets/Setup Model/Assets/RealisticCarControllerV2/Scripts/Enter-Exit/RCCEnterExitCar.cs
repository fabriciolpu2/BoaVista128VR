//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCEnterExitCar : MonoBehaviour {

	private GameObject carCamera;
	private GameObject player;
	private GameObject dashboard;
	public Transform getOutPosition;

	private bool  opened = false;
	private float waitTime = 1f;
	private bool  temp = false;
	
	void Awake (){

		carCamera = GameObject.FindObjectOfType<RCCCarCamera>().gameObject;
		carCamera.GetComponent<Camera>().enabled = false;
		carCamera.GetComponent<AudioListener>().enabled = false;

		GetComponent<RCCCarControllerV2>().runEngineAtAwake = false;
		GetComponent<RCCCarControllerV2>().canControl = false;
		GetComponent<RCCCarControllerV2>().engineRunning = false;

		if(carCamera.GetComponent<RCCCarCamera>())
			carCamera.GetComponent<RCCCarCamera>().enabled = true;
	
		if(GameObject.FindObjectOfType<RCCDashboardInputs>())
			dashboard = GameObject.FindObjectOfType<RCCDashboardInputs>().gameObject;

		if(!getOutPosition){
			GameObject getOutPos = new GameObject("Get Out Position");
			getOutPos.transform.SetParent(transform);
			getOutPos.transform.localPosition = new Vector3(-3f, 0f, 0f);
			getOutPos.transform.localRotation = Quaternion.identity;
			getOutPosition = getOutPos.transform;
		}

		if(GetComponent<RCCCarCameraConfig>())
			GetComponent<RCCCarCameraConfig>().enabled = false;

	}

	void Start(){

		if(dashboard)
			StartCoroutine("DisableDashboard");

	}

	IEnumerator DisableDashboard(){

		yield return new WaitForFixedUpdate();
		dashboard.SetActive(false);

	}
	
	void Update (){

		if((Input.GetKeyDown(KeyCode.E)) && opened && !temp){
			GetOut();
			opened = false;
			temp = false;
		}

	}
	
	IEnumerator Act (GameObject fpsplayer){

		player = fpsplayer;

		if (!opened && !temp){
			GetIn();
			opened = true;
			temp = true;
			yield return new WaitForSeconds(waitTime);
			temp = false;
		}

	}
	
	void GetIn (){

		if(carCamera.GetComponent<RCCCamManager>()){
			carCamera.GetComponent<RCCCamManager>().cameraChangeCount = 10;
			carCamera.GetComponent<RCCCamManager>().ChangeCamera();
		}
		carCamera.transform.GetComponent<RCCCarCamera>().playerCar = transform;
		player.transform.SetParent(transform);
		player.transform.localPosition = Vector3.zero;
		player.transform.localRotation = Quaternion.identity;
		player.SetActive(false);
		carCamera.GetComponent<Camera>().enabled = true;
		if(GetComponent<RCCCarCameraConfig>())
			GetComponent<RCCCarCameraConfig>().enabled = true;
		GetComponent<RCCCarControllerV2>().canControl = true;
		if(dashboard)
			dashboard.SetActive(true);
		carCamera.GetComponent<AudioListener>().enabled = true;
		SendMessage("StartEngine");
		Cursor.lockState = CursorLockMode.None;
	}
	
	void GetOut (){        
		player.transform.SetParent(null);
		player.transform.position = getOutPosition.position;
		player.transform.rotation = getOutPosition.rotation;
		player.SetActive(true);
		carCamera.GetComponent<Camera>().enabled = false;
		if(GetComponent<RCCCarCameraConfig>())
			GetComponent<RCCCarCameraConfig>().enabled = false;
		carCamera.GetComponent<AudioListener>().enabled = false;
		GetComponent<RCCCarControllerV2>().canControl = false;
		GetComponent<RCCCarControllerV2>().engineRunning = false;
		if(dashboard)
			dashboard.SetActive(false);
		Cursor.lockState = CursorLockMode.Locked;

	}
	
}