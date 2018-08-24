//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCCameraOrbit : MonoBehaviour
{

	public Transform target;
	public float distance= 10.0f;
	
	public float xSpeed= 250f;
	public float  ySpeed= 120f;
	
	public float yMinLimit= -20f;
	public float yMaxLimit= 80f;
	
	private float x= 0f;
	private float y= 0f;
		
	void  Start (){

		Vector3 angles= transform.eulerAngles;
		x = angles.y;
		y = angles.x;
		
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;

	}
	
	void  LateUpdate (){

		if (target) {

			x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
			
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			
			Quaternion rotation= Quaternion.Euler(y, x, 0);
			Vector3 position= rotation * new Vector3(0f, 0f, -distance) + target.position;
			
			transform.rotation = rotation;
			transform.position = position;
		}

	}
	
	static float ClampAngle ( float angle ,   float min ,   float max  ){

		if (angle < -360)
			angle += 360;
		if (angle > 360)
			angle -= 360;
		return Mathf.Clamp (angle, min, max);

	}
	
}