//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCCarCameraConfig : MonoBehaviour {

	public float distance = 10f;
	public float height = 5f;

	void OnEnable () {

		Camera cam = GameObject.FindObjectOfType<RCCCarCamera>().GetComponent<Camera>();

		if(!cam)
			return;

		if(cam.GetComponent<RCCCarCamera>()){
			cam.GetComponent<RCCCarCamera>().distance = distance;
			cam.GetComponent<RCCCarCamera>().height = height;
		}

	}

}
