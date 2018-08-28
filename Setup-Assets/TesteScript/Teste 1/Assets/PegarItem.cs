using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegarItem : MonoBehaviour
{
    public Camera cam;
    bool triggerEntered = false;
    Collider balla;
    Collider trig;
    public GameObject myHand;
    public bool inHands = false;
    Vector3 ballPoss;
    Collider ballCol;
    Rigidbody ballRb;
    public float teste=100;
    public LayerMask itensPegaveis ;

 /*   public void OnTriggerEnter(Collider ball)
    {
        trig = ball;
        triggerEntered = true;
  
    }
    public void OnTriggerExit(Collider ball)
    {
        trig = null;
        triggerEntered = false;
    }

    */

    private void FixedUpdate()
    {
        Ray raio = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(raio.origin, raio.direction * teste, Color.red);
        RaycastHit impacto;

        if(Physics.Raycast(raio,out impacto,teste, itensPegaveis))
        {
            Debug.DrawRay(raio.origin, raio.direction * teste, Color.red);
            if (impacto.distance <= 1f)
            {
                balla = impacto.collider;
                print(impacto.collider);
            }
            
        }
        else
        {
            Debug.DrawRay(raio.origin, raio.direction * teste, Color.green);
        }

        if (Input.GetButtonDown("Fire1") && balla != null)
        {

            ballRb = balla.GetComponent<Rigidbody>();
            if (!inHands)
            {
                ballRb.isKinematic = true;
                ballRb.useGravity = false;
                balla.transform.SetParent(myHand.transform);
                inHands = true;

            }
            else if (inHands)
            {
                
                ballRb.isKinematic = false;
                ballRb.useGravity = true;
                balla.transform.SetParent(null);
                inHands = false;
                balla = null; 
            }

        }
    }

    void Update()
    {

            //     float distancia = vector3.Distance(tranform.position, Jogador.position);
           
        }
        
    }
