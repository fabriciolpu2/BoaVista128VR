//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCCarCamera : MonoBehaviour{

	// The target we are following
	public Transform playerCar;
	private Rigidbody playerRigid;
	private Camera cam;

	// The distance in the x-z plane to the target
	public float distance = 6.0f;
	
	// the height we want the camera to be above the target
	public float height = 2.0f;
	
	public float heightOffset = .75f;
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	public bool useSmoothRotation = true;
	
	public float minimumFOV = 50f;
	public float maximumFOV = 70f;
	
	public float maximumTilt = 15f;
	private float tiltAngle = 0f;
	
	void Start(){
		
		// Early out if we don't have a target
		if (!playerCar)
			return;

		playerRigid = playerCar.GetComponent<Rigidbody>();
		cam = GetComponent<Camera>();

		if(GetComponent<RCCCamManager>())
			GetComponent<RCCCamManager>().target = playerCar;
		
	}
	
	void Update(){
		
		// Early out if we don't have a target
		if (!playerCar)
			return;
		
		if(playerRigid != playerCar.GetComponent<Rigidbody>())
			playerRigid = playerCar.GetComponent<Rigidbody>();
		
		//Tilt Angle Calculation.
		tiltAngle = Mathf.Lerp (tiltAngle, (Mathf.Clamp (-playerCar.InverseTransformDirection(playerRigid.velocity).x, -35, 35)), Time.deltaTime * 2f);

		if(!cam)
			cam = GetComponent<Camera>();

		cam.fieldOfView = Mathf.Lerp (minimumFOV, maximumFOV, (playerRigid.velocity.magnitude * 3f) / 150f);
		
	}
	
	void LateUpdate (){
		
		// Early out if we don't have a target
		if (!playerCar || !playerRigid)
			return;
		
		float speed = (playerRigid.transform.InverseTransformDirection(playerRigid.velocity).z) * 3f;
		
		// Calculate the current rotation angles.
		float wantedRotationAngle = playerCar.eulerAngles.y;
		float wantedHeight = playerCar.position.y + height;
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;

		if(useSmoothRotation)
			rotationDamping = Mathf.Lerp(0f, 3f, (playerRigid.velocity.magnitude * 3f) / 40f);
		
		if(speed < -10)
			wantedRotationAngle = playerCar.eulerAngles.y + 180;
		
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight + Mathf.Lerp(-1f, 0f, (playerRigid.velocity.magnitude * 3f) / 20f), heightDamping * Time.deltaTime);
		
		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = playerCar.position;
		transform.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
		transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);
		
		// Always look at the target
		transform.LookAt (new Vector3(playerCar.position.x, playerCar.position.y + heightOffset, playerCar.position.z));
		transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y, Mathf.Clamp(tiltAngle, -10f, 10f));
		
	}

}