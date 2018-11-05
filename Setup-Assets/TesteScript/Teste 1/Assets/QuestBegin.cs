using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestBegin : MonoBehaviour
{
    public bool iniciada = false;
    public GameObject player;
    public TextMesh text;
    public TextMesh text2;
    StatusPlayer zonas;
    Transform minhaPosicao;
    public GameObject objTexto1;
    public GameObject objTexto2;
    float timer = 0.0f;
    //


    void Start()
    {
        minhaPosicao = GetComponent<Transform>();

    }

    private void Awake()
    {
        zonas = player.GetComponent<StatusPlayer>();

    }
    public void iniciarQuest()
    {
        float distancia = Vector3.Distance(minhaPosicao.transform.position, player.transform.position);
        if (distancia < 4)
        {
            iniciada = true;
            objTexto1.SetActive(true);
            objTexto2.SetActive(true);
            text.text = "Saudações recruta, por favor ";
            text2.text = "faça a Ronda no forte.";


        }



    }
    public void questInciada()
    {
        text.text = "Lugares restantes: " + zonas.zonas + "/4";
        objTexto2.SetActive(false);
    }

    private void Update()
    {

        if ((zonas.zonas == 4) && (timer < 7))
        {
            print(timer);

            objTexto2.SetActive(true);


            timer += Time.deltaTime;


            if (timer <= 5)
            {
                text.text = "Missão Concluida!! ";
                text2.text = "Retorne ao Capitão.";
            }
            else if (timer > 5)
            {
                objTexto1.SetActive(false);
                objTexto2.SetActive(false);
            }



        }
    }
}



