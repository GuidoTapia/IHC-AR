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
            brazos = GameObject.FindGameObjectWithTag("MainCamera");
            initialPosition = transform.position;

            playerVida = player.GetComponent<vidaJugador>();
            temp = playerVida.pantallaRoja.color;
            
            visionRadius = (int)VariablesConfig.alcance;
            speed = (int)VariablesConfig.velocidad;
            cantidad = (int)VariablesConfig.danio * -1;


        }

        // Debug.Log("VISIONRADIUS");
        // Debug.Log(visionRadius);
        // Debug.Log("SPEED");
        // Debug.Log(speed);
        // Debug.Log("DAÑO");
        // Debug.Log(cantidad);

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
     
    void OnCollisionEnter(Collision other) {
        if(other.gameObject  == player ){
             PV.RPC("restarVida", RpcTarget.All);
        }
    }

    void OnCollisionExit(Collision other) {
         if(other.gameObject  == player){
            temp.a= 0.0f;
            playerVida.pantallaRoja.color = temp;
            Debug.Log(playerVida.pantallaRoja.color.a);
        }
    }

    [PunRPC]
    void restarVida(){
        VariablesConfig.vidaJugador += cantidad;
        temp.a = 0.5f;
        playerVida.pantallaRoja.color = temp;
        Debug.Log(playerVida.pantallaRoja.color.a);
    }



}
