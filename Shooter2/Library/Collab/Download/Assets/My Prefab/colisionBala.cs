using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class colisionBala : MonoBehaviour
{
    public GameObject fragmento;
    public int cantidadBalas = 1;
    public int puntos = 0;
    int balas = 0;

    public TextMeshProUGUI points;
    int puntosActuales;

    Scene currentScene;
    string sceneName;

    bool flag =true;

    PhotonView PV;

    // private TutoController wallController;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if(sceneName == "Juego"){
        points = GameObject.FindGameObjectWithTag("Puntos").GetComponent<TextMeshProUGUI>();
        // wallController = GameObject.FindGameObjectWithTag("WallController").GetComponent<TutoController>();
        if(this.gameObject.name == "enemigo(Clone)")
            cantidadBalas = (int)VariablesConfig.vida;
        }
        // Debug.Log("NOMBRE DE MI GAMEOBJECT");
        // Debug.Log(this.gameObject.name);
        // Debug.Log("BALAS");
        // Debug.Log(balas);
    }

    // Update is called once per frame
    void Update()
    {

         if(flag){
            currentScene = SceneManager.GetActiveScene ();
            sceneName = currentScene.name;
        }
        
        if(sceneName == "Juego" && flag){
            Start();
            flag = false;
        }
    }

    private void OnCollisionEnter(Collision col) {
        
        if(col.gameObject.tag == "Bala"){
            balas ++;
            if(balas == cantidadBalas){
                Debug.Log("MUERE");
                if(VariablesConfig.tipoDeJuego == 1){
                    PhotonView pvE = this.gameObject.GetComponent<PhotonView>();
                    PV.RPC("restarEnemigo", RpcTarget.All, VariablesConfig.idJugador,pvE.ViewID );
                }
                else{
                    var clone = Instantiate(fragmento, transform.position, Quaternion.identity);
                    Destroy(col.gameObject);
                    Destroy(gameObject);
                    Destroy(clone,3f);
                    puntosActuales = int.Parse(points.text);
                    puntosActuales += puntos;
                    points.text = puntosActuales.ToString();
                    if(puntos == 1)
                        VariablesConfig.cantidadDianas --;
                    else
                        VariablesConfig.cantidadEnemigos --;          
                }     
            }
        }
    }

    [PunRPC]
    void restarEnemigo(int id, int idEnemy){
            GameObject enemigo = PhotonView.Find(idEnemy).gameObject;
            Destroy(enemigo);
            VariablesConfig.cantidadEnemigos--;
    }

}
