//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCWheelCollider : MonoBehaviour {
	
	private RCCCarControllerV2 carController;
	private Rigidbody carRigid;

	private float startSlipValue = .25f;
	private RCCSkidmarks skidmarks;
	private int lastSkidmark = -1;
	private WheelCollider wheelCollider;
	
	private float wheelSlipAmountSideways;
	private float wheelSlipAmountForward;

	//WheelFriction Curves and Stiffness.
	private WheelFrictionCurve forwardFrictionCurve;
	private WheelFrictionCurve sidewaysFrictionCurve;
	
	void  Start (){

		wheelCollider = GetComponent<WheelCollider>();
		carController = GetComponentInParent<RCCCarControllerV2>();
		carRigid = carController.GetComponent<Rigidbody>();

		if(FindObjectOfType(typeof(RCCSkidmarks)))
			skidmarks = FindObjectOfType(typeof(RCCSkidmarks)) as RCCSkidmarks;
		else
			Debug.Log("No skidmarks object found. Skidmarks will not be drawn. Drag ''RCCSkidmarksManager'' from Prefabs folder, and drop on to your existing scene...");

		forwardFrictionCurve = GetComponent<WheelCollider>().forwardFriction;
		sidewaysFrictionCurve = GetComponent<WheelCollider>().sidewaysFriction;

	}
	
	void  FixedUpdate (){

		if(skidmarks){

			WheelHit GroundHit;
			wheelCollider.GetGroundHit(out GroundHit);
			
			wheelSlipAmountSideways = Mathf.Abs(GroundHit.sidewaysSlip);
			wheelSlipAmountForward = Mathf.Abs(GroundHit.forwardSlip);
			
			if (wheelSlipAmountSideways > startSlipValue || wheelSlipAmountForward > .5f){
				
				Vector3 skidPoint = GroundHit.point + 2f * (carRigid.velocity) * Time.deltaTime;

				if(carRigid.velocity.magnitude > 1f)
					lastSkidmark = skidmarks.AddSkidMark(skidPoint, GroundHit.normal, (wheelSlipAmountSideways / 2f) + (wheelSlipAmountForward / 2.5f), lastSkidmark);
				else
					lastSkidmark = -1;

			}

			else{

				lastSkidmark = -1;

			}
			
		}

		WheelHit hit;
		wheelCollider.GetGroundHit(out hit);

		if(wheelCollider.GetGroundHit(out hit)){

			if(carController._groundMaterial == RCCCarControllerV2.GroundMaterial.Asphalt && carController.asphaltPhysicsMaterial){
				forwardFrictionCurve.stiffness = carController.asphaltPhysicsMaterial.staticFriction * 1.666f;
				sidewaysFrictionCurve.stiffness = carController.asphaltPhysicsMaterial.staticFriction * 1.666f;
			}else if(carController._groundMaterial == RCCCarControllerV2.GroundMaterial.Grass && carController.grassPhysicsMaterial){
				forwardFrictionCurve.stiffness = carController.grassPhysicsMaterial.staticFriction * 1.666f;
				sidewaysFrictionCurve.stiffness = carController.grassPhysicsMaterial.staticFriction * 1.666f;
			}else if(carController._groundMaterial == RCCCarControllerV2.GroundMaterial.Sand && carController.sandPhysicsMaterial){
				forwardFrictionCurve.stiffness = carController.sandPhysicsMaterial.staticFriction * 1.666f;
				sidewaysFrictionCurve.stiffness = carController.sandPhysicsMaterial.staticFriction * 1.666f;
			}else if(carController.asphaltPhysicsMaterial){
				forwardFrictionCurve.stiffness = carController.asphaltPhysicsMaterial.staticFriction * 1.666f;
				sidewaysFrictionCurve.stiffness = carController.asphaltPhysicsMaterial.staticFriction * 1.666f;
			}else{
				forwardFrictionCurve.stiffness = 1f;
				sidewaysFrictionCurve.stiffness = 1f;
			}

			if(carController._wheelTypeChoise == RCCCarControllerV2.WheelType.DRIFT){
				if(wheelCollider == carController.FrontLeftWheelCollider || wheelCollider == carController.FrontRightWheelCollider){
					sidewaysFrictionCurve.asymptoteValue = .75f;
				}else{
					forwardFrictionCurve.asymptoteValue = 1f;
					sidewaysFrictionCurve.asymptoteValue = .65f;
				}
			}

			wheelCollider.forwardFriction = forwardFrictionCurve;
			wheelCollider.sidewaysFriction = sidewaysFrictionCurve;

		}

	}

}