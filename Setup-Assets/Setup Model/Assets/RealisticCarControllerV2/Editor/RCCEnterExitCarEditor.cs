//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;

public class RCCEnterExitCarEditor : Editor {
	
	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Enter-Exit/Add Enter-Exit Script to Vehicle")]
	static void CreateEnterExitVehicleBehavior(){

		GameObject[] selectedGameObjects = Selection.gameObjects;

		for(int i = 0; i < selectedGameObjects.Length; i++){
		
			if(!selectedGameObjects[i].GetComponent<RCCEnterExitCar>() && selectedGameObjects[i].GetComponent<RCCCarControllerV2>()){
				selectedGameObjects[i].AddComponent<RCCEnterExitCar>();
			}else if(selectedGameObjects[i].GetComponent<RCCCarControllerV2>()){	
				EditorUtility.DisplayDialog("Your Vehicle Already Has Enter-Exit Script", "Your Vehicle Named " + "''" + selectedGameObjects[i].name + "''"  + " Already Has Enter-Exit Script", "Ok");
			}else if(!selectedGameObjects[i].GetComponent<RCCCarControllerV2>()){
				EditorUtility.DisplayDialog("Your Vehicle Has Not RCCCarControllerV2", "Your Vehicle Named " + "''" + selectedGameObjects[i].name + "''"  + " Has Not RCCCarControllerV2.", "Ok");
			}

		}
		
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Enter-Exit/Add Enter-Exit Script to Vehicle", true)]
	static bool CheckEnterExitVehicleBehavior() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Enter-Exit/Add Enter-Exit Script to FPS Player")]
	static void CreateEnterExitPlayerBehavior(){

		GameObject[] selectedGameObjects = Selection.gameObjects;

		for(int i = 0; i < selectedGameObjects.Length; i++){

			if(!selectedGameObjects[i].GetComponentInChildren<RCCEnterExitPlayer>()){
				if(selectedGameObjects[i].GetComponentInChildren<Camera>() == null){
					EditorUtility.DisplayDialog("Your Player Named " +  "''" + selectedGameObjects[i].name + "''" + " Has Not Any Camera", "Your Player Has Not Any Camera", "Ok");
					return;
				}
				Camera cam = selectedGameObjects[i].GetComponentInChildren<Camera>();
				if(cam.gameObject.GetComponent<RCCEnterExitPlayer>())
					EditorUtility.DisplayDialog("Your Player Already Has Enter-Exit Script", "Your Player Named " + "''" + selectedGameObjects[i].name + "''" + " Already Has Enter-Exit Script", "Ok");
				else
					cam.gameObject.AddComponent<RCCEnterExitPlayer>();
			}else{
				EditorUtility.DisplayDialog("Your Player Already Has Enter-Exit Script", "Your Player Named " + "''" + selectedGameObjects[i].name + "''" + " Already Has Enter-Exit Script", "Ok");
			}

		}		

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/Enter-Exit/Add Enter-Exit Script to FPS Player", true)]
	static bool CheckEnterExitPlayerBehavior() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}
	
}
