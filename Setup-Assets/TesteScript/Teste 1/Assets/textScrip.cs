using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textScrip : MonoBehaviour
{
    public GameObject player;
    PegarItem script;

    public TextMesh bala;
 

    // Use this for initialization
    void Start()
    {
        script = player.GetComponent<PegarItem>();
        bala.text = "Nada Ainda";
    }

    // Update is called once per frame
    void Update()
    {
 
        if (script.balla != null)
        {
            bala.text=script.balla.name;

        }
        else
        {
            bala.text="Nada Ainda";
        }
    }
}
