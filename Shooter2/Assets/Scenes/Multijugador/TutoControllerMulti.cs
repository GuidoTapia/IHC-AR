using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Wikitude;
using Photon.Pun;

[System.Serializable]

public class TutoControllerMulti : MonoBehaviour
{

    //variables del juego
    //UI JUEGO
    public Button construirButton;
    public Button bloquearButton;
    public Text instruccion;
    //FUNCIONAL JUEGO
    public GameObject trackable;
    public GameObject enemigo;
    public GameObject laser;
    public GameObject diana;
    public GameObject paredGame;
    public GameObject torreInicioGame;
    public GameObject torreFinGame;
    public GameObject safeZoneGame;
    public GameObject laserGame;
    public GameObject enemigoGame;
    private GameObject safeZoneAuxGame;
    static public GameObject[] torresGame;
    static public GameObject[] paredesGame;
    static public GameObject[] lasersGame;
    static public GameObject[] enemigosGame;
    static public GameObject[] dianasGame;
    private bool SaveZoneinImageGame;
    private int cantidadTorresGame;
    private int torresReconocidasGame;
    private int cantidadLasersGame;
    public int cantidadDianasGame;
    private int lasersReconocidosGame;
    private int dianasReconocidasGame;
    public int cantidadEnemigosGame;
    private int enemigosReconocidosGame;
    private int paredesConstruidasGame;
    private int flagGame;
    static public PhotonView PV;
    Text texto;

    Color temp;
    vidaJugador playerVida;
    GameObject player;
    bool flagPantalla = true;


    //fin de las variables del juego


    private void Start()
    {
        VariablesConfig.etapaGame = 2; // 2: Construccion
        PV = GetComponent<PhotonView>();
        
        construirButton.gameObject.SetActive(true);
        bloquearButton.gameObject.SetActive(true);
        instruccion.gameObject.SetActive(true);

    
        

        //inicializacion de variables del juego
        flagGame = 0;
        torresReconocidasGame = 0;
        paredesConstruidasGame = 0;
        cantidadTorresGame = 0;
        lasersReconocidosGame = 0;
        cantidadEnemigosGame = 0;
        cantidadLasersGame = 0;
        enemigosReconocidosGame = 0;
        cantidadDianasGame = 0;
        dianasReconocidasGame = 0;
        paredesConstruidasGame = 0;
        paredesGame = new GameObject[64];
        torresGame = new GameObject[64];
        lasersGame = new GameObject[64];
        enemigosGame = new GameObject[64];
        dianasGame = new GameObject[64];
    }

    void Update()
    {
        if(VariablesConfig.etapaGame == 3 && flagPantalla){
            player = GameObject.FindGameObjectWithTag("Player");
            playerVida = player.GetComponent<vidaJugador>();
            temp = playerVida.pantallaRoja.color;
            flagPantalla = false;
        }

        if (flagGame == 1)
        {
            posicionarLasers();
        }
        

        if (VariablesConfig.salio && VariablesConfig.etapaGame == 2)
        {
            VariablesConfig.nTotalJugadores--;
            // Debug.Log("Un soldado ha dejado el campo de batalla");
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
            for (int i = 0; i < cantidadEnemigosGame; i++)
            {
                enemigosGame[i].transform.parent = null;
                enemigosGame[i].transform.position -= safeZoneAuxGame.transform.position;
            }
            for (int i = 0; i < cantidadLasersGame; i++)
            {
                lasersGame[i].transform.parent = null;
                lasersGame[i].transform.position -= safeZoneAuxGame.transform.position;
            }
            for (int i = 0; i < cantidadDianasGame; i++)
            {
                dianasGame[i].transform.parent = null;
                dianasGame[i].transform.position -= safeZoneAuxGame.transform.position;
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

    void posicionarLasers()
    {
        for (int i = cantidadLasersGame; i < cantidadLasersGame + lasersReconocidosGame; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(lasersGame[i].transform.parent.position.x, 0.2f, lasersGame[i].transform.parent.position.z), new Vector3(-lasersGame[i].transform.parent.forward.x, 0f, -lasersGame[i].transform.parent.forward.z), out hit))
            {
                if (hit.collider && hit.transform.gameObject.tag == "Wall")
                {
                    lasersGame[i].transform.position = hit.point;
                    /*Transform[] laserTransform = lasersGame[i].GetComponentsInChildren<Transform>();
                    foreach (Transform t in laserTransform)
                    {
                        if (Physics.Raycast(lasersGame[i].transform.position, lasersGame[i].transform.forward, out hit))
                        {
                            t.position = (hit.point + lasersGame[i].transform.position) / 2;
                            t.LookAt(hit.point);
                            t.localScale = new Vector3(t.localScale.x, t.localScale.y, Vector3.Distance(lasersGame[i].transform.position, hit.point));
                        }
                    }*/
                }
                else if (!hit.collider)
                {
                    lasersGame[i].transform.position = lasersGame[i].transform.parent.position;
                }
            }
            // else lr.SetPosition(1, transform.forward*5000);
        }
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
        for (int i = cantidadEnemigosGame; i < cantidadEnemigosGame + enemigosReconocidosGame; i++)
        {
            Destroy(enemigosGame[i]);
        }
        for (int i = cantidadLasersGame; i < cantidadLasersGame + lasersReconocidosGame; i++)
        {
            Destroy(lasersGame[i]);
        }
        for (int i = cantidadDianasGame; i < cantidadDianasGame + dianasReconocidasGame; i++)
        {
            Destroy(dianasGame[i]);
        }
        torresReconocidasGame = 0;
        enemigosReconocidosGame = 0;
        lasersReconocidosGame = 0;
        dianasReconocidasGame = 0;

        for (int i = 0; i < markers.Count; i++)
        {
            if (markers[i].gameObject.name.StartsWith("safZone"))
            {
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
                    for (int j = 0; j < cantidadEnemigosGame; j++)
                    {
                        enemigosGame[j].transform.parent = safeZoneAuxGame.transform;
                    }
                    for (int j = 0; j < cantidadLasersGame; j++)
                    {
                        lasersGame[j].transform.parent = safeZoneAuxGame.transform;
                    }
                    for (int j = 0; j < cantidadDianasGame; j++)
                    {
                        dianasGame[j].transform.parent = safeZoneAuxGame.transform;
                    }
                }
            }
            else if (markers[i].gameObject.name.StartsWith("tower") && SaveZoneinImageGame && flagGame == 0)
            {
                torresGame[cantidadTorresGame + torresReconocidasGame] = Instantiate(torreInicioGame, markers[i].position, markers[i].rotation);
                torresGame[cantidadTorresGame + torresReconocidasGame].transform.parent = markers[i];
                torresReconocidasGame++;
            }
            else if (markers[i].gameObject.name.StartsWith("enemy") && SaveZoneinImageGame && flagGame == 1)
            {
                enemigosGame[cantidadEnemigosGame + enemigosReconocidosGame] = Instantiate(enemigo, markers[i].position, markers[i].rotation);
                enemigosGame[cantidadEnemigosGame + enemigosReconocidosGame].transform.parent = markers[i];
                enemigosReconocidosGame++;
            }
            else if (markers[i].gameObject.name.StartsWith("laserMod") && SaveZoneinImageGame && flagGame == 1)
            {
                lasersGame[cantidadLasersGame + lasersReconocidosGame] = Instantiate(laser, markers[i].position, markers[i].rotation);
                lasersGame[cantidadLasersGame + lasersReconocidosGame].transform.localScale = lasersGame[cantidadLasersGame + lasersReconocidosGame].transform.localScale / 4;
                lasersGame[cantidadLasersGame + lasersReconocidosGame].transform.parent = markers[i];
                lasersReconocidosGame++;
            }
            else if (markers[i].gameObject.name.StartsWith("target") && SaveZoneinImageGame && flagGame == 1)
            {
                dianasGame[cantidadDianasGame + dianasReconocidasGame] = Instantiate(diana, markers[i].position, markers[i].rotation);
                dianasGame[cantidadDianasGame + dianasReconocidasGame].transform.parent = markers[i];
                dianasReconocidasGame++;
            }
        }
    }
    public void BotonIzquierdo()
    {
        if (flagGame == 0)
        {
            guardarTorres();
        }
        else
        {
            guardarLasersEnemigos();
        }
    }

    
    [PunRPC]
    void BDerechoRPC(){
        instruccion.text = "Fija algunos obstáculos y empieza el juego";
        construirButton.GetComponentInChildren<Text>().text = "Empezar el juego";
        if (flagGame == 0)
        {
            Invoke("giftWraper", 0.1f);
        }
        else
        {
            escalaExportar();
        }
    }



    public void BotonDerecho()
    {
        PV.RPC("BDerechoRPC", RpcTarget.All);
    }

    [PunRPC]
    void updateElementos(int id, int nElementos, int flag){
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
        else if (flag == 1)
        {
            GameObject enemigo = PhotonView.Find(id).gameObject;
            if (cantidadEnemigosGame < nElementos)
            {
                enemigosGame[cantidadEnemigosGame] = enemigo;
                cantidadEnemigosGame++;
                Debug.Log("ENEMIGO AÑADIDO");
            }
        }
        else if (flag == 2)
        {
            GameObject laser = PhotonView.Find(id).gameObject;
            if (cantidadLasersGame < nElementos)
            {
                lasersGame[cantidadLasersGame] = laser;
                cantidadLasersGame++;
                Debug.Log("LASER AÑADIDO");
            }
        }
        else if (flag == 3)
        {
            GameObject diana = PhotonView.Find(id).gameObject;
            if (cantidadDianasGame < nElementos)
            {
                dianasGame[cantidadDianasGame] = diana;
                cantidadDianasGame++;
                Debug.Log("DIANA AÑADIDA");
            }
        }

        
    }

    public void guardarLasersEnemigos()
    {
        for (int i = cantidadEnemigosGame; i < cantidadEnemigosGame + enemigosReconocidosGame; i++)
        {
            enemigosGame[i].transform.parent = safeZoneAuxGame.transform;
            enemigosGame[i].transform.position = new Vector3(enemigosGame[i].transform.position.x, 0.09f, enemigosGame[i].transform.position.z);
            enemigosGame[i].transform.rotation = new Quaternion(0f,0f,0f,0f);
        }
        cantidadEnemigosGame += enemigosReconocidosGame;

        if(enemigosReconocidosGame != 0){
            GameObject nEnemigo = PhotonNetwork.Instantiate("PHenemigo", enemigosGame[cantidadEnemigosGame-1].transform.position, enemigosGame[cantidadEnemigosGame-1].transform.rotation);
            PhotonView pvNE = nEnemigo.GetComponent<PhotonView>();
            Debug.Log(pvNE.ViewID);
            PV.RPC("updateElementos", RpcTarget.All, pvNE.ViewID, cantidadEnemigosGame,1);
        }

        enemigosReconocidosGame = 0;

        for (int i = cantidadLasersGame; i < cantidadLasersGame + lasersReconocidosGame; i++)
        {
            lasersGame[i].transform.parent = safeZoneAuxGame.transform;
        }
        cantidadLasersGame += lasersReconocidosGame;

        if(lasersReconocidosGame != 0){
            GameObject nLaser = PhotonNetwork.Instantiate("PHParedLaser", lasersGame[cantidadLasersGame-1].transform.position, lasersGame[cantidadLasersGame-1].transform.rotation);
            PhotonView pvNL = nLaser.GetComponent<PhotonView>();
            Debug.Log(pvNL.ViewID);
            PV.RPC("updateElementos", RpcTarget.All, pvNL.ViewID, cantidadLasersGame,2);
        }

        lasersReconocidosGame = 0;

        for (int i = cantidadDianasGame; i < cantidadDianasGame + dianasReconocidasGame; i++)
        {
            dianasGame[i].transform.parent = safeZoneAuxGame.transform;
            dianasGame[i].transform.position = new Vector3(dianasGame[i].transform.position.x, 0.07f, dianasGame[i].transform.position.z);
            dianasGame[i].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        cantidadDianasGame += dianasReconocidasGame;

        if(dianasReconocidasGame != 0){
            GameObject nuevo = PhotonNetwork.Instantiate("PHdiana", dianasGame[cantidadDianasGame-1].transform.position,dianasGame[cantidadDianasGame-1].transform.rotation);
            PhotonView pvN = nuevo.GetComponent<PhotonView>();
            Debug.Log(pvN.ViewID);
            PV.RPC("updateElementos", RpcTarget.All, pvN.ViewID, cantidadDianasGame,3);
        }

        dianasReconocidasGame = 0;

        Invoke("PosMarcadores", 0.1f);
    }
    public void escalaExportar()
    {
        safeZoneAuxGame.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        for (int j = 0; j < cantidadTorresGame; j++)
        {
            if (torresGame[j] != null)
            {
                torresGame[j].transform.parent = null;
                torresGame[j].transform.position -= safeZoneAuxGame.transform.position;
                torresGame[j].transform.localScale *= 20;
                torresGame[j].transform.position *= 20;
                DontDestroyOnLoad(torresGame[j]);
            }
        }
        for (int j = 0; j < paredesConstruidasGame; j++)
        {
            if (paredesGame[j] != null)
            {
                paredesGame[j].transform.parent = null;
                paredesGame[j].transform.position -= safeZoneAuxGame.transform.position;
                paredesGame[j].transform.localScale *= 20;
                paredesGame[j].transform.position *= 20;
                DontDestroyOnLoad(paredesGame[j]);
            }
        }
        for (int j = 0; j < cantidadEnemigosGame; j++)
        {
            if (enemigosGame[j] != null)
            {
                enemigosGame[j].transform.parent = null;
                enemigosGame[j].transform.position -= safeZoneAuxGame.transform.position;
                // enemigos[j].transform.localScale *= 20;
                enemigosGame[j].transform.position *= 20;
                VariablesConfig.enemigosGlobalPos.Add(enemigosGame[j].transform.position);
                // DontDestroyOnLoad(enemigosGame[j]);
            }
        }
        for (int j = 0; j < cantidadLasersGame; j++)
        {
            if (lasersGame[j] != null)
            {
                lasersGame[j].transform.parent = null;
                lasersGame[j].transform.position -= safeZoneAuxGame.transform.position;
                lasersGame[j].transform.localScale *= 5;
                lasersGame[j].transform.position *= 20;
                DontDestroyOnLoad(lasersGame[j]);
            }
        }
        for (int j = 0; j < cantidadDianasGame; j++)
        {
            if (dianasGame[j] != null)
            {
                dianasGame[j].transform.parent = null;
                dianasGame[j].transform.position -= safeZoneAuxGame.transform.position;
                // lasers[j].transform.localScale *= 20;
                dianasGame[j].transform.position *= 20;
                VariablesConfig.dianasGlobalPos.Add(dianasGame[j].transform.position);
                // DontDestroyOnLoad(dianasGame[j]);
            }
        }

        VariablesConfig.cantidadEnemigos = cantidadEnemigosGame;
        VariablesConfig.cantidadDianas = cantidadDianasGame;
        
        DontDestroyOnLoad(this);
        // SceneManager.LoadScene("Juego");
        PhotonNetwork.LoadLevel(4);
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
        PV.RPC("updateElementos", RpcTarget.All, pvN.ViewID, cantidadTorresGame,0);
        

        torresReconocidasGame = 0;
        Invoke("PosMarcadores", 0.1f);
    }
    public void giftWraper()
    {
        flagGame = 1;
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

     [PunRPC]
    void restarEnemigo(int idEnemy, int tag){
            
            GameObject enemigo = PhotonView.Find(idEnemy).gameObject;
            Destroy(enemigo);
            if(tag==2){
                Debug.Log("ENEMIGO ABATIDO!!!!!!!");
                if(VariablesConfig.idJugador == 1){
                    temp.a= 0.0f;
                    playerVida.pantallaRoja.color = temp;
                }
                VariablesConfig.cantidadEnemigos--;
            }
            else{
                
                Debug.Log("DIANA ABATIDA!!!!!!!");
                VariablesConfig.cantidadDianas --;
            }
    }


    
}
