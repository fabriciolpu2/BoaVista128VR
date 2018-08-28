//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RCCUIController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public float input;
	public float sensitivity = 5f;
	private bool pressing;

	public void OnPointerDown(PointerEventData eventData){

		pressing = true;

	}

	public void OnPointerUp(PointerEventData eventData){
		 
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
