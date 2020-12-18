using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Wikitude;
using Photon.Pun;

[System.Serializable]

public class TutoMulti : MonoBehaviour
{

    //variables del juego
    //UI JUEGO
    public Button construirButton;
    public Button bloquearButton;
    public Text instruccion;
    //FUNCIONAL JUEGO
    public GameObject trackable;

    public GameObject paredGame;
    public GameObject torreInicioGame;
    public GameObject torreFinGame;
    public GameObject safeZoneGame;

    private GameObject safeZoneAuxGame;
    private GameObject[] torresGame;
    private GameObject[] paredesGame;

    private bool SaveZoneinImageGame;
    private int cantidadTorresGame;
    private int torresReconocidasGame;


    private int paredesConstruidasGame;
    private int flagGame;
    PhotonView PV;
    Text texto;


    private int totalJugadores;
    private int nTorresParaConstruir;


    //fin de las variables del juego


    private void Start()
    {   
        
        totalJugadores = AutoLobby.playersCount;

        PV = GetComponent<PhotonView>();
        construirButton.gameObject.SetActive(false);
        bloquearButton.gameObject.SetActive(false);
        instruccion.gameObject.SetActive(true);

        //inicializacion de variables del juego
        flagGame = 0;
        torresReconocidasGame = 0;
        paredesConstruidasGame = 0;
        cantidadTorresGame = 0;

        paredesConstruidasGame = 0;
        paredesGame = new GameObject[64];
        torresGame = new GameObject[64];

    }

    void Update()
    {
        if(cantidadTorresGame == totalJugadores){
            construirButton.gameObject.SetActive(true);
            instruccion.text = "Listo! Ahora puedes construir paredes";
        }

        if (VariablesConfig.salio)
        {
            VariablesConfig.nTotalJugadores--;
            Debug.Log("Un soldado ha dejado el campo de batalla");
            // GameObject texto = Instantiate(textoDesconexion, textoDesconexion.transform.position,Quaternion.identity);
            GameObject canvas = GameObject.FindGameObjectWithTag("PCanvas").gameObject;
            texto = canvas.transform.Find("caida").GetComponent<Text>();
            texto.text = "Un soldado ha caido";
            VariablesConfig.salio = false;  
            // Destroy(texto,3f);
            Invoke("borrarTextoDesconeccion", 3f);
            
        }
        
    }

    void borrarTextoDesconeccion(){
        texto.text = " ";
    }

    public void OnImageLost(ImageTarget target)
    {
        if (target.Name.StartsWith("safZon"))
        {
            construirButton.gameObject.SetActive(false);
            bloquearButton.gameObject.SetActive(false);
            instruccion.text = "Pon el marcador de Zona Inicial";

            safeZoneAuxGame.transform.parent = null;
            safeZoneAuxGame.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            for (int i = 0; i < cantidadTorresGame; i++)
            {
                torresGame[i].transform.parent = null;
                torresGame[i].transform.position -= safeZoneAuxGame.transform.position;
            }
            for (int i = 0; i < paredesConstruidasGame; i++)
            {
                paredesGame[i].transform.parent = null;
                paredesGame[i].transform.position -= safeZoneAuxGame.transform.position;
            }
            Destroy(safeZoneAuxGame);
            SaveZoneinImageGame = false;
        }
        else if (true)
        {
            Invoke("PosMarcadores", 0.1f);
        }
    }

    public List<Instruccion> instrucciones;
    // Start is called before the first frame update

    public void OnImageRecognized(ImageTarget target)
    {
        Invoke("PosMarcadores", 0.1f);
    }


    void PosMarcadores()
    {
        Debug.Log("Detecta un marcador");
        Transform[] allTransforms = trackable.GetComponentsInChildren<Transform>();
        List<Transform> markers = new List<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.parent.gameObject == trackable)
                markers.Add(t);
        }
        for (int i = cantidadTorresGame; i < cantidadTorresGame + torresReconocidasGame; i++)
        {
            Destroy(torresGame[i]);
        }

        torresReconocidasGame = 0;

        for (int i = 0; i < markers.Count; i++)
        {
            if (markers[i].gameObject.name.StartsWith("safZone"))
            {
                if(flagGame == 0){
                    instruccion.text = "Pon el marcador de Torre y utiliza el Botón de Fijar";
                    bloquearButton.gameObject.SetActive(true);
                }
                else if (cantidadTorresGame == totalJugadores){
                    construirButton.gameObject.SetActive(true);
                }

                if (SaveZoneinImageGame)
                {
                }
                else
                {
                    SaveZoneinImageGame = true;
                    safeZoneAuxGame = Instantiate(safeZoneGame, markers[i].position, markers[i].rotation);
                    // safeZoneAux.transform.localScale = new Vector3(10f,10f,10f);
                    safeZoneAuxGame.transform.localScale = safeZoneAuxGame.transform.localScale / 10;
                    safeZoneAuxGame.transform.parent = markers[i];
                    for (int j = 0; j < cantidadTorresGame; j++)
                    {
                        torresGame[j].transform.parent = safeZoneAuxGame.transform;
                    }
                    for (int j = 0; j < paredesConstruidasGame; j++)
                    {
                        paredesGame[j].transform.parent = safeZoneAuxGame.transform;
                    }
                }
            }
            else if (markers[i].gameObject.name.StartsWith("tower") && SaveZoneinImageGame && flagGame == 0)
            {
                torresGame[cantidadTorresGame + torresReconocidasGame] = Instantiate(torreInicioGame, markers[i].position, markers[i].rotation);
                torresGame[cantidadTorresGame + torresReconocidasGame].transform.parent = markers[i];
                torresReconocidasGame++;
            }
        }
    }
    public void BotonIzquierdo()
    {
        if (flagGame == 0)
        {
            guardarTorres();
            flagGame = 1;
            instruccion.text = "Esperando a los demás jugadores ...";
            bloquearButton.gameObject.SetActive(false);
            

        }

    }

    
    [PunRPC]
    void BDerechoRPC(){
        instruccion.text = "Perfecto!";
        construirButton.GetComponentInChildren<Text>().text = "Empezar el juego";
        if (flagGame == 1)
        {
            Invoke("giftWraper", 0.1f);
        }
        else if (flagGame == 2){
            PhotonNetwork.LoadLevel(6);
        }

    }



    public void BotonDerecho()
    {
        PV.RPC("BDerechoRPC", RpcTarget.All);
    }

    [PunRPC]
    void updateElementosTutorial(int id, int nElementos, int flag){
        if (flag == 0)
        {
            GameObject torre = PhotonView.Find(id).gameObject;
            if (cantidadTorresGame < nElementos)
            {
                torresGame[cantidadTorresGame] = torre;
                cantidadTorresGame++;
                Debug.Log("TORRE AÑADIDA");
            }
        }        
    }


    public void guardarTorres()
    {
        for (int i = cantidadTorresGame; i < cantidadTorresGame + torresReconocidasGame; i++)
        {
            torresGame[i].transform.parent = safeZoneAuxGame.transform;            
        }

        cantidadTorresGame += torresReconocidasGame;

        GameObject nuevo = PhotonNetwork.Instantiate("PHTorre", torresGame[cantidadTorresGame-1].transform.position, torresGame[cantidadTorresGame-1].transform.rotation);
        PhotonView pvN = nuevo.GetComponent<PhotonView>();
        Debug.Log(pvN.ViewID);
        PV.RPC("updateElementosTutorial", RpcTarget.All, pvN.ViewID, cantidadTorresGame,0);
        

        torresReconocidasGame = 0;
        Invoke("PosMarcadores", 0.1f);
    }

    public void giftWraper()
    {
        flagGame = 2;
        guardarTorres();
        int iMinimo = 0;
        double dMinimo = 100000.0d;
        for (int i = 0; i < cantidadTorresGame; i++)
        {
            torresGame[i].transform.rotation = new Quaternion(0, 0, 0, 0);
            torresGame[i].transform.position = new Vector3(torresGame[i].transform.position.x, 0.0f, torresGame[i].transform.position.z);
            if (torresGame[i].transform.position.magnitude < dMinimo)
            {
                iMinimo = i;
                dMinimo = torresGame[i].transform.position.magnitude;
            }
        }
        Vector3 positionSwap = torresGame[0].transform.position;
        Quaternion rotationSwap = torresGame[0].transform.rotation;
        torresGame[0].transform.position = torresGame[iMinimo].transform.position;
        torresGame[0].transform.rotation = torresGame[iMinimo].transform.rotation;
        torresGame[iMinimo].transform.position = positionSwap;
        torresGame[iMinimo].transform.rotation = rotationSwap;

        Vector3 positionAux = torresGame[0].transform.position / 2;
        for (int i = 1; i < cantidadTorresGame; i++)
        {
            iMinimo = i;
            dMinimo = Vector3.Distance(positionAux, torresGame[i].transform.position);
            for (int j = i + 1; j < cantidadTorresGame; j++)
            {
                if (Vector3.Distance(positionAux, torresGame[j].transform.position) < dMinimo)
                {
                    iMinimo = j;
                    dMinimo = Vector3.Distance(positionAux, torresGame[j].transform.position);
                }
            }
            positionSwap = torresGame[i].transform.position;
            rotationSwap = torresGame[i].transform.rotation;
            torresGame[i].transform.position = torresGame[iMinimo].transform.position;
            torresGame[i].transform.rotation = torresGame[iMinimo].transform.rotation;
            torresGame[iMinimo].transform.position = positionSwap;
            torresGame[iMinimo].transform.rotation = rotationSwap;

            paredesGame[i - 1] = Instantiate(paredGame);
            paredesGame[i - 1].transform.position = (torresGame[i].transform.position + torresGame[i - 1].transform.position) / 2;
            paredesGame[i - 1].transform.LookAt(torresGame[i].transform.position);
            paredesGame[i - 1].transform.localScale = new Vector3(0.1f, 0.5f, Vector3.Distance(torresGame[i].transform.position, torresGame[i - 1].transform.position));
            paredesGame[i - 1].transform.parent = safeZoneAuxGame.transform;
            positionAux = (torresGame[i].transform.position + torresGame[i - 1].transform.position) / 2;
            paredesConstruidasGame++;
        }

    }
    void adjustWall(List<Transform> markers, GameObject muro, Vector3 p, Vector3 ls, Quaternion r)
    {
        torreInicioGame.transform.LookAt(markers[0].transform.position);
        torreFinGame.transform.LookAt(markers[1].transform.position);
        float distance = Vector3.Distance(torreFinGame.transform.position, torreInicioGame.transform.position);
        p = torreInicioGame.transform.position + distance / 2 * torreInicioGame.transform.forward + 0.5f * Vector3.up;
        r = torreInicioGame.transform.rotation;
        ls = new Vector3(muro.transform.localScale.x, muro.transform.localScale.y, distance);
    }
    void createWall(Vector3 posInicio, Vector3 posFin, int index)
    {
        float distance = Vector3.Distance(posInicio, posFin);
        paredesGame[index].transform.position = (posInicio + posFin) / 2;
        paredesGame[index].transform.LookAt(posFin);
        paredesGame[index].transform.localScale = new Vector3(0.1f, 0.5f, distance);
    }


    
}
