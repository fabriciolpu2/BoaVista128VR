//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCMobileButtons : MonoBehaviour {

	public GameObject gasButton;
	public GameObject brakeButton;
	public GameObject leftButton;
	public GameObject rightButton;
	public GameObject steeringWheel;
	public GameObject handbrakeButton;
	public GameObject boostButton;

	public void ChangeCamera () {

		if(GameObject.FindObjectOfType<RCCCamManager>())
			GameObject.FindObjectOfType<RCCCamManager>().ChangeCamera();

	}

}
