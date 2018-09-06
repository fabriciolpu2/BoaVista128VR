using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FecharDash : MonoBehaviour {
    public GameObject information;
    public GameObject esseObjeto;

	// Use this for initialization
	void Start () {
        esseObjeto = information.GetComponent<GameObject>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void fecharDash()
    {
        esseObjeto.SetActive(false);
    }

}
/*Cria um script para o dash board que gire junto com o player
e feche ao clicar ou se distanciar


criar um script para o information que gire junto com o player e ao clicar abra o dashboard
*/
