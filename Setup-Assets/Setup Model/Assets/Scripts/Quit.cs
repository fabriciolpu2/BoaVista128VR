using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

	public void dotQuit() {
		Debug.Log ("Você saiu");
		Application.Quit(); 
	}

}