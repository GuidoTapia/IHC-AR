using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Wikitude;

[System.Serializable]
public class Instruccion
{
    public string instruccion;
    public Texture prompt;
}

public class TutoController : MonoBehaviour
{
    //variable global de tutorial o del juego false para el tutorial true para el juego
    bool GLOBALTUTOJUEGO;

    //variables del tutorial
    List<string> msj = new List<string>();
    public GameObject trackable;
    public GameObject torre;
    public GameObject enemigo;
    public GameObject laser;
    public GameObject puntoPartida;
    public GameObject diana;
    public Button passButton;
    bool gotReward = false;
    bool cambio = false;
    bool buttonPassApeear = false;
    public Text mensaje;
    int countCorrect = 0;

    public string word = "tower";

    public RawImage screenPrompt;

    int currentInstruction = 0;
    //fin de variables del tutorial

    //variables del juego
    //UI JUEGO
    public Button construirButton;
    public Button bloquearButton;
    public Text instruccion;
    //FUNCIONAL JUEGO
    public GameObject paredGame;
    public GameObject torreInicioGame;
    public GameObject torreFinGame;
    public GameObject safeZoneGame;
    public GameObject laserGame;
    public GameObject enemigoGame;
    private GameObject safeZoneAuxGame;
    private GameObject[] torresGame;
    private GameObject[] paredesGame;
    private GameObject[] lasersGame;
    private GameObject[] enemigosGame;
    private GameObject[] dianasGame;
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


    //fin de las variables del juego

    public void PickRandomMarker()
    {
        if (instrucciones.Count != 0)
        {
            Debug.Log("Estamos en pick random con: " + instrucciones.Count);
            word = instrucciones[currentInstruction].instruccion;
            if (instrucciones.Count <= 3 && !buttonPassApeear)
            {
                passButton.gameObject.SetActive(true);
                buttonPassApeear = true;
            }
            screenPrompt.texture = instrucciones[currentInstruction].prompt;
            instrucciones.RemoveAt(currentInstruction);
        }
        else
        {
            if (!buttonPassApeear)
            {
                passButton.gameObject.SetActive(true);
                buttonPassApeear = true;
            }
        }
    }

    private void Start()
    {
        msj.Add("Coloca el marcador del dibujo en la pantalla!");
        msj.Add("Bien hecho! Retira ahora el marcador");
        GLOBALTUTOJUEGO = false;
        construirButton.gameObject.SetActive(false);
        bloquearButton.gameObject.SetActive(false);
        instruccion.gameObject.SetActive(false);
        passButton.gameObject.SetActive(false);

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
        if (GLOBALTUTOJUEGO)
        {
            if (flagGame == 1)
            {
                posicionarLasers();
            }
            getInput();
        }
    }

    public void OnImageLost(ImageTarget target)
    {
        if (GLOBALTUTOJUEGO) //si esta en el juego
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
        else // si esta en el tuto
        {
            mensaje.text = msj[0];
            if (gotReward)
            {
                if (instrucciones.Count == 0)
                {
                    //trackable.transform.parent.gameObject.SetActive(false);
                    if (cambio == true)
                    {
                        //aqui se cambia el bool para ver donde va si al juego o al tuto
                        //SceneManager.LoadScene("building");
                        GLOBALTUTOJUEGO = true;
                        //desactiva cosas del tuto
                        screenPrompt.gameObject.SetActive(false);
                        mensaje.gameObject.SetActive(false);
                        passButton.gameObject.SetActive(false);
                        //activa cosas del juego
                        construirButton.gameObject.SetActive(true);
                        bloquearButton.gameObject.SetActive(true);
                        instruccion.gameObject.SetActive(true);
                    }
                }
                else
                {
                    PickRandomMarker();
                    gotReward = false;
                }
            }
        }
    }

    public List<Instruccion> instrucciones;
    // Start is called before the first frame update

    public void OnImageRecognized(ImageTarget target)
    {
        //Debug.Log("Reconocio con el bool en: "+GLOBALTUTOJUEGO.ToString());
        if (GLOBALTUTOJUEGO) //si esta en el juego
        {
            //Debug.Log("Reconocio en el juego");
            Invoke("PosMarcadores", 0.1f);
        }
        else //si esta en el tuto
        {
            //Debug.Log("Reconocio en el tuto");
            Invoke("CheckMarkers", 0.1f);
        }
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

    public void PasarImagen()
    {
        Debug.Log("Boton apretado con " + countCorrect);
        if (!cambio)
        {
            PickRandomMarker();
            gotReward = false;
            countCorrect++;
            if (countCorrect == 5)
            {
                passButton.GetComponentInChildren<Text>().text = "Siguiente etapa!";
                cambio = true;
            }
        }
        else
        {
            //aqui se cambia el bool para ver donde va si al juego o al tuto
            //SceneManager.LoadScene("building");
            GLOBALTUTOJUEGO = true;
            //desactiva cosas del tuto
            screenPrompt.gameObject.SetActive(false);
            mensaje.gameObject.SetActive(false);
            passButton.gameObject.SetActive(false);
            //activa cosas del juego
            construirButton.gameObject.SetActive(true);
            bloquearButton.gameObject.SetActive(true);
            instruccion.gameObject.SetActive(true);
        }
    }

    public void BotonDerecho()
    {
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
    public void guardarLasersEnemigos()
    {
        for (int i = cantidadEnemigosGame; i < cantidadEnemigosGame + enemigosReconocidosGame; i++)
        {
            enemigosGame[i].transform.parent = safeZoneAuxGame.transform;
            enemigosGame[i].transform.position = new Vector3(enemigosGame[i].transform.position.x, 0.09f, enemigosGame[i].transform.position.z);
            enemigosGame[i].transform.rotation = new Quaternion(0f,0f,0f,0f);
        }
        cantidadEnemigosGame += enemigosReconocidosGame;
        enemigosReconocidosGame = 0;

        for (int i = cantidadLasersGame; i < cantidadLasersGame + lasersReconocidosGame; i++)
        {
            lasersGame[i].transform.parent = safeZoneAuxGame.transform;
        }
        cantidadLasersGame += lasersReconocidosGame;
        lasersReconocidosGame = 0;

        for (int i = cantidadDianasGame; i < cantidadDianasGame + dianasReconocidasGame; i++)
        {
            dianasGame[i].transform.parent = safeZoneAuxGame.transform;
            dianasGame[i].transform.position = new Vector3(dianasGame[i].transform.position.x, 0.07f, dianasGame[i].transform.position.z);
            dianasGame[i].transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
        cantidadDianasGame += dianasReconocidasGame;
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
                DontDestroyOnLoad(enemigosGame[j]);
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
            if (lasersGame[j] != null)
            {
                dianasGame[j].transform.parent = null;
                dianasGame[j].transform.position -= safeZoneAuxGame.transform.position;
                // lasers[j].transform.localScale *= 20;
                dianasGame[j].transform.position *= 20;
                DontDestroyOnLoad(dianasGame[j]);
            }
        }
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("Juego");
    }
    public void guardarTorres()
    {
        for (int i = cantidadTorresGame; i < cantidadTorresGame + torresReconocidasGame; i++)
        {
            torresGame[i].transform.parent = safeZoneAuxGame.transform;
        }
        cantidadTorresGame += torresReconocidasGame;
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
    void CheckMarkers()
    {
        Transform[] allTransforms = trackable.GetComponentsInChildren<Transform>();
        List<Transform> markers = new List<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.parent.gameObject == trackable)
                markers.Add(t);
        }
        
        for (int i = 0; i < markers.Count; i++)
        {
            if (countCorrect == 0)
            {
                if (markers[i].gameObject.name.StartsWith("tower_") && gotReward== false)
                {
                    countCorrect++;
                    GameObject tickobj = Instantiate(torre, markers[i].position, markers[i].rotation);
                    tickobj.transform.parent = markers[i];
                    gotReward = true;
                    mensaje.text = msj[1];
                }
            }
            if (countCorrect == 1)
            {
                if (markers[i].gameObject.name.StartsWith("safZone_") && gotReward == false)
                {
                    countCorrect++;
                    GameObject tickobj = Instantiate(puntoPartida, markers[i].position, markers[i].rotation);
                    tickobj.transform.localScale = tickobj.transform.localScale / 10;
                    tickobj.transform.parent = markers[i];
                    gotReward = true;
                    mensaje.text = msj[1];
                }
            }
            if (countCorrect == 2)
            {
                if (markers[i].gameObject.name.StartsWith("laserMod_") && gotReward == false)
                {
                    countCorrect++;
                    GameObject tickobj = Instantiate(laser, markers[i].position, markers[i].rotation);
                    tickobj.transform.parent = markers[i];
                    gotReward = true;
                    mensaje.text = msj[1];
                }
            }
            if (countCorrect == 3)
            {
                if (markers[i].gameObject.name.StartsWith("enemy_") && gotReward == false)
                {
                    countCorrect++;
                    GameObject tickobj = Instantiate(enemigo, markers[i].position, markers[i].rotation);
                    tickobj.transform.parent = markers[i];
                    gotReward = true;
                    mensaje.text = msj[1];
                }
            }
            if (countCorrect == 4)
            {
                if (markers[i].gameObject.name.StartsWith("target_") && gotReward == false)
                {
                    countCorrect++;
                    GameObject tickobj = Instantiate(diana, markers[i].position, markers[i].rotation);
                    tickobj.transform.parent = markers[i];
                    gotReward = true;
                    mensaje.text = msj[1];
                }
            }
            if (countCorrect == 5)
            {
                cambio = true;
            }
        }

    }
    void getInput()
    {
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
