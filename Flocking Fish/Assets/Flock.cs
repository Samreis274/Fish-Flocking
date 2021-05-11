using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    //Pega o flockinmanager e coloca em uma variavel
    public FlockinManager mymanager;
    //Variavel speed
    float speed;
    //Cria uma booleana com valor de false
    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        //Pegas as velocidade do Flockinmanager e coloca na variavel speed
        speed = Random.Range(mymanager.minSpeed, mymanager.maxSpeed);    
    }

    // Update is called once per frame
    void Update()
    {
        //Cria um bounds para limitar aonde eles podem nadar
        Bounds b = new Bounds(mymanager.transform.position, mymanager.swinLimits * 2);

        //Cria um raycast
        RaycastHit hit = new RaycastHit();
        //Novo vector de rotacao
        Vector3 direction = mymanager.transform.position - transform.position;

        //Limita onde o eles vao poder nadar
        if(!b.Contains(transform.position))
        {
            //Muda booleana para true
            turning = true;
            direction = mymanager.transform.position - transform.position;
        }
        //Condicao caso o raycast colida com o pilar
        else if(Physics.Raycast(transform.position, this.transform.forward * 50, out hit))
        {
            //Muda booleana para true
            turning = true;
            //vector para refletir a posicao do peixes
            direction = Vector3.Reflect(this.transform.forward, hit.normal);
        }
        else
            //Muda booleana para false
            turning = false;

        
        if (turning)
        {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                Quaternion.LookRotation(direction), 
                mymanager.rotationSpeed * Time.deltaTime);
        }
        else
        {
            //Verifica o random range é menor que 10
            if(Random.Range(0,100) < 10)
            {
                speed = Random.Range(mymanager.minSpeed, mymanager.maxSpeed);
            }
            //verifica o random range é menor que 20
            if (Random.Range(0,100) < 20)
            {
                //Aplica o metodo ApplyRules
                 ApplyRules();
            }
        }
       
        //coloca movimentação no peixe no eixo z
        transform.Translate(0, 0, Time.deltaTime * speed);  

    }

    void ApplyRules()
    {
        //Array de gameObject chamado gos
        GameObject[] gos;
        //pega o array do flockinmanager e coloca na variavel gos
        gos = mymanager.allfish;

        //Calculo central do cardume mais proximo
        Vector3 vcentre = Vector3.zero;
        //Evitar a colisão de todos os peixes
        Vector3 vavoid = Vector3.zero;
        //velocidade da rotação entre eles
        float gSpeed = 0.01f;
        //Distancia entre os agentes
        float nDistance;
        //Grupos mais proximos
        int groupSize = 0;

        //criaçao dos objetos e aplicaçao
        foreach(GameObject go in gos)
        {
            
            if(go != this.gameObject)
            {
                //calculo das distancias
                nDistance = Vector3.Distance(go.transform.position, this.transform.position);
                //verifica a distancia com os pontos maximos entre os peixes
                if (nDistance <= mymanager.neighbourDistance)
                {
                    //calcula o ponto central
                    vcentre += go.transform.position;
                    //adiciona um grupo
                    groupSize++;

                    if(nDistance < 1.0f)
                    {
                        //evita a colisão entre eles
                        vavoid = vavoid + (this.transform.position - go.transform.position);
                    }
                    //Recebe uma nova velocidade para fazer a rotação entre eles
                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed = gSpeed + anotherFlock.speed;
                }
            }
        }
        //verifica se groupSize é maior que 0
        if (groupSize > 0)
        {
            //calcula o centro 
            vcentre = vcentre / groupSize + (mymanager.goalPos - this.transform.position);

          
            //nova velocidade
            speed = gSpeed / groupSize;

            //novo vector de rotaçao
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                    Quaternion.LookRotation(direction), 
                    mymanager.rotationSpeed * Time.deltaTime);
        }
    }
}
