using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teste1 : MonoBehaviour {
    public GameObject playerCamera;
    GvrPointerPhysicsRaycaster raycast;
	// Use this for initialization
	void Start () {
        raycast = GetComponent<GvrPointerPhysicsRaycaster>();	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Jump"))
        {
            //Destroy(other.gameObject);
         //   Destroy(raycast.)
        }
	}
}
