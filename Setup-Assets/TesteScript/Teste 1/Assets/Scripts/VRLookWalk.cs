using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRLookWalk : MonoBehaviour {
    public Transform VrCamera;

    public float toggleAngre = 30.0f;

    public float speed = 3.0f;

    public bool moveForward;

    public CharacterController cc;
    
    
    
    // Use this for initialization
	void Start () {
        cc = GetComponentInParent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		 if(VrCamera.eulerAngles.x>=toggleAngre && VrCamera.eulerAngles.x< 90.0f)
        {
            moveForward = true;
        }
        else
        {
            moveForward = false;

        }

        if (moveForward)
        {
            Vector3 foward = VrCamera.TransformDirection(Vector3.forward);

            cc.SimpleMove(foward * speed);
        }
	}
}
