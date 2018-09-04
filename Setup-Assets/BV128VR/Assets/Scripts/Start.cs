using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour {
	// Use this for initialization

	public void selectStart(){
		SceneManager.LoadScene (1);
	}
}