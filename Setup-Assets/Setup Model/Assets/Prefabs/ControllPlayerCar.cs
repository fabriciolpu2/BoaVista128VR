using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class ControllPlayerCar : MonoBehaviour {

    //public GameObject cameraCarro;
    public GameObject cameraPersonagem;
    public Camera cameraCarro;
    public GameObject player;

    private GameObject[] objects;
    private GameObject activeObject;
    private int activeObjectIdx;
    private Camera mainCamera;
    public GameObject GUICar;


    public bool noCarro;

    void Awake()
    {
        RCCCarControllerV2[] vehicles = GameObject.FindObjectsOfType<RCCCarControllerV2>();
        objects = new GameObject[vehicles.Length];

        for (int i = 0; i < vehicles.Length; i++)
        {
            objects[i] = vehicles[i].gameObject;
            vehicles[i].gameObject.name = i.ToString();
            
        }

        foreach (GameObject controller in objects)
        {
            controller.GetComponent<RCCCarControllerV2>().canControl = false;
            controller.GetComponent<RCCCarControllerV2>().runEngineAtAwake = false;
            controller.GetComponent<RCCCarControllerV2>().engineRunning = false;
        }

        mainCamera = GameObject.FindObjectOfType<RCCCarCamera>().GetComponent<Camera>();

    }

    void EntraCarro()
    {
        noCarro = true;
        cameraCarro.GetComponent<Camera>().enabled = true;
        //cameraCarro.gameObject.SetActive(false);
        cameraPersonagem.gameObject.SetActive(false);

        // Ativa Controle Carro
        objects[activeObjectIdx].GetComponent<RCCCarControllerV2>().canControl = true;
        objects[activeObjectIdx].GetComponent<RCCCarControllerV2>().KillOrStartEngine();
        
        // Ativa Controle Carro Camera
        cameraCarro.GetComponent<RCCCamManager>().enabled = true;
        cameraCarro.GetComponent<RCCCarCamera>().playerCar = objects[activeObjectIdx].transform;
        cameraCarro.GetComponent<RCCCamManager>().cameraChangeCount = 5;
        cameraCarro.GetComponent<RCCCamManager>().ChangeCamera();

        // Desativa Controles Player
        player.GetComponent<ThirdPersonUserControl>().enabled = false;
        player.GetComponent<ThirdPersonCharacter>().enabled = false;
        player.transform.position = objects[activeObjectIdx].transform.position;
        player.transform.parent = objects[activeObjectIdx].transform;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<CapsuleCollider>().isTrigger = true;
        //player.GetComponent<MeshRenderer>().enabled = false;
        player.transform.localPosition = new Vector3(0, 0, 0);

        GUICar.gameObject.SetActive(true);

    }
    public void SaiCarro()
    {
        GUICar.gameObject.SetActive(false);
        objects[activeObjectIdx].GetComponent<RCCCarControllerV2>().canControl = false;
        objects[activeObjectIdx].GetComponent<RCCCarControllerV2>().KillOrStartEngine();
        cameraCarro.GetComponent<RCCCamManager>().enabled = false;
        cameraCarro.GetComponent<RCCCarCamera>().playerCar = null;
        cameraCarro.GetComponent<RCCCamManager>().cameraChangeCount = 5;
        cameraCarro.GetComponent<RCCCamManager>().ChangeCamera();

        noCarro = false;
        cameraCarro.GetComponent<Camera>().enabled = false;
        //cameraCarro.gameObject.SetActive(false);
        cameraPersonagem.gameObject.SetActive(true);

        player.GetComponent<ThirdPersonUserControl>().enabled = true;
        player.GetComponent<ThirdPersonCharacter>().enabled = true;
        player.transform.position = new Vector3(player.transform.position.x - 5, player.transform.position.y, player.transform.position.z);
        player.transform.parent = null;
        //player.GetComponent<MeshRenderer>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;
        player.GetComponent<CapsuleCollider>().isTrigger = false;

    }
	// Update is called once per frame
	void Update () {
        //print("Carro ID: " + activeObjectIdx);
		if(Input.GetKeyDown("f"))
        {
            if(noCarro == false)
            {
                EntraCarro();
            } else
            {
                SaiCarro();
            }

            
        }
	}
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Carros")
        {
            activeObjectIdx = int.Parse(collision.gameObject.name);
            print(activeObjectIdx);
        }
        
    }
}

