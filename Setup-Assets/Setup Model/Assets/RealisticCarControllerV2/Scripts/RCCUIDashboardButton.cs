//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RCCUIDashboardButton : MonoBehaviour {
	
	public ButtonType _buttonType;
	public enum ButtonType{Start, ABS, ESP, TCS, Headlights};

	private RCCCarControllerV2[] cars;

	void Start(){

		Check();

	}
	
	public void Act () {
		
		cars = GameObject.FindObjectsOfType<RCCCarControllerV2>();
		
		switch(_buttonType){
			
		case ButtonType.Start:
			
			for(int i = 0; i < cars.Length; i++){
				
				if(cars[i].canControl)
					cars[i].KillOrStartEngine();
				
			}
			
			break;
			
		case ButtonType.ABS:
			
			for(int i = 0; i < cars.Length; i++){
				
				if(cars[i].canControl)
					cars[i].ABS = !cars[i].ABS;
				
			}
			
			break;
			
		case ButtonType.ESP:
			
			for(int i = 0; i < cars.Length; i++){
				
				if(cars[i].canControl)
					cars[i].ESP = !cars[i].ESP;
				
			}
			
			break;
			
		case ButtonType.TCS:
			
			for(int i = 0; i < cars.Length; i++){
				
				if(cars[i].canControl)
					cars[i].TCS = !cars[i].TCS;
				
			}
			
			break;
			
		case ButtonType.Headlights:
			
			for(int i = 0; i < cars.Length; i++){
				
				if(cars[i].canControl)
					cars[i].headLightsOn = !cars[i].headLightsOn;
				
			}
			
			break;
			
		}
		
		Check();
		
	}
	
	public void Check(){

		cars = GameObject.FindObjectsOfType<RCCCarControllerV2>();
		
		switch(_buttonType){
			
		case ButtonType.ABS:
			
			for(int i = 0; i < cars.Length; i++){
				
				if(cars[i].canControl && cars[i].ABS)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(cars[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		case ButtonType.ESP:
			
			for(int i = 0; i < cars.Length; i++){
				
				if(cars[i].canControl && cars[i].ESP)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(cars[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		case ButtonType.TCS:
			
			for(int i = 0; i < cars.Length; i++){
				
				if(cars[i].canControl && cars[i].TCS)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(cars[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		case ButtonType.Headlights:
			
			for(int i = 0; i < cars.Length; i++){
				
				if(cars[i].canControl && cars[i].headLightsOn)
					GetComponent<Image>().color = new Color(1, 1, 1, 1);
				else if(cars[i].canControl)
					GetComponent<Image>().color = new Color(.25f, .25f, .25f, 1);
				
			}
			
			break;
			
		}
		
	}
	
}
