using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour {
    public GameObject ball;
    public GameObject myHand;
    //publicFloat
    bool inHands=false;
    Vector3 ballPoss;
    Collider ballCol;
    Rigidbody ballRb;
   // Camera cam;

	// Use this for initialization

	void Start () {
        ballCol = ball.GetComponent<BoxCollider>();
        ballRb = ball.GetComponent<Rigidbody>();
      //  cam.GetComponentInChildren<Camera>();    
    }
    // Update is called once per frame
    void Update () {
        
   //     float distancia = vector3.Distance(tranform.position, Jogador.position);
        if (Input.GetButtonDown("Fire1"))
        {
            if (!inHands)
            {
                ballCol.isTrigger = true;
                ballRb.useGravity = false;
                ball.transform.SetParent(myHand.transform);
                ball.transform.localPosition = new Vector3(0f, -0.36f, 0f);
                inHands = true;
                
            }
            else if (inHands)
            {
                ballCol.isTrigger = false;
                ballRb.useGravity = true;
                this.GetComponent<PlayerGrab>().enabled = false;
                ball.transform.SetParent(null);
                inHands = false;

            }
              
        }
	}
}
