using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegarItem : MonoBehaviour
{
    bool triggerEntered = false;
    Collider balla=null;
    public GameObject myHand;
    bool inHands = false;
    Vector3 ballPoss;
    Collider ballCol;
    Rigidbody ballRb;

    public Collider OnTriggerEnter(Collider ball)
    {
        Debug.Log("entrou no colisor");
        if (balla == null)
        {
            balla = ball;
            return null;
        }

        

        triggerEntered = true;
        return null;
    }
    public Collider OnTriggerExit(Collider ball)
    {
        triggerEntered = false;
        return null;
    }


    /*  void Start()
{
    ballCol = ball.GetComponent <BoxCollider>();
    ballRb = ball.GetComponent<Rigidbody>();
    //  cam.GetComponentInChildren<Camera>();    
}
*/  // Update is called once per frame
    void Update()
    {
  
        //     float distancia = vector3.Distance(tranform.position, Jogador.position);
        if (Input.GetButtonDown("Fire1") && triggerEntered)
        {
         //   ballCol = balla.GetComponent<BoxCollider>();
            ballRb = balla.GetComponent<Rigidbody>();
            if (!inHands)
            {
                //         ballCol.isTrigger = true;
                ballRb.isKinematic = true;
                ballRb.useGravity = false;
                balla.transform.SetParent(myHand.transform);
                balla.transform.localPosition = new Vector3(0f, -0.36f, 0f);
                inHands = true;

            }
            else if (inHands)
            {
                //       ballCol.isTrigger = false;
                ballRb.isKinematic = false;

                ballRb.useGravity = true;
                this.GetComponent<PegarItem>().enabled = false;
                balla.transform.SetParent(null);
                inHands = false;
                balla = null;

            }

        }
    }
}
