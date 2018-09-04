using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissaoRonda : MonoBehaviour {
    public GameObject player;
    StatusPlayer zonas;
    public TextMesh texto;
    BoxCollider colisor;
    public GameObject MestreMissao;
    QuestBegin status;


    private void Awake()
    {
        zonas = player.GetComponent<StatusPlayer>();
        status = MestreMissao.GetComponent<QuestBegin>();

    }
    void Start () {
        texto.text = "Lugares restantes: " + zonas.zonas + "/4";
        colisor = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (status.iniciada == true)
        {
            zonas.zonas++;

            texto.text = "Lugares restantes: " + zonas.zonas + "/4";
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (status.iniciada == true)
        {
            colisor.enabled = false;

        }
    }
    // Update is called once per frame

}
