using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectGrab : MonoBehaviour {
 //  public PlayerGrab teste;
    GameObject objeto;
  //  GameObject teste;
	// Use this for initialization
	void Awake () {
        objeto=GetComponent<GameObject>();
    }
	
	// Update is called once per frame
	void Update () {
        if(objeto!= null) {
         //   PlayerGrab.ball = objeto;
        }
            
    }
}
