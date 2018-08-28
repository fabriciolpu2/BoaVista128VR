//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(RCCDashboardInputs))]

public class RCCUIDashboardDisplay : MonoBehaviour {

	private RCCDashboardInputs inputs;
	
	public Text RPMLabel;
	public Text KMHLabel;
	public Text GearLabel;

	public Image ABS;
	public Image ESP;
	public Image Park;
	public Image Headlights;
	
	void Start () {
		
		inputs = GetComponent<RCCDashboardInputs>();
		StartCoroutine("LateDisplay");
		
	}

	void OnEnable(){

		StopAllCoroutines();
		StartCoroutine("LateDisplay");

	}
	
	IEnumerator LateDisplay () {

		while(true){

			yield return new WaitForSeconds(.04f);
		
			if(RPMLabel)
				RPMLabel.text = inputs.RPM.ToString("0");
			if(KMHLabel)
				KMHLabel.text = inputs.KMH.ToString("0");

			if(GearLabel){
				if(!inputs.NGear)
					GearLabel.text = inputs.Gear >= 0 ? (inputs.Gear + 1).ToString("0") : "R";
				else
					GearLabel.text = "N";
			}

			if(ABS)
				ABS.color = inputs.ABS == true ? Color.red : Color.white;
			if(ESP)
				ESP.color = inputs.ESP == true ? Color.red : Color.white;
			if(Park)
				Park.color = inputs.Park == true ? Color.red : Color.white;
			if(Headlights)
				Headlights.color = inputs.Headlights == true ? Color.green : Color.white;

		}

	}

}
