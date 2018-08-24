//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RCCMainFixedCam : MonoBehaviour {
	
	private Transform[] camPositions;
	private RCCChildFixedCam[] childCams;
	private float[] childDistances;
	private RCCCamManager switcher;
	private float distance;
	internal Transform player;
	internal Camera mainCamera;
	public bool canTrackNow = false;
	public float minimumFOV = 20f;
	public float maximumFOV = 60f;
	public bool drawGizmos = true;

	void Start(){

		StartCoroutine(GetVariables());

	}

	IEnumerator GetVariables () {

		yield return new WaitForSeconds(1);

		childCams = GetComponentsInChildren<RCCChildFixedCam>();
		switcher = GameObject.FindObjectOfType<RCCCamManager>();

		foreach(RCCChildFixedCam l in childCams){
			l.enabled = false;
			l.player = player;
		}

		camPositions = new Transform[childCams.Length];
		childDistances = new float[childCams.Length];

		for(int i = 0; i < camPositions.Length; i ++){
			camPositions[i] = childCams[i].transform;
			childDistances[i] = childCams[i].distance;
		}

	}

	void Act(){

		foreach(RCCChildFixedCam l in childCams){
			l.enabled = false;
			l.player = player;
		}

	}

	void Update(){

		if(!player)
			return;

		if(canTrackNow)
			Tracking ();

		foreach(RCCChildFixedCam l in childCams){
			if(l.player != player)
				l.player = player;
		}

	}

	void Tracking () {
	
		for(int i = 0; i < camPositions.Length; i ++){

			distance = Vector3.Distance(camPositions[i].position, player.transform.position);
			
			if(distance <= childDistances[i]){

				if(childCams[i].enabled != true)
					childCams[i].enabled = true;

				if(mainCamera.transform.parent != childCams[i].transform){
					mainCamera.transform.SetParent(childCams[i].transform);
					mainCamera.transform.localPosition = Vector3.zero;
					mainCamera.transform.localRotation = Quaternion.identity;
				}

				mainCamera.fieldOfView = Mathf.Lerp (mainCamera.fieldOfView, Mathf.Lerp (maximumFOV, minimumFOV, ((Vector3.Distance(mainCamera.transform.position, player.transform.position) * 2f) / childDistances[i])), Time.deltaTime * 3f);

			}else{

				if(childCams[i].enabled != false)
					childCams[i].enabled = false;

				if(mainCamera.transform.parent == childCams[i].transform){
					mainCamera.transform.SetParent(null);
					childCams[i].transform.rotation = Quaternion.identity;
					switcher.cameraChangeCount = 10;
					switcher.ChangeCamera();
					canTrackNow = false;
				}

			}

		}

	}

	void OnDrawGizmos() {

		if(drawGizmos){

			childCams = GetComponentsInChildren<RCCChildFixedCam>();
			camPositions = new Transform[childCams.Length];
			childDistances = new float[childCams.Length];

			for(int i = 0; i < camPositions.Length; i ++){
				camPositions[i] = childCams[i].transform;
				childDistances[i] = childCams[i].distance;
				Gizmos.matrix = camPositions[i].transform.localToWorldMatrix;
				Gizmos.color = new Color(1f, 0f, 0f, .5f);
				Gizmos.DrawCube(Vector3.zero,new Vector3(childDistances[i] * 2f, childDistances[i] / 2f, childDistances[i] * 2f));
			}

		}

	}
	
}
