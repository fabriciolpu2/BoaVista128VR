using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manterPosicao : MonoBehaviour {
    public GameObject player;
    public Transform playerTransform;

    // Use this for initialization
	void Start () {
        playerTransform = player.GetComponent<Transform>();	
	}
	
	// Update is called once per frame
	void Update () {
       // print(playerTransform.position.);
        //transform.eulerAngles = rotacao;
	}
}
