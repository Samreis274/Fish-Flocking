using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockinManager : MonoBehaviour
{
    //Criando varialvel para gameobject
    public GameObject fishPrefab;
    //Varial de numeros de peixes
    public int numfish = 20;
    //Array dos peixes
    public GameObject[] allfish;
    //Velocidade da movimentaçao espacial do peixe
    public Vector3 swinLimits = new Vector3(5, 5, 5);
    // cria uma variavel vector3
    public Vector3 goalPos;


    [Header("Configurações do Cardume")]
    //Range da velocidade minima do cardume
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    //Range da Velocidade maxima do cardume
    [Range(0.0f, 5.0f)]
    public float maxSpeed;
    //Range pontos maximos entre os peixes
    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    //Range velocidade da rotação do cardume
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;


    void Start()
    {
        //adiciona os peixes no array
        allfish = new GameObject[numfish];
        //for de contagem com os numero de peixes
        for (int i = 0; i < numfish; i++)
        {
            //Posicionamento dos peixes
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
            //Instacia o peixe na posição
            allfish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            //pegando componente flock do meu mymanager
            allfish[i].GetComponent<Flock>().mymanager = this;
                                                                
        }
        //Adiciona minha posicao no vector3 goalpos
        goalPos = this.transform.position;
    }

    void Update()
    {
        //pega minha posicao
        goalPos = this.transform.position;
        //cria uma condicao com random
        if(Random.Range(0,100)<10)
        //posicionamento dos peixes 
        goalPos = this.transform.position + new Vector3(Random.Range(-swinLimits.x, swinLimits.x),
                                                                Random.Range(-swinLimits.y, swinLimits.y),
                                                                Random.Range(-swinLimits.z, swinLimits.z));
    }
}
