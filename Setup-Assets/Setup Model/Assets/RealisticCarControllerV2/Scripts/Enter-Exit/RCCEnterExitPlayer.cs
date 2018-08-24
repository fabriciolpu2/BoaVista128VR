//----------------------------------------------
//            Realistic Car Controller
//
// Copyright © 2015 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;

public class RCCEnterExitPlayer : MonoBehaviour {
	
	public float maxRayDistance= 2.0f;
	private bool showGui = false;
    public GameObject activeCar;
    public GameObject cameraPlayer;
    public GameObject uiPlayerController;

    void Update (){
		
		Vector3 direction= transform.TransformDirection(Vector3.forward);
		RaycastHit hit;
        /**
		if (Physics.Raycast(transform.position, direction, out hit, maxRayDistance)){

			if(hit.transform.GetComponentInParent<RCCCarControllerV2>()){

				showGui = true;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    //hit.transform.root.SendMessage("Act", GetComponentInParent<CharacterController>().gameObject, SendMessageOptions.DontRequireReceiver);
                    hit.transform.root.SendMessage("Act", GetComponentInParent<ThirdPersonUserControl>().gameObject, SendMessageOptions.DontRequireReceiver);
                }				
            }
            else{
				showGui = false;
			}			
		}else{
			showGui = false;
		}	
        */

        if (Input.GetButtonDown("Fire1") && activeCar != null)
        {
            print("chegou no KeyCOde");
            activeCar.transform.root.SendMessage("Act", GetComponentInParent<ThirdPersonCharacter>().gameObject, SendMessageOptions.DontRequireReceiver);
            cameraPlayer.SetActive(false);
            uiPlayerController.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Carros")
        {
            print("Carros");
            if (collision.transform.GetComponentInParent<RCCCarControllerV2>())
            {
                activeCar = collision.gameObject;
                print("chegou nos getInParent");
                showGui = true;
                
            } else
            {
                showGui = false;
            }
        } else
        {
            showGui = false;
        }
        
    }

    void OnGUI (){
		
		if(showGui){
			GUI.Label( new Rect(Screen.width - (Screen.width/1.7f),Screen.height - (Screen.height/1.4f),800,100),"Press ''E'' key to Get In");
		}
		
	}
    public void setCamera()
    {

    }
	
}