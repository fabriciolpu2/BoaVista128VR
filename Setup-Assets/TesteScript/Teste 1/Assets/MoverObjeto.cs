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

    // Update is called once per frame
    void Update () {
	}
    public void mover()
    {


        if (!script.inHands)
            {
                ballRb.isKinematic = true;
                ballRb.useGravity = false;
                posiçãoOBj.transform.SetParent(hands.transform);
             //   posiçãoOBj.transform.localPosition = new Vector3(0f, -0.36f, 0f);
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
