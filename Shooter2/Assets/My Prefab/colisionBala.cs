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

    Color temp;
    vidaJugador playerVida;
    GameObject player;

    bool menosEnemigo = false;
    bool menosDiana = false;
    // private TutoController wallController;

    // Start is called before the first frame update
    void Start()
    {   


        PV = GetComponent<PhotonView>();
        if(sceneName == "Juego"){
            
            player = GameObject.FindGameObjectWithTag("Player");
            playerVida = player.GetComponent<vidaJugador>();
            temp = playerVida.pantallaRoja.color;
            

            if(this.gameObject.name == "enemigo(Clone)")
                cantidadBalas = (int)VariablesConfig.vida;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(VariablesConfig.idJugador == 1)
            points = GameObject.FindGameObjectWithTag("Puntos").GetComponent<TextMeshProUGUI>();

         if(flag){
            currentScene = SceneManager.GetActiveScene ();
            sceneName = currentScene.name;
        }
        
        if(sceneName == "Juego" && flag){
            Start();
            flag = false;
        }

        if(menosEnemigo){
            VariablesConfig.cantidadEnemigos -=1;
            menosEnemigo = false;
        }
        if(menosDiana){
            VariablesConfig.cantidadDianas -=1;
            menosDiana = false;
        }

    }

    private void OnCollisionEnter(Collision col) {
        
        if(col.gameObject.tag == "Bala"){
            balas ++;
            if(balas == cantidadBalas){
                Debug.Log("MUERE");
                if(VariablesConfig.tipoDeJuego == 1){
                    
                    PhotonView pvE = gameObject.GetComponent<PhotonView>();
                    if(puntos == 1){
                        
                        PV.RPC("restarEnemigo", RpcTarget.All,pvE.ViewID,1);
                    }
                    else{
                        PV.RPC("restarEnemigo", RpcTarget.All,pvE.ViewID,2);
                    }
                    // Destroy(gameObject, 0.1f);
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
    public void restarEnemigo(int idEnemy,int tag){
            
        GameObject enemigo = PhotonView.Find(idEnemy).gameObject;
        if(tag==2){
            
            // menosEnemigo = true;
            VariablesConfig.cantidadEnemigos -=0.1;
            
        }
        else{
            // menosDiana = true;
            VariablesConfig.cantidadDianas -=0.1;
        }
        Destroy(enemigo,0.1f);
}

}
