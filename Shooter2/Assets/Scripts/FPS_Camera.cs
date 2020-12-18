using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class FPS_Camera : MonoBehaviour
{


    public Camera FPSCamera;
    public bool gameOver = false;
    public joystick js;

    public Text EnemigosRestantes;
    public Text DianasRestantes;

    bool pause = false;

    public GameObject winText;
    Animator animacion;
  

    Scene currentScene;
    string sceneName;
    static float sensibilidad;

    float h;
    float v;
    

    void Start(){
        
        animacion = winText.GetComponent<Animator>();
        sensibilidad = VariablesConfig.sensibilidad;


        if(VariablesConfig.tipoDeJuego == 0){
            GameObject canvas = GameObject.FindGameObjectWithTag("PCanvas").gameObject;
            Button botonRecargar = canvas.transform.Find("RecargaButton").GetComponent<Button>();
            botonRecargar.gameObject.SetActive(true);
        }

        else if(VariablesConfig.tipoDeJuego == 1){
            PhotonView myPV = GetComponent<PhotonView>();

            Debug.Log("MI ID ES :"  + VariablesConfig.idJugador);
            if(!myPV.IsMine){
                FPSCamera.enabled = false;
                gameObject.transform.Find("Canvas").gameObject.SetActive(false);
                // this.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {   

        if(VariablesConfig.idJugador == 1)
            VariablesConfig.ayudaAnimation = GameObject.FindGameObjectWithTag("ayudaP").gameObject.GetComponent<Animator>();

        if(!gameOver){
            transform.Translate(FPSCamera.transform.right.x * -js.Vertical()/sensibilidad,FPSCamera.transform.right.z * -js.Vertical()/sensibilidad,0f);
            transform.Translate(FPSCamera.transform.forward.x * js.Horizontal()/sensibilidad,FPSCamera.transform.forward.z * js.Horizontal()/sensibilidad,0f);
        }
        else{
            pause = false;
            Invoke("Pause", 3);
        }

        VariablesConfig.cantidadEnemigos = (int)VariablesConfig.cantidadEnemigos;
        EnemigosRestantes.text = (VariablesConfig.cantidadEnemigos).ToString();
        VariablesConfig.cantidadDianas = (int)VariablesConfig.cantidadDianas;
        DianasRestantes.text = (VariablesConfig.cantidadDianas).ToString();

        if( VariablesConfig.cantidadEnemigos == 0 &&  VariablesConfig.cantidadDianas == 0){
            animacion.SetBool("activo", true);
        }

        Debug.Log("C ENEMIGOS : " + VariablesConfig.cantidadEnemigos);
        Debug.Log("C DIANAS : " + VariablesConfig.cantidadDianas);
    }



    public void Pause(){
        pause = pause ? false : true;
        if(pause){
            Time.timeScale = 0f;
        }
        else{
            Time.timeScale = 1f;
        }
    }

    public void Reset(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Juego");
        Debug.Log("RESET");
    }

     public void Menu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    

    
}
