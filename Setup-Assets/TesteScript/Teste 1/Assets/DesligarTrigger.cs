﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesligarTrigger : MonoBehaviour {
    public GameObject player;
    PegarItem script;
    Collider ballcol;
    LayerMask mask;
    private void Start()
    {
        script = player.GetComponent<PegarItem>();
        ballcol = GetComponent<BoxCollider>();
      //  mask = GetComponent<Layer>();
    }
    void Update () {



        if (script.inHands == true )
        {
        //    ballcol.isTrigger = true;
            gameObject.layer = 0;
            
        }
        else 
        {
      //      script.enabled = true;
          //  ballcol.isTrigger = false;
            gameObject.layer = 9;
        }
    }
}