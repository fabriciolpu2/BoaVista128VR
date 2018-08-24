//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCCamManager: MonoBehaviour {

	private RCCCarCamera carCamera;
	private RCCCameraOrbit orbitScript;
	private RCCCockpitCamera cockpitCamera;
	private RCCWheelCamera wheelCamera;

	public bool useOrbitCamera = false;
	public bool useFixedCamera = false;

	internal float dist = 10f;
	internal float height = 5f;
	internal int cameraChangeCount = 0;
	internal Transform target;

	void Start () {

		cameraChangeCount = 0;

		carCamera = GetComponent<RCCCarCamera>();
		target = carCamera.playerCar;

		if(GetComponent<RCCCameraOrbit>())
			orbitScript = GetComponent<RCCCameraOrbit>();
		else
			orbitScript = gameObject.AddComponent<RCCCameraOrbit>();

	}

	void Update () {

		if(Input.GetKeyDown(KeyCode.C))
			ChangeCamera();

	}

	public void ChangeCamera(){

		target = carCamera.playerCar;

		if(!target)
			return;

		cameraChangeCount++;
		if(cameraChangeCount >= 5)
			cameraChangeCount = 0;

		if(target.GetComponent<RCCCarCameraConfig>()){
			dist = target.GetComponent<RCCCarCameraConfig>().distance;
			height = target.GetComponent<RCCCarCameraConfig>().height;
			carCamera.distance = dist;
			carCamera.height = height;
		}

		if(useOrbitCamera){
			orbitScript.target = target.transform;
			orbitScript.distance = dist;
		}

		if(target.GetComponentInChildren<RCCCockpitCamera>())
			cockpitCamera = target.GetComponentInChildren<RCCCockpitCamera>();
		if(target.GetComponentInChildren<RCCWheelCamera>())
			wheelCamera = target.GetComponentInChildren<RCCWheelCamera>();

		switch(cameraChangeCount){

		case 0:
			if(useFixedCamera){
				if(GameObject.FindObjectOfType<RCCMainFixedCam>())
					GameObject.FindObjectOfType<RCCMainFixedCam>().canTrackNow = false;
			}
			carCamera.enabled = true;
			orbitScript.enabled = false;
			carCamera.transform.SetParent(null);
			break;
		case 1:
			if(!useOrbitCamera){
				ChangeCamera();
				break;
			}
			orbitScript.enabled = true;
			carCamera.enabled = false;
			carCamera.transform.SetParent(null);
			break;
		case 2:
			if(!cockpitCamera){
				ChangeCamera();
				break;
			}
			orbitScript.enabled = false;
			carCamera.enabled = false;
			carCamera.transform.SetParent(cockpitCamera.transform);
			carCamera.transform.localPosition = Vector3.zero;
			carCamera.transform.localRotation = Quaternion.identity;
			carCamera.GetComponent<Camera>().fieldOfView = 60;
			break;
		case 3:
			if(!wheelCamera){
				ChangeCamera();
				break;
			}
			orbitScript.enabled = false;
			carCamera.enabled = false;
			carCamera.transform.SetParent(wheelCamera.transform);
			carCamera.transform.localPosition = Vector3.zero;
			carCamera.transform.localRotation = Quaternion.identity;
			carCamera.GetComponent<Camera>().fieldOfView = 60; 
			break;
		case 4:
			if(!useFixedCamera){
				ChangeCamera();
				break;
			}
			orbitScript.enabled = false;
			carCamera.enabled = false;
			carCamera.transform.SetParent(null);
			GameObject.FindObjectOfType<RCCMainFixedCam>().mainCamera = GetComponent<Camera>();
			GameObject.FindObjectOfType<RCCMainFixedCam>().player = target;
			GameObject.FindObjectOfType<RCCMainFixedCam>().canTrackNow = true;
			break;
		}

	}

}
