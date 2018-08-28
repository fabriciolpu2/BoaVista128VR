﻿using UnityEngine;
using System.Collections;

public class RCCCharacterController : MonoBehaviour {

	private RCCCarControllerV2 carController;
	private Rigidbody carRigid;
	private Animator animator;

	public string driverSteeringParameter;
	public string driverShiftingGearParameter;
	public string driverDangerParameter;
	public string driverReversingParameter;

	public float steerInput = 0f;
	public float directionInput = 0f;
	public bool reversing = false;
	public float impactInput = 0f;
	public float gearInput = 0f;

	void Start () {

		animator = GetComponent<Animator>();
		carController = GetComponent<RCCCarControllerV2>();
		carRigid = GetComponent<Rigidbody>();
		
	}

	void Update () {

		steerInput = Mathf.Lerp(steerInput, carController.steerInput, Time.deltaTime * 5f);
		directionInput = carRigid.transform.InverseTransformDirection(carRigid.velocity).z;
		impactInput -= Time.deltaTime * 5f;

		if(impactInput < 0)
			impactInput = 0f;
		if(impactInput > 1)
			impactInput = 1f;

		if(directionInput <= -2f)
			reversing = true;
		else if(directionInput > -1f)
			reversing = false;

		if(carController.changingGear)
			gearInput = 1f;
		else
			gearInput -= Time.deltaTime * 5f;

		if(gearInput < 0)
			gearInput = 0f;
		if(gearInput > 1)
			gearInput = 1f;

		if(!reversing){
			animator.SetBool(driverReversingParameter, false);
		}else{
			animator.SetBool(driverReversingParameter, true);
		}

		if(impactInput > .5f){
			animator.SetBool(driverDangerParameter, true);
		}else{
			animator.SetBool(driverDangerParameter, false);
		}

		if(gearInput > .5f){
			animator.SetBool(driverShiftingGearParameter, true);
		}else{
			animator.SetBool(driverShiftingGearParameter, false);
		}

		animator.SetFloat(driverSteeringParameter, steerInput);
		
	}

	void OnCollisionEnter(Collision col){

		if(col.relativeVelocity.magnitude < 2.5f)
			return;

		impactInput = 1f;

	}

}
