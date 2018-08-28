//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCTruckTrailer : MonoBehaviour {

	private RCCCarControllerV2 carController;
	public Transform centerOfMass;

	//Extra Wheels.
	public Transform[] wheelTransforms;
	public WheelCollider[] wheelColliders;

	private float[] rotationValues;

	void Start () {
	
		carController = transform.root.GetComponent<RCCCarControllerV2>();
		rotationValues = new float[wheelColliders.Length];
		GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;
		GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.Interpolate;
		GetComponent<Rigidbody>().centerOfMass = new Vector3((centerOfMass.transform.localPosition.x * transform.localScale.x), (centerOfMass.transform.localPosition.y * transform.localScale.y), (centerOfMass.transform.localPosition.z * transform.localScale.z));

	}

	void Update(){

		WheelAlign();

	}

	void WheelAlign(){

		if(carController.sleepingRigid)
			return;

		if(wheelColliders.Length > 0){
			
			RaycastHit hit;
			
			for(int i = 0; i < wheelColliders.Length; i++){
				
				Vector3 ColliderCenterPointExtra = wheelColliders[i].transform.TransformPoint( wheelColliders[i].center );
				
				if(Physics.Raycast(ColliderCenterPointExtra, -wheelColliders[i].transform.up, out hit, (wheelColliders[i].suspensionDistance + wheelColliders[i].radius) * transform.localScale.y) && !hit.collider.isTrigger && hit.transform != transform){
					wheelTransforms[i].transform.position = hit.point + (wheelColliders[i].transform.up * wheelColliders[i].radius) * transform.localScale.y;
				}else{
					wheelTransforms[i].transform.position = ColliderCenterPointExtra - (wheelColliders[i].transform.up * wheelColliders[i].suspensionDistance) * transform.localScale.y;
				}

				wheelTransforms[i].transform.rotation = wheelColliders[i].transform.rotation * Quaternion.Euler( rotationValues[i], 0, wheelColliders[i].transform.rotation.z);
				rotationValues[i] += wheelColliders[i].rpm * ( 6 ) * Time.deltaTime;
				
			}
			
		}

	}

	void FixedUpdate(){

		//Applying Small Torque For Preventing Stuck Issue. Unity 5 Wheel Colliders Are Weird :/
		foreach(WheelCollider wc in wheelColliders){
			wc.motorTorque = carController.gasInput;
		}

	}

}
