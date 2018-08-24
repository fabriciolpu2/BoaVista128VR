//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCNGUIController : MonoBehaviour {

	public float input;
	public float sensitivity = 5f;
	private bool pressing;

	void Start () {
	
	}

	void OnPress (bool isPressed)
	{
		if(isPressed)
			pressing = true;
		else
			pressing = false;
	}

	void Update(){

		if(pressing)
			input += Time.deltaTime * sensitivity;
		else
			input -= Time.deltaTime * sensitivity;

		if(input < 0f)
			input = 0f;
		if(input > 1f)
			input = 1f;

	}

}
