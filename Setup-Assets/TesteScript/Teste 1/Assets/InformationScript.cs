using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationScript : MonoBehaviour {
    
        public GameObject player;
        public GameObject dashBoard;
        Transform minhaPosicao;
        MeshRenderer dashRender;
        BoxCollider dashCollider;
        // Use this for initialization
        void Start()
        {
            minhaPosicao = GetComponent<Transform>();
            dashRender = dashBoard.GetComponent<MeshRenderer>();
            dashCollider = dashBoard.GetComponent<BoxCollider>();
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            Vector3 direcao = player.transform.position - transform.position;
            Quaternion novaRotacao = Quaternion.LookRotation(direcao);
            GetComponent<Rigidbody>().MoveRotation(novaRotacao);


        }

        public void ativarDash()
        {
            float distancia = Vector3.Distance(minhaPosicao.transform.position, player.transform.position);
            if (distancia < 4)
            {
               dashRender.enabled=true;
               dashCollider.enabled = true;
            }
        }

    }


