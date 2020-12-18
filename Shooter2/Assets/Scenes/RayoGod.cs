using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RayoGod : MonoBehaviour
{      
    PhotonView PV;
    private float inicioDisparar;
    // Animator ayudaAnimation;
    

    // Start is called before the first frame update
    void Start()
    {
        
        PV = GetComponent<PhotonView>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(VariablesConfig.vidaJugador);
    }


    //TODO
    public void disparoVida(){
        // Debug.Log("DISPARO VIDA");
     float tiempoDisparo = 0.5f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit. collider && Time.time > inicioDisparar)
            {   
                inicioDisparar = Time.time + tiempoDisparo;
                Debug.Log("CHOCO CON ALGO");
                if(hit.transform.gameObject.tag == "Player"){
                    Debug.Log("DISTE VIDA");
                    PV.RPC("EfectoBueno", RpcTarget.All, 1, 10);
                    // EfectoBueno(1,10);
                }
                else{
                    // Debug.Log("ALGO MALO");
                    int tag = (int)Random.Range(0, 2);
                    if(tag == 0){
                        Debug.Log("MENOS VIDA");
                        PV.RPC("EfectoBueno", RpcTarget.All, 1, -15);
                        // EfectoBueno(1,-15);
                    }
                    else{
                        GameObject enemy = PhotonNetwork.Instantiate("PHenemigo",hit.point, Quaternion.identity);
                        enemy.transform.Translate (0,1.5f,0);
                        PV.RPC("EfectoBueno", RpcTarget.All, 3, -2);
                        Debug.Log("ENEMIGO");
                    }
                }         
            }
        }
    }

    void desactivarAnim(){
        VariablesConfig.ayudaAnimation.SetBool("verde",false);
        VariablesConfig.ayudaAnimation.SetBool("rojo",false);
    }  

    [PunRPC]
    void EfectoBueno(int tag, int val){
        if(val > 0){
            VariablesConfig.ayudaAnimation.SetBool("verde",true);
            Invoke("desactivarAnim", 0.1f);
        }
        else{
            VariablesConfig.ayudaAnimation.SetBool("rojo",true);
            Invoke("desactivarAnim", 0.1f);
        }

        if(tag == 1){
            VariablesConfig.vidaJugador += val;
        }
        else if(tag == 2){
            VariablesConfig.nBalasJugador += val;
        }
        else if(tag == 3){
            VariablesConfig.cantidadEnemigos++;
        }
    }

    //TODO
    public void disparoBalas(){
        Debug.Log("DISPARO BALAS");
     float tiempoDisparo = 0.5f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit. collider)
            {   
                if(hit.transform.gameObject.tag == "Player"){
                    PV.RPC("EfectoBueno", RpcTarget.All, 2, 1);
                    // EfectoBueno(2,1);
                    Debug.Log("DISTE BALAS");
                }
                else{
                    Debug.Log("ALGO MALO");
                    int tag = (int)Random.Range(0, 2);
                    if(tag == 0){
                        Debug.Log("MENOS BALAS");
                        PV.RPC("EfectoBueno", RpcTarget.All, 2, -2);
                        // EfectoBueno(2,-2);
                    }
                    else{
                        GameObject enemy = PhotonNetwork.Instantiate("PHenemigo",hit.point, Quaternion.identity);
                        enemy.transform.Translate (0,1.5f,0);
                        PV.RPC("EfectoBueno", RpcTarget.All, 3, -2);
                        Debug.Log("ENEMIGO");
                    }
                }       
            }
        }
    }

}
