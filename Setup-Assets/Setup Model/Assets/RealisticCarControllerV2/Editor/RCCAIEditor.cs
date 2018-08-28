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

[CustomEditor(typeof(RCCAICarController))]
public class RCCAIEditor : Editor {

	RCCAICarController aiController;

	Texture2D AIIcon;

	void Awake () {

		AIIcon = Resources.Load("AIIcon", typeof(Texture2D)) as Texture2D;
	
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/AI Controller/Add AI Controller To Vehicle")]
	static void CreateAIBehavior(){

		if(!Selection.activeGameObject.GetComponent<RCCAICarController>() && Selection.activeGameObject.GetComponent<RCCCarControllerV2>()){
			Selection.activeGameObject.AddComponent<RCCAICarController>();
		}else if(Selection.activeGameObject.GetComponent<RCCCarControllerV2>()){	
			EditorUtility.DisplayDialog("Your Vehicle Already Has AI Car Controller", "Your Vehicle Already Has AI Car Controller", "Ok");
		}else if(!Selection.activeGameObject.GetComponent<RCCCarControllerV2>()){
			EditorUtility.DisplayDialog("Your Vehicle Has Not RCCCarControllerV2", "Your Vehicle Has Not RCCCarControllerV2.", "Ok");
		}

	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/AI Controller/Add AI Controller To Vehicle", true)]
	static bool CheckAIBehavior() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/AI Controller/Add Waypoints Container To Scene")]
	static void CreateWaypointsContainer(){
		
		if(GameObject.FindObjectOfType<RCCAIWaypointsContainer>() == null){
			
			GameObject wp = new GameObject("Waypoints Container");
			wp.transform.position = Vector3.zero;
			wp.transform.rotation = Quaternion.identity;
			wp.AddComponent<RCCAIWaypointsContainer>();
			Selection.activeGameObject = wp;
			
		}else{
			EditorUtility.DisplayDialog("Your Scene Already Has Waypoints Container", "Your Scene Already Has Waypoints Container", "Ok");
		}
		
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/AI Controller/Add Waypoints Container To Scene", true)]
	static bool CheckWaypointsContainer() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/AI Controller/Add BrakeZones Container To Scene")]
	static void CreateBrakeZonesContainer(){
		
		if(GameObject.FindObjectOfType<RCCAIBrakeZonesContainer>() == null){
			
			GameObject bz = new GameObject("Brake Zones Container");
			bz.transform.position = Vector3.zero;
			bz.transform.rotation = Quaternion.identity;
			bz.AddComponent<RCCAIBrakeZonesContainer>();
			Selection.activeGameObject = bz;
			
		}else{
			EditorUtility.DisplayDialog("Your Scene Already Has Brake Zones Container", "Your Scene Already Has Brake Zones", "Ok");
		}
		
	}

	[MenuItem("Tools/BoneCracker Games/Realistic Car Controller/AI Controller/Add BrakeZones Container To Scene", true)]
	static bool CheckBrakeZonesContainer() {
		if(Selection.gameObjects.Length > 1 || !Selection.activeTransform)
			return false;
		else
			return true;
	}

	public override void OnInspectorGUI () {

		serializedObject.Update();

		aiController = (RCCAICarController)target;

		if(!aiController.gameObject.GetComponent<RCCCarControllerV2>().AIController)
			aiController.gameObject.GetComponent<RCCCarControllerV2>().AIController = true;

		if(aiController.gameObject.GetComponent<RCCCarControllerV2>().canEngineStall)
			aiController.gameObject.GetComponent<RCCCarControllerV2>().canEngineStall = false;

		if(!aiController.gameObject.GetComponent<RCCCarControllerV2>().autoReverse)
			aiController.gameObject.GetComponent<RCCCarControllerV2>().autoReverse = true;

		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Box(AIIcon, GUILayout.ExpandWidth(true));
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();

		EditorGUILayout.PropertyField(serializedObject.FindProperty("obstacleLayers"), new GUIContent("Obstacle Layers", "Obstacle Layers For Avoid Dynamic Objects."), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("wideRayLength"), new GUIContent("Wide Ray Distance", "Wide Rays For Avoid Dynamic Objects."), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("tightRayLength"), new GUIContent("Tight Ray Distance", "Tight Rays For Avoid Dynamic Objects."), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("sideRayLength"), new GUIContent("Side Ray Distance", "Side Rays For Avoid Dynamic Objects."), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("limitSpeed"), new GUIContent("Limit Speed", "Limits The Speed."), false);

		if(aiController.limitSpeed)
			EditorGUILayout.Slider(serializedObject.FindProperty("maximumSpeed"), 0f, aiController.GetComponent<RCCCarControllerV2>().maxspeed);

		EditorGUILayout.PropertyField(serializedObject.FindProperty("smoothedSteer"), new GUIContent("Smooth Steering", "Smooth Steering."), false);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("nextWaypointPassRadius"), new GUIContent("Next Waypoint Pass Radius", "If car gets closer then this radius, goes to next waypoint."), false);

		EditorGUILayout.Separator();
		EditorGUILayout.LabelField("Current Waypoint: ", aiController.currentWaypoint.ToString());
		EditorGUILayout.LabelField("Laps: ", aiController.lap.ToString());
		EditorGUILayout.LabelField("Total Waypoints Passed: ", aiController.totalWaypointPassed.ToString());
		EditorGUILayout.LabelField("Ignoring Waypoint Due To Unexpected Obstacle: ", aiController.ignoreWaypointNow.ToString());
		EditorGUILayout.Separator();

		serializedObject.ApplyModifiedProperties();
	
	}

}
