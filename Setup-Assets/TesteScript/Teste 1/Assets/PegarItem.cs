using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PegarItem : MonoBehaviour
{
    public Camera cam;
  //  bool triggerEntered = false;
    public Collider balla;
  //  Collider trig;
    public GameObject myHand;
    public bool inHands = false;
  //  Vector3 ballPoss;
  //  Collider ballCol;
    Rigidbody ballRb;
    public float teste=1;
    public LayerMask itensPegaveis ;



    private void FixedUpdate()
    {
        if (!inHands)
        {
            balla = null;
        }
        Ray raio = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit impacto;

        if (Physics.Raycast(raio, out impacto, teste ))
        {
            Debug.DrawRay(raio.origin, raio.direction * teste, Color.red);
            if (impacto.distance <= 3f)
            {
                if (impacto.collider.CompareTag("Item"))
                {
                    balla = impacto.collider;

                }
                //print(impacto.collider);
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
        //        balla.transform.localPosition = new Vector3(0f, -0.36f, 0f);
                inHands = true;
            }
            else if (inHands)
            {
                ballRb.isKinematic = false;
                ballRb.useGravity = true;
                balla.transform.SetParent(null);
                inHands = false;
            }

        }
    }

}