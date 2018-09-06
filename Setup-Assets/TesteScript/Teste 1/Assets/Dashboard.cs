using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashboard : MonoBehaviour {
    public GameObject player;
    private Transform minhaPosicao;
	// Use this for initialization
	void Start () {
        minhaPosicao = GetComponent<Transform>();

    }

    private void FixedUpdate()
    {
        Vector3 direcao = player.transform.position - transform.position;
        Quaternion novaRotacao = Quaternion.LookRotation(direcao);
        GetComponent<Rigidbody>().MoveRotation(novaRotacao);
        float distancia = Vector3.Distance(minhaPosicao.transform.position, player.transform.position);
        if (distancia > 6)
        {
            FecharDash();
        }


    }


    public void FecharDash()
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
    }

}


