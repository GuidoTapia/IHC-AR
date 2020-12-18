using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Enemigo : MonoBehaviour
{
    public float visionRadius;
    public float speed;
    GameObject player;
    GameObject brazos;

    vidaJugador playerVida;
    public int cantidad;
    Color temp;

    Scene currentScene;
    string sceneName;

    Vector3 initialPosition;

    bool flag =true;

    static float sensibilidad;

    PhotonView PV;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        currentScene = SceneManager.GetActiveScene ();
        sceneName = currentScene.name;

        if(sceneName == "Juego"){
            player = GameObject.FindGameObjectWithTag("Player");
            brazos = GameObject.FindGameObjectWithTag("PCamera");
            initialPosition = transform.position;

            playerVida = player.GetComponent<vidaJugador>();
            temp = playerVida.pantallaRoja.color;
            
            visionRadius = (int)VariablesConfig.alcance;
            speed = (int)VariablesConfig.velocidad;
            cantidad = (int)VariablesConfig.danio * -1;


        }

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
            Debug.Log("JUEGOOOOOOOOOOOOOOOOO");
        }
        

        Vector3 target = initialPosition;

        if(sceneName == "Juego"){

            float dist = Vector3.Distance(brazos.transform.position, transform.position);
            if(dist<visionRadius) {
                target.x = brazos.transform.position.x;
                target.z = brazos.transform.position.z;
            }
        

            float fixedSpeed = speed *Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, fixedSpeed);
            
            Debug.DrawLine(transform.position,target,Color.green);
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
    }
     

    void desactivarAnim(){
        VariablesConfig.ayudaAnimation.SetBool("rojo",false);
    }  
    
    void OnCollisionEnter(Collision other) {
        if(other.gameObject  == player ){
             VariablesConfig.ayudaAnimation.SetBool("rojo",true);
             Invoke("desactivarAnim", 0.1f);
             PV.RPC("restarVida", RpcTarget.All);
        }
    }


    [PunRPC]
    void restarVida(){
        VariablesConfig.vidaJugador += cantidad;
    }



}
