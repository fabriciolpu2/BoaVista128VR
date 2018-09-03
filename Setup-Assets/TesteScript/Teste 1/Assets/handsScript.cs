using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class handsScript : MonoBehaviour


{
    public GameObject player;
  //  public GameObject testeObj;
 //   public Collider obj;

    PegarItem script;
    public TextMesh hands;
//    GvrReticlePointer teste;
   

    // Use this for initialization
    void Start()
    {
        script = player.GetComponent<PegarItem>();
     //   teste = testeObj.GetComponent<GvrReticlePointer>();

        hands.text = "";
    }

    // Update is called once per frame
    void Update()
    {
       // obj = teste.CurrentRaycastResult.gameObject.collider;
        if (script.inHands == true)
        {
            hands.text="Ocupada ";

        }
        else
        {
            hands.text="Livre";
        }
    }
}
