using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Logica_Arma;

public class Bala : MonoBehaviour
{
    [Header("Otros Scripts")]
    public GameObject arma;

    [Header("Propiedades de Bala")]
    public Rigidbody balaPrefab;
    public Transform lanzador;
    public float VelDisparo;
    public float tiempoDisparo;

    private float inicioDisparar;
    private Logica_Arma armaScript;
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        armaScript = arma.GetComponent<Logica_Arma>();
    }

    // Update is called once per frame
    void Update()
    {   
        
        // if(Input.GetButton("Fire1") && Time.time > inicioDisparar && armaScript.balasEnCartucho > 0 ){
        //    inicioDisparar = Time.time + tiempoDisparo; 
        //    Rigidbody balaPrefabInstance;
        //    balaPrefabInstance = Instantiate(balaPrefab,lanzador.position, Quaternion.identity);
        //    balaPrefabInstance.AddForce(lanzador.forward * 100 * VelDisparo);
        //    Destroy(balaPrefabInstance,3f);
        // }

    }

    public void Disparar(){
        if(Time.time > inicioDisparar && VariablesConfig.nBalasJugador > 0 ){
           inicioDisparar = Time.time + tiempoDisparo; 
           Rigidbody balaPrefabInstance;
           balaPrefabInstance = Instantiate(balaPrefab,lanzador.position, Quaternion.identity);
           balaPrefabInstance.AddForce(lanzador.forward * 100 * VelDisparo);
           Destroy(balaPrefabInstance,3f);
        }
    }

}
