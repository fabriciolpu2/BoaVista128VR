using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverObjeto : MonoBehaviour {
    Transform posiçãoOBj;
    Rigidbody ballRb;
    public GameObject hands;
  //  public bool statusMao;
    StatusPlayer script;
    public GameObject player;


    void Start () {
        posiçãoOBj = GetComponent<Transform>();
        script = player.GetComponent<StatusPlayer>();
        ballRb = posiçãoOBj.GetComponent<Rigidbody>();

    }

  
    public void mover()
    {
        float distancia = Vector3.Distance(posiçãoOBj.transform.position, player.transform.position);

        if (distancia < 2.0f)
        {
            if (!script.inHands)
            {
                ballRb.isKinematic = true;
                ballRb.useGravity = false;
                posiçãoOBj.transform.SetParent(hands.transform);
                script.inHands = true;
            }
            else if (script.inHands)
            {
                ballRb.isKinematic = false;
                ballRb.useGravity = true;
                posiçãoOBj.transform.SetParent(null);
                script.inHands = false;
            }
        }
    }
}
