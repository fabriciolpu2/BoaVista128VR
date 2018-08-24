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
using System.Collections.Generic;

public class RCCAICarController : MonoBehaviour {

	private RCCCarControllerV2 carController;
	private Rigidbody rigid;
	
	// Waypoint Container.
	private RCCAIWaypointsContainer waypointsContainer;
	public int currentWaypoint = 0;
	
	// Raycast distances.
	public LayerMask obstacleLayers = -1;
	public int wideRayLength = 20;
	public int tightRayLength = 20;
	public int sideRayLength = 3;
	private float newInputSteer = 0f;
	private bool  raycasting = false;
	private float resetTime = 0f; 
	
	// Steer, motor, and brake inputs.
	private float steerInput = 0f;
	private float motorInput = 0f;
	private float brakeInput = 0f;

	public bool limitSpeed = false;
	public float maximumSpeed = 100f;

	public bool smoothedSteer = true;
	
	// Brake Zone.
	private float maximumSpeedInBrakeZone = 0f;
	private bool inBrakeZone = false;
	
	// Counts laps and how many waypoints passed.
	public int lap = 0;
	public int totalWaypointPassed = 0;
	public int nextWaypointPassRadius = 40;
	public bool ignoreWaypointNow = false;
	
	// Unity's Navigator.
	private UnityEngine.AI.NavMeshAgent navigator;
	private GameObject navigatorObject;

	void Awake() {

		if(!GetComponent<RCCCarControllerV2>().AIController)
			GetComponent<RCCCarControllerV2>().AIController = true;
		
		if(GetComponent<RCCCarControllerV2>().canEngineStall)
			GetComponent<RCCCarControllerV2>().canEngineStall = false;
		
		if(!GetComponent<RCCCarControllerV2>().autoReverse)
			GetComponent<RCCCarControllerV2>().autoReverse = true;

	}

	void Start (){

		carController = GetComponent<RCCCarControllerV2>();
		rigid = GetComponent<Rigidbody>();

		waypointsContainer = FindObjectOfType(typeof(RCCAIWaypointsContainer)) as RCCAIWaypointsContainer;
		
		navigatorObject = new GameObject("Navigator");
		navigatorObject.transform.parent = transform;
		navigatorObject.transform.localPosition = Vector3.zero;
		navigatorObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
		navigatorObject.GetComponent<UnityEngine.AI.NavMeshAgent>().radius = 1;
		navigatorObject.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 1f;
		navigatorObject.GetComponent<UnityEngine.AI.NavMeshAgent>().height = 1;
		navigatorObject.GetComponent<UnityEngine.AI.NavMeshAgent>().avoidancePriority = 99;
		navigator = navigatorObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
		
	}
	
	void Update(){
		
		navigator.transform.localPosition = new Vector3(0, carController.FrontLeftWheelCollider.transform.localPosition.y, carController.FrontLeftWheelCollider.transform.localPosition.z);
		
	}
	
	void  FixedUpdate (){

		if(!carController.canControl)
			return;

		Navigation();
		FixedRaycasts();
		ApplyTorques();
		Resetting();

	}
	
	void Navigation (){
		
		if(!waypointsContainer){
			Debug.LogError("Waypoints Container Couldn't Found!");
			return;
		}
		if(waypointsContainer && waypointsContainer.waypoints.Count < 1){
			Debug.LogError("Waypoints Container Doesn't Have Any Waypoints!");
			return;
		}
		
		// Next waypoint's position.
		Vector3 nextWaypointPosition = transform.InverseTransformPoint( new Vector3(waypointsContainer.waypoints[currentWaypoint].position.x, transform.position.y, waypointsContainer.waypoints[currentWaypoint].position.z));
		float navigatorInput = Mathf.Clamp(transform.InverseTransformDirection(navigator.desiredVelocity).x * 1.5f, -1f, 1f);

		navigator.SetDestination(waypointsContainer.waypoints[currentWaypoint].position);

		//Steering Input.
		if(!carController.reverseGear){
			if(!ignoreWaypointNow)
				steerInput = Mathf.Clamp((navigatorInput + newInputSteer), -1f, 1f);
			else
				steerInput = Mathf.Clamp(newInputSteer, -1f, 1f);
		}else{
			steerInput = Mathf.Clamp((-navigatorInput - newInputSteer), -1f, 1f);
		}
		
		if(!inBrakeZone){
			if(carController.speed >= 25){
				brakeInput = Mathf.Lerp(0f, .25f, (Mathf.Abs(steerInput)));
			}else{
				brakeInput = 0f;
			}
		}else{
			brakeInput = Mathf.Lerp(0f, 1f, (carController.speed - maximumSpeedInBrakeZone) / maximumSpeedInBrakeZone);
		}

		if(!inBrakeZone){
			if(carController.speed >= 10){
				if(!carController.changingGear)
					motorInput = Mathf.Clamp(1f - Mathf.Abs(navigatorInput), .5f, 1f) - (Mathf.Abs(newInputSteer) / 2f);
				else
					motorInput = 0f;
			}else{
				if(!carController.changingGear)
					motorInput = 1f;
				else
					motorInput = 0f;
			}
		}else{
			if(!carController.changingGear)
				motorInput = Mathf.Lerp(1f, 0f, (carController.speed) / maximumSpeedInBrakeZone);
			else
				motorInput = 0f;

		}
		
		// Checks for the distance to next waypoint. If it is less than written value, then pass to next waypoint.
		if ( nextWaypointPosition.magnitude < nextWaypointPassRadius ) {
			currentWaypoint ++;
			totalWaypointPassed ++;
			
			// If all waypoints are passed, sets the current waypoint to first waypoint and increase lap.
			if ( currentWaypoint >= waypointsContainer.waypoints.Count ) {
				currentWaypoint = 0;
				lap ++;
			}
		}
		
	}
	
	void Resetting (){
		
		if(carController.speed <= 15 && transform.InverseTransformDirection(rigid.velocity).z < 1f)
			resetTime += Time.deltaTime;
		
		if(resetTime >= 4)
			carController.reverseGear = true;

		if(resetTime >= 6 || carController.speed >= 25){
			carController.reverseGear = false;
			resetTime = 0;
		}
		
	}
	
	void FixedRaycasts(){
		
		Vector3 fwd = transform.TransformDirection ( new Vector3(0, 0, 1));
		Vector3 pivotPos = new Vector3(transform.localPosition.x, carController.FrontLeftWheelCollider.transform.position.y, transform.localPosition.z);
		RaycastHit hit;
		
		// New bools effected by fixed raycasts.
		bool  tightTurn = false;
		bool  wideTurn = false;
		bool  sideTurn = false;
		bool  tightTurn1 = false;
		bool  wideTurn1 = false;
		bool  sideTurn1 = false;
		
		// New input steers effected by fixed raycasts.
		float newinputSteer1 = 0.0f;
		float newinputSteer2 = 0.0f;
		float newinputSteer3 = 0.0f;
		float newinputSteer4 = 0.0f;
		float newinputSteer5 = 0.0f;
		float newinputSteer6 = 0.0f;
		
		// Drawing Rays.
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(25, transform.up) * fwd * wideRayLength, Color.white);
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-25, transform.up) * fwd * wideRayLength, Color.white);
		
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(7, transform.up) * fwd * tightRayLength, Color.white);
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-7, transform.up) * fwd * tightRayLength, Color.white);

		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(90, transform.up) * fwd * sideRayLength, Color.white);
		Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-90, transform.up) * fwd * sideRayLength, Color.white);
		
		// Wide Raycasts.
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(25, transform.up) * fwd, out hit, wideRayLength, obstacleLayers) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(25, transform.up) * fwd * wideRayLength, Color.red);
			newinputSteer1 = Mathf.Lerp (-.5f, 0, (hit.distance / wideRayLength));
			wideTurn = true;
		}
		
		else{
			newinputSteer1 = 0;
			wideTurn = false;
		}
		
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(-25, transform.up) * fwd, out hit, wideRayLength, obstacleLayers) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-25, transform.up) * fwd * wideRayLength, Color.red);
			newinputSteer4 = Mathf.Lerp (.5f, 0, (hit.distance / wideRayLength));
			wideTurn1 = true;
		}
		
		else{
			newinputSteer4 = 0;
			wideTurn1 = false;
		}
		
		// Tight Raycasts.
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(7, transform.up) * fwd, out hit, tightRayLength, obstacleLayers) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(7, transform.up) * fwd * tightRayLength , Color.red);
			newinputSteer3 = Mathf.Lerp (-1, 0, (hit.distance / tightRayLength));
			tightTurn = true;
		}
		
		else{
			newinputSteer3 = 0;
			tightTurn = false;
		}
		
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(-7, transform.up) * fwd, out hit, tightRayLength, obstacleLayers) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-7, transform.up) * fwd * tightRayLength, Color.red);
			newinputSteer2 = Mathf.Lerp (1, 0, (hit.distance / tightRayLength));
			tightTurn1 = true;
		}
		
		else{
			newinputSteer2 = 0;
			tightTurn1 = false;
		}

		// Side Raycasts.
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(90, transform.up) * fwd, out hit, sideRayLength, obstacleLayers) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(90, transform.up) * fwd * sideRayLength , Color.red);
			newinputSteer5 = Mathf.Lerp (-1, 0, (hit.distance / sideRayLength));
			sideTurn = true;
		}
		
		else{
			newinputSteer5 = 0;
			sideTurn = false;
		}
		
		if (Physics.Raycast (pivotPos, Quaternion.AngleAxis(-90, transform.up) * fwd, out hit, sideRayLength, obstacleLayers) && !hit.collider.isTrigger && hit.transform.root != transform) {
			Debug.DrawRay (pivotPos, Quaternion.AngleAxis(-90, transform.up) * fwd * sideRayLength, Color.red);
			newinputSteer6 = Mathf.Lerp (1, 0, (hit.distance / sideRayLength));
			sideTurn1 = true;
		}
		
		else{
			newinputSteer6 = 0;
			sideTurn1 = false;
		}
		
		if(wideTurn || wideTurn1 || tightTurn || tightTurn1 || sideTurn || sideTurn1)
			raycasting = true;
		else
			raycasting = false;
		
		if(raycasting)
			newInputSteer = (newinputSteer1 + newinputSteer2 + newinputSteer3 + newinputSteer4 + newinputSteer5 + newinputSteer6);
		else
			newInputSteer = 0;
		
		if(raycasting && Mathf.Abs(newInputSteer) > .5f)
			ignoreWaypointNow = true;
		else
			ignoreWaypointNow = false;
		
	}

	void ApplyTorques(){

		if(!carController.reverseGear){
			if(!limitSpeed){
				carController.gasInput = motorInput;
			}else{
				carController.gasInput = motorInput * Mathf.Clamp01(Mathf.Lerp(10f, 0f, (carController.speed) / maximumSpeed));
			}
		}else{
			carController.gasInput = 0f;
		}

		if(smoothedSteer)
			carController.steerInput = Mathf.Lerp(carController.steerInput, steerInput, Time.deltaTime * 15f);
		else
			carController.steerInput = steerInput;

		if(!carController.reverseGear)
			carController.brakeInput = brakeInput;
		else
			carController.brakeInput = motorInput;

	}
	
	void OnTriggerEnter (Collider other){
		
		if(other.gameObject.GetComponent<RCCAIBrakeZone>()){
			inBrakeZone = true;
			maximumSpeedInBrakeZone = other.gameObject.GetComponent<RCCAIBrakeZone>().targetSpeed;
		}
		
	}
	
	void OnTriggerExit (Collider other){
		
		if(other.gameObject.GetComponent<RCCAIBrakeZone>()){
			inBrakeZone = false;
			maximumSpeedInBrakeZone = 0;
		}
		
	}
	
}