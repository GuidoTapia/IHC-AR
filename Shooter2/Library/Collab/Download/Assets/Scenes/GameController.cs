using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{   

    public GameObject shooter;
    public GameObject dios;
    public GameObject textoDesconexion;
    public Text texto;

    public GameObject myShooter;
    public GameObject myDios;

    public PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        // PV = GetComponent<PhotonView>();
        VariablesConfig.etapaGame = 3; // 3: Juego

        Invoke("iniciarJuego", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("TIPO DE JUEGO: " + VariablesConfig.tipoDeJuego );
        Debug.Log("SALIO??  " + VariablesConfig.salio);

        if (VariablesConfig.salio)
        {
            VariablesConfig.nTotalJugadores--;
            Debug.Log("Un soldado ha dejado el campo de batalla GC");
            
            GameObject canvas = GameObject.FindGameObjectWithTag("PCanvas").gameObject;
            texto = canvas.transform.Find("caida").GetComponent<Text>();
            texto.text = "Un soldado ha caido";
            Debug.Log("CAMBIANDO EL TEXTO");
            VariablesConfig.salio = false;  
            Debug.Log("INKE BORRAR TEXTO");
            Invoke("borrarTextoDesconeccion", 3f);
            Debug.Log("CAIDA 1");
            PV.RPC("recuperarCaida", RpcTarget.All, VariablesConfig.idJugador);
            Debug.Log("CAIDA 2");
            
        }
    }

    void borrarTextoDesconeccion(){
        texto.text = " ";
    }

    void iniciarJuego(){


        foreach(Vector3 x in VariablesConfig.enemigosGlobalPos){
            PhotonNetwork.Instantiate("PHenemigo", x, Quaternion.identity, 0);
        }
        foreach(Vector3 x in VariablesConfig.dianasGlobalPos){
            PhotonNetwork.Instantiate("PHdiana", x, Quaternion.identity, 0);
        }

        GameObject initCam = GameObject.FindGameObjectWithTag("InitCam").gameObject;
        initCam.SetActive(false);
        if(VariablesConfig.tipoDeJuego  == 0){
            GameObject Player = Instantiate(shooter, new Vector3(0 , 0.5f, 0), Quaternion.Euler(90,90,0));
        }
        else if(VariablesConfig.tipoDeJuego  == 1){
            if(VariablesConfig.idJugador == 1){
                myShooter = PhotonNetwork.Instantiate("PHPlayer", new Vector3(0 , 0.5f, 0), Quaternion.Euler(90,90,0), 0);
                myDios = GameObject.FindGameObjectWithTag("God");
                // Debug.Log("PLAYER INSTANCIADO");
            }
            else{
                myDios = PhotonNetwork.Instantiate("PHGod", new Vector3(0 , 15, 0), Quaternion.Euler(90,120,0), 0);
                myShooter = GameObject.FindGameObjectWithTag("Player");
                // Debug.Log("DIOS INSTANCIADO");
            }
        }
        
    }

    [PunRPC]
    void recuperarCaida(int myID){
        VariablesConfig.idJugadoresDisponibles.Add(myID);
        Debug.Log("ENTRE A RPC DE RECUPERAR CAIDA");
        VariablesConfig.nBalasJugador = 12;
        Invoke("contarJugadores",0.5f);
    }

    void contarJugadores(){
        foreach(int x in VariablesConfig.idJugadoresDisponibles){
         if(x == 1){
             VariablesConfig.idJugadoresDisponibles.Clear();
             return;
         }   
        }
        
        Debug.Log("ENTRE A FUNCION CONTARJUGADORES");
        myShooter = GameObject.FindGameObjectWithTag("Player");

        if(VariablesConfig.idJugador == 2){
            Debug.Log("ENTRE A PRIMER IF");
            VariablesConfig.idJugador = 1;
            myDios.transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
            myDios.gameObject.transform.Find("Canvas").gameObject.SetActive(false);
            myShooter.gameObject.transform.Find("Canvas").gameObject.SetActive(true);
            GameObject camaraShooter = GameObject.FindGameObjectWithTag("PCamera");
            camaraShooter.GetComponent<Camera>().enabled = true;
            VariablesConfig.idJugadoresDisponibles.Clear();
            Debug.Log("TERMINE SEGUNDO IF");
            return;
        }
        else if(VariablesConfig.idJugador == 3){
            VariablesConfig.idJugador = 1;
            myDios.transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
            myDios.gameObject.transform.Find("Canvas").gameObject.SetActive(false);
            myShooter.transform.Find("Main Camera").gameObject.GetComponent<Camera>().enabled = true;
            myShooter.gameObject.transform.Find("Canvas").gameObject.SetActive(true);
            VariablesConfig.idJugadoresDisponibles.Clear();
            return;
        }
        else if(VariablesConfig.idJugador == 4){
            VariablesConfig.idJugador = 1;
            myDios.transform.Find("Camera").gameObject.GetComponent<Camera>().enabled = false;
            myDios.gameObject.transform.Find("Canvas").gameObject.SetActive(false);
            myShooter.transform.Find("Main Camera").gameObject.GetComponent<Camera>().enabled = true;
            myShooter.gameObject.transform.Find("Canvas").gameObject.SetActive(true);
            VariablesConfig.idJugadoresDisponibles.Clear();
            return;
        }
    }
}
