//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;

public class RCCResetScene : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyUp(KeyCode.R)){
			Application.LoadLevel(Application.loadedLevel);
		}
	
	}

}
