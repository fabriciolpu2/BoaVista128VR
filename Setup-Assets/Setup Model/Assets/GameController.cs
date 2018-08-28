using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{

    public GameObject player;
    public GameObject[] objetos;
    public GameObject UIMenu;
    public Text texto;
    public Text txtItensCount;
    public int itensCount = 0;
    public int qtdTotalItens = 3;
    public GameObject forrestGump;
    // Use this for initialization
    public AudioClip[] gump;

    public GameObject objetoParaInvestigar;
    public bool investigando;
    //public bool podeExaminar;
    public GameObject btnInvestigar;
    public AudioClip missaoCumprida;
    AudioSource audioS;

    void Awake() {
       forrestGump.SetActive(false);
       btnInvestigar.SetActive(false);
        audioS = player.GetComponent<AudioSource>();
        audioS.clip = missaoCumprida;
        
    }
    
    /* 
     * 
     * 
     * 
     */

    void InstaciaForrest()
    {
        forrestGump.SetActive(true);        
        AudioSource audio = forrestGump.GetComponent<AudioSource>();
        //audio.clip = gump[2];
        //audio.Play();
        audio.enabled = true;
    }
    // Update is called once per frame
    void FixedUpdate() {
        if (itensCount >= 1)
        {
            InstaciaForrest();
            print("Fim Jogo");
        }       
    }
    private void OnTriggerStay(Collider other)
    {
        btnInvestigar.SetActive(true);
        objetoParaInvestigar = other.gameObject;
    }
    private void OnTriggerExit(Collider other)
    {
        btnInvestigar.SetActive(false);
        objetoParaInvestigar = null;
        investigando = false;
    }

    // Examinar Ojbetos
    public void Investigar()
    {
        if (objetoParaInvestigar != null)
        {
            investigando = true;
            if (objetoParaInvestigar.tag == "objeto")
            {
                print("examinando: " + objetoParaInvestigar.name);
                //Tocar Musica
                itensCount++;
                txtItensCount.text = "Intens: " + itensCount;
                texto.text = "   paRabeNs!!! \n Foco da Dengue \n  destRUido";
                Destroy(objetoParaInvestigar.gameObject);
                objetoParaInvestigar = null;
                investigando = false;
                btnInvestigar.SetActive(false);
                StartCoroutine(TempoTxt());
                
            }
        }        

    }
    public void Iniciar()
    {
        UIMenu.SetActive(false);
    }
    IEnumerator TempoTxt()
    {
        // Tocar Musica
        audioS.Play();
        yield return new WaitForSeconds(5);
        texto.text = "";
    }

    public void EscolherPersonagem()
    {
        new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
    }
    public void Reiniciar()
    {
        //print("Reiniciar");
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
    }
}
