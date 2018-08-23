using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesligarTrigger : MonoBehaviour {
    public GameObject player;
    PegarItem script;
    Collider ballcol;

    private void Start()
    {
        script = player.GetComponent<PegarItem>();
        ballcol = GetComponent<BoxCollider>();

    }
    void Update () {
        if (script.inHands == true)
        {
            ballcol.enabled = false;
        }
        else 
        {
            ballcol.enabled = true;
        }
    }
}
