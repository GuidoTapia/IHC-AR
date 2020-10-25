using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Laser : MonoBehaviour {

    private LineRenderer lr;
    vidaJugador playerVida;
    Color temp;

    Scene currentScene;
    string sceneName;

    bool flag2 =true;
    bool flag ;


    void Start () {
        lr= GetComponent<LineRenderer>();
         if(sceneName == "Juego"){
            playerVida = GameObject.FindGameObjectWithTag("Player").GetComponent<vidaJugador>();
            temp = playerVida.pantallaRoja.color;
         }
        flag = false;

    }
    
    // Update is called once per frame
    void Update () {

            if(flag2){
                currentScene = SceneManager.GetActiveScene ();
                sceneName = currentScene.name;
            }
            
            if(sceneName == "Juego" && flag2){
                Start();
                flag2 = false;
            }
            
            lr.SetPosition(0, transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                if (hit. collider)
                {   
                    lr.SetPosition(1, hit.point);
                    if(hit.transform.gameObject.tag == "Player"){
                        flag = true;
                        playerVida.vida -=30;
                        temp.a = 0.5f;
                        playerVida.pantallaRoja.color = temp;
                        //hit.transform.gameObject.transform.position =  new Vector3(0,0,0);
                    }
                    else if (flag){
                        temp.a = 0.0f;
                        playerVida.pantallaRoja.color = temp;
                        flag = false;
                    }
                    
                }
            }
    else lr.SetPosition(1, transform.forward*5000);
    }

}