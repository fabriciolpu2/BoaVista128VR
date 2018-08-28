//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System;
using System.Collections;

public class RCCCarChange : MonoBehaviour {
	
	private GameObject[] objects;
	private GameObject activeObject;
	private int activeObjectIdx;
	private Camera mainCamera;
	private bool selectScreen = true;
	
	public Vector3 cameraOffset = new Vector3(10f, -70f, 0f);

	void Awake () {

		RCCCarControllerV2[] vehicles = GameObject.FindObjectsOfType<RCCCarControllerV2>();
		objects = new GameObject[vehicles.Length];

		for(int i = 0; i < vehicles.Length; i++){
			objects[i] = vehicles[i].gameObject;
		}

		foreach(GameObject controller in objects){
			controller.GetComponent<RCCCarControllerV2>().canControl = false;
			controller.GetComponent<RCCCarControllerV2>().runEngineAtAwake = false;
			controller.GetComponent<RCCCarControllerV2>().engineRunning = false;
		}

		mainCamera = GameObject.FindObjectOfType<RCCCarCamera>().GetComponent<Camera>();

	}

	void Update () {

		if(selectScreen){
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, objects[activeObjectIdx].transform.position + (-mainCamera.transform.forward * 10f) + new Vector3(0f, .5f, 0f), Time.deltaTime * 5f);
			mainCamera.transform.rotation = Quaternion.Euler(cameraOffset);
			GetComponent<Camera>().fieldOfView = 50;
		}

	}
	
	void OnGUI()
	{

		if(selectScreen){

			GUIStyle centeredStyle = GUI.skin.GetStyle("Button");
			centeredStyle.alignment = TextAnchor.MiddleCenter;

			// Next
			if( GUI.Button(new Rect(Screen.width/2 + 65, 100, 120, 50), "Next") )
			{
				activeObjectIdx++;
				if( activeObjectIdx >= objects.Length )
					activeObjectIdx = 0;
			}	
			
			// Previous
			if( GUI.Button(new Rect(Screen.width / 2 - 185, 100, 120, 50), "Previous") )
			{
				activeObjectIdx--;
				if( activeObjectIdx < 0 )
					activeObjectIdx = objects.Length - 1;
			}

			// Select Car
			if( GUI.Button(new Rect(Screen.width / 2 - 60, 100, 120, 50), "Select") )
			{
				selectScreen = false;
				objects[activeObjectIdx].GetComponent<RCCCarControllerV2>().canControl = true;
				objects[activeObjectIdx].GetComponent<RCCCarControllerV2>().KillOrStartEngine();
				GetComponent<RCCCamManager>().enabled = true;
				GetComponent<RCCCarCamera>().playerCar = objects[activeObjectIdx].transform;
				GetComponent<RCCCamManager>().cameraChangeCount = 5;
				GetComponent<RCCCamManager>().ChangeCamera();

				//Delete Comment Lines If You Gonna Use NGUI Buttons. Be Sure You Have Imported Latest NGUI Package.
//				if(GameObject.FindObjectOfType<RCCNGUIDashboardButton>()){
//					RCCNGUIDashboardButton[] dButtons = GameObject.FindObjectsOfType<RCCNGUIDashboardButton>();
//					for(int i = 0; i < dButtons.Length; i ++){
//						dButtons[i].Check();
//					}
//				}

				if(GameObject.FindObjectOfType<RCCUIDashboardButton>()){
					RCCUIDashboardButton[] dButtons = GameObject.FindObjectsOfType<RCCUIDashboardButton>();
					for(int i = 0; i < dButtons.Length; i ++){
						dButtons[i].Check();
					}
				}
			}

		}else{

			if( GUI.Button(new Rect(Screen.width / 2 - 120 , 100, 240, 50), "Select Screen") )
			{
				selectScreen = true;
				objects[activeObjectIdx].GetComponent<RCCCarControllerV2>().canControl = false;
				objects[activeObjectIdx].GetComponent<RCCCarControllerV2>().engineRunning = false;
				GetComponent<RCCCamManager>().cameraChangeCount = 10;
				GetComponent<RCCCamManager>().ChangeCamera();
				GetComponent<RCCCamManager>().enabled = false;
				GetComponent<RCCCarCamera>().enabled = false;
				GetComponent<RCCCameraOrbit>().enabled = false;
				mainCamera.transform.rotation = Quaternion.Euler(cameraOffset);
				GetComponent<Camera>().fieldOfView = 50;
			}

		}

	}

}
