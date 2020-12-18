using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wikitude;
using UnityEngine.SceneManagement;
//using System.Numerics;

public class WallController : MonoBehaviour
{

    public GameObject trackable;
    public GameObject pared;
    public GameObject torreInicio;
    public GameObject torreFin;
    public GameObject safeZone;
    public GameObject laser;
    public GameObject enemigo;
    public GameObject diana;
    private GameObject safeZoneAux;
    private GameObject[] torres;
    private GameObject[] paredes;
    private GameObject[] lasers;
    private GameObject[] enemigos;
    private GameObject[] dianas;
    private bool SaveZoneinImage;
    private int cantidadTorres;
    private int torresReconocidas;
    private int cantidadLasers;
    public int cantidadDianas;
    private int dianasReconocidas;
    private int lasersReconocidos;
    public int cantidadEnemigos;
    private int enemigosReconocidos;
    private int paredesConstruidas;
    private int flag;
    // Start is called before the first frame update
    void Start()
    {
        flag = 0;
        torresReconocidas = 0;
        paredesConstruidas = 0;
        cantidadTorres = 0;
        lasersReconocidos = 0;
        cantidadEnemigos = 0;
        enemigosReconocidos = 0;
        paredesConstruidas = 0;
        cantidadDianas=0;
        dianasReconocidas=0;
        paredes = new GameObject[64];
        torres = new GameObject[64];
        lasers = new GameObject[64];
        enemigos = new GameObject[64];
        dianas = new GameObject[64];
        /*for (int i = 0; i < 64; i++)
        {
            paredes[i] = Instantiate(pared);
        }*/
    }
    public void OnImageRecognized(ImageTarget target)
    {
        Invoke("PosMarcadores", 0.1f);
    }
    void Update()
    {
        if (flag == 1)
        {
            posicionarLasers();
        }
        getInput();
    }
    void posicionarLasers()
    {
        for (int i = cantidadLasers; i < cantidadLasers + lasersReconocidos; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(lasers[i].transform.parent.position.x,0.2f, lasers[i].transform.parent.position.z), new Vector3(-lasers[i].transform.parent.forward.x,0f, -lasers[i].transform.parent.forward.z), out hit))
            {
                if (hit.collider && hit.transform.gameObject.tag == "Wall")
                {
                    lasers[i].transform.position = hit.point;
                    Transform[] laserTransform =lasers[i].GetComponentsInChildren<Transform>();
                    foreach (Transform t in laserTransform)
                    {
                        if (Physics.Raycast(lasers[i].transform.position, lasers[i].transform.forward, out hit))
                        {
                            t.position= (hit.point+ lasers[i].transform.position)/2;
                            t.LookAt(hit.point);
                            t.localScale = new Vector3(t.localScale.x, t.localScale.y,Vector3.Distance(lasers[i].transform.position, hit.point));
                        }
                    }
                }
                else if (!hit.collider)
                {
                    lasers[i].transform.position = lasers[i].transform.parent.position;
                }
            }
            // else lr.SetPosition(1, transform.forward*5000);
        }
    }
    void getInput()
    {
    }
    public void OnImageLost(ImageTarget target){

        if (target.Name.StartsWith("safZon"))
        {
            safeZoneAux.transform.parent = null;
            safeZoneAux.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            for (int i = 0; i < cantidadTorres; i++)
            {
                torres[i].transform.parent = null;
                torres[i].transform.position -= safeZoneAux.transform.position;
            }
            for (int i = 0; i < paredesConstruidas; i++)
            {
                paredes[i].transform.parent = null;
                paredes[i].transform.position -= safeZoneAux.transform.position;
            }
            for (int i = 0; i < cantidadEnemigos; i++)
            {
                enemigos[i].transform.parent = null;
                enemigos[i].transform.position -= safeZoneAux.transform.position;
            }
            for (int i = 0; i < cantidadLasers; i++)
            {
                lasers[i].transform.parent = null;
                lasers[i].transform.position -= safeZoneAux.transform.position;
            }
            for (int i = 0; i < cantidadDianas; i++)
            {
                dianas[i].transform.parent = null;
                dianas[i].transform.position -= safeZoneAux.transform.position;
            }
            Destroy(safeZoneAux);
            SaveZoneinImage = false;
        }
        else
        {
            Invoke("PosMarcadores", 0.1f);
        }
    }
    void PosMarcadores()
    {
        Transform[] allTransforms = trackable.GetComponentsInChildren<Transform>();
        List<Transform> markers = new List<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.parent.gameObject == trackable)
                markers.Add(t);
        }
        for (int i = cantidadTorres; i < cantidadTorres + torresReconocidas; i++)
        {
            Destroy(torres[i]);
        }
        torresReconocidas = 0;
        enemigosReconocidos = 0;
        lasersReconocidos = 0;

        for (int i = 0; i < markers.Count; i++)
        {
            if (markers[i].gameObject.name.StartsWith("safZone"))
            {
                if (SaveZoneinImage)
                {
                    //solo puede haber una zona segura
                }
                else
                {
                    SaveZoneinImage = true;
                    safeZoneAux = Instantiate(safeZone, markers[i].position, markers[i].rotation);
                    // safeZoneAux.transform.localScale = new Vector3(10f,10f,10f);
                    safeZoneAux.transform.parent = markers[i];
                    for (int j = 0; j < cantidadTorres; j++)
                    {
                        torres[j].transform.parent = safeZoneAux.transform;
                    }
                    for (int j = 0; j < paredesConstruidas; j++)
                    {
                        paredes[j].transform.parent = safeZoneAux.transform;
                    }
                    for (int j = 0; j < cantidadEnemigos; j++)
                    {
                        enemigos[j].transform.parent = safeZoneAux.transform;
                    }
                    for (int j = 0; j < cantidadLasers; j++)
                    {
                        lasers[j].transform.parent = safeZoneAux.transform;
                    }
                }
            }
            else if (markers[i].gameObject.name.StartsWith("tower") && SaveZoneinImage && flag==0)
            {
                torres[cantidadTorres + torresReconocidas] = Instantiate(torreInicio, markers[i].position, markers[i].rotation);
                torres[cantidadTorres + torresReconocidas].transform.parent = markers[i];
                torresReconocidas++;
            }
            else if (markers[i].gameObject.name.StartsWith("laserMod") && SaveZoneinImage && flag == 1)
            {
                lasers[cantidadLasers + lasersReconocidos] = Instantiate(laser, markers[i].position, markers[i].rotation);
                lasers[cantidadLasers + lasersReconocidos].transform.localScale = lasers[cantidadLasers + lasersReconocidos].transform.localScale / 4;
                lasers[cantidadLasers + lasersReconocidos].transform.parent = markers[i];
                lasersReconocidos++;
            }
            else if (markers[i].gameObject.name.StartsWith("enemy") && SaveZoneinImage && flag == 1)
            {
                enemigos[cantidadEnemigos + enemigosReconocidos] = Instantiate(enemigo, markers[i].position, markers[i].rotation);
                enemigos[cantidadEnemigos + enemigosReconocidos].transform.parent = markers[i];
                enemigosReconocidos++;
            }
            else if (markers[i].gameObject.name.StartsWith("target") && SaveZoneinImage && flag == 1)
            {
                dianas[cantidadDianas + dianasReconocidas] = Instantiate(diana, markers[i].position, markers[i].rotation);
                dianas[cantidadDianas + dianasReconocidas].transform.parent = markers[i];
                dianasReconocidas++;
            }
        }
    }
    public void BotonIzquierdo()
    {
        if (flag == 0)
        {
            guardarTorres();
        }
        else
        {
            guardarLasersEnemigos();
        }
    }
    public void BotonDerecho()
    {
        if (flag == 0)
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
        for (int i = cantidadEnemigos; i < cantidadEnemigos + enemigosReconocidos; i++)
        {
            enemigos[i].transform.parent = safeZoneAux.transform;
            enemigos[i].transform.position = new Vector3(enemigos[i].transform.position.x, 0.0f, enemigos[i].transform.position.z);
        }
        cantidadEnemigos += enemigosReconocidos;
        enemigosReconocidos = 0;

        for (int i = cantidadLasers; i < cantidadLasers + lasersReconocidos; i++)
        {
            lasers[i].transform.parent = safeZoneAux.transform;
            lasers[i].transform.position = new Vector3(lasers[i].transform.position.x, 0.0f, lasers[i].transform.position.z);
        }
        cantidadLasers += lasersReconocidos;
        lasersReconocidos = 0;

        for (int i = cantidadDianas; i < cantidadDianas + dianasReconocidas; i++)
        {
            dianas[i].transform.parent = safeZoneAux.transform;
            dianas[i].transform.position = new Vector3(dianas[i].transform.position.x, 0.0f, dianas[i].transform.position.z);
        }
        cantidadDianas += dianasReconocidas;
        dianasReconocidas = 0;
        Invoke("PosMarcadores", 0.1f);
    }
    public void escalaExportar()
    {
        safeZoneAux.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        for (int j = 0; j < cantidadTorres; j++)
        {
            torres[j].transform.parent = null;
            torres[j].transform.position -= safeZoneAux.transform.position;
            torres[j].transform.localScale *= 20;
            torres[j].transform.position *= 20;
            DontDestroyOnLoad(torres[j]);
        }
        for (int j = 0; j < paredesConstruidas; j++)
        {
            paredes[j].transform.parent = null;
            paredes[j].transform.position -= safeZoneAux.transform.position;
            paredes[j].transform.localScale *= 20;
            paredes[j].transform.position *= 20;
            DontDestroyOnLoad(paredes[j]);
        }
        for (int j = 0; j < cantidadEnemigos; j++)
        {
            enemigos[j].transform.parent = null;
            enemigos[j].transform.position -= safeZoneAux.transform.position;
            // enemigos[j].transform.localScale *= 20;
            enemigos[j].transform.position *= 20;
            DontDestroyOnLoad(enemigos[j]);
        }
        for (int j = 0; j < cantidadLasers; j++)
        {
            lasers[j].transform.parent = null;
            lasers[j].transform.position -= safeZoneAux.transform.position;
            lasers[j].transform.localScale *= 20;
            lasers[j].transform.position *= 20;
            DontDestroyOnLoad(lasers[j]);
        }
        for (int j = 0; j < cantidadDianas; j++)
        {
            dianas[j].transform.parent = null;
            dianas[j].transform.position -= safeZoneAux.transform.position;
            // lasers[j].transform.localScale *= 20;
            dianas[j].transform.position *= 20;
            DontDestroyOnLoad(dianas[j]);
        }

        DontDestroyOnLoad(this);
        SceneManager.LoadScene("Juego");
    }
    public void guardarTorres()
    {
        for (int i = cantidadTorres; i < cantidadTorres + torresReconocidas; i++)
        {
            torres[i].transform.parent = safeZoneAux.transform;
        }
        cantidadTorres += torresReconocidas;
        torresReconocidas = 0;
        Invoke("PosMarcadores", 0.1f);
    }
    public void giftWraper()
    {
        flag = 1;
        guardarTorres();
        int iMinimo = 0;
        double dMinimo = 100000.0d;
        for(int i=0;i< cantidadTorres; i++)
        {
            torres[i].transform.rotation = new Quaternion(0,0,0,0);
            torres[i].transform.position = new Vector3(torres[i].transform.position.x, 0.0f, torres[i].transform.position.z);
            if (torres[i].transform.position.magnitude < dMinimo)
            {
                iMinimo = i;
                dMinimo = torres[i].transform.position.magnitude;
            }
        }
        Vector3 positionSwap= torres[0].transform.position;
        Quaternion rotationSwap= torres[0].transform.rotation;
        torres[0].transform.position=torres[iMinimo].transform.position;
        torres[0].transform.rotation=torres[iMinimo].transform.rotation;
        torres[iMinimo].transform.position = positionSwap;
        torres[iMinimo].transform.rotation = rotationSwap;

        Vector3 positionAux = torres[0].transform.position / 2;
        for (int i = 1; i < cantidadTorres; i++)
        {
            iMinimo = i;
            dMinimo = Vector3.Distance(positionAux, torres[i].transform.position);
            for (int j = i+1; j < cantidadTorres; j++)
            {
                if (Vector3.Distance(positionAux, torres[j].transform.position) < dMinimo)
                {
                    iMinimo = j;
                    dMinimo = Vector3.Distance(positionAux, torres[j].transform.position);
                }
            }
            positionSwap = torres[i].transform.position;
            rotationSwap = torres[i].transform.rotation;
            torres[i].transform.position = torres[iMinimo].transform.position;
            torres[i].transform.rotation = torres[iMinimo].transform.rotation;
            torres[iMinimo].transform.position = positionSwap;
            torres[iMinimo].transform.rotation = rotationSwap;

            paredes[i-1] = Instantiate(pared);
            paredes[i-1].transform.position = (torres[i].transform.position + torres[i-1].transform.position) / 2;
            paredes[i-1].transform.LookAt(torres[i].transform.position);
            paredes[i-1].transform.localScale = new Vector3(0.1f, 0.5f, Vector3.Distance(torres[i].transform.position, torres[i-1].transform.position));
            paredes[i-1].transform.parent = safeZoneAux.transform;
            positionAux = (torres[i].transform.position + torres[i - 1].transform.position) / 2;
            paredesConstruidas++;
        }

    }

    void adjustWall(List<Transform> markers, GameObject muro,Vector3 p,Vector3 ls,Quaternion r)
    {
        torreInicio.transform.LookAt(markers[0].transform.position);
        torreFin.transform.LookAt(markers[1].transform.position);
        float distance = Vector3.Distance(torreFin.transform.position, torreInicio.transform.position);
        p = torreInicio.transform.position + distance / 2 * torreInicio.transform.forward + 0.5f * Vector3.up;
        r = torreInicio.transform.rotation;
        ls = new Vector3(muro.transform.localScale.x, muro.transform.localScale.y, distance);
    }
    void createWall(Vector3 posInicio, Vector3 posFin, int index)
    {
        float distance = Vector3.Distance(posInicio, posFin);
        paredes[index].transform.position = (posInicio + posFin)/2;
        paredes[index].transform.LookAt(posFin);
        paredes[index].transform.localScale = new Vector3(0.1f, 0.5f, distance);
    }
}
