using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Infor : MonoBehaviour {
	public GameObject uiObject;
	public Text guiText;

	// Use this for initialization
	void Start () 
	{
		uiObject.SetActive(false);
	}

	void ativaDesativa () 
	{
		uiObject.SetActive (true);
		StartCoroutine(Example());
	}
	
	// Update is called once per frame
	public void infor () {
		ativaDesativa ();		
	}
		
	IEnumerator Example()
	{
		yield return new WaitForSeconds (5);
		uiObject.SetActive (false);
	}
}
