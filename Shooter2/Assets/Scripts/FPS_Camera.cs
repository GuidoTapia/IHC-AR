using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FPS_Camera : MonoBehaviour
{
    public Camera FPSCamera;
    public bool gameOver = false;
    public joystick js;

    public Text EnemigosRestantes;
    public Text DianasRestantes;

    bool pause = false;
    private TutoController wallController;

    public GameObject winText;
    Animator animacion;
    

    void Start(){
        wallController = GameObject.FindGameObjectWithTag("WallController").GetComponent<TutoController>();
        animacion = winText.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {        
        Start();
        if(!gameOver){
            transform.Translate(FPSCamera.transform.right.x * -js.Vertical()/10,FPSCamera.transform.right.z * -js.Vertical()/10,0f);
            transform.Translate(FPSCamera.transform.forward.x * js.Horizontal()/10,FPSCamera.transform.forward.z * js.Horizontal()/10,0f);
        }
        else{
            pause = false;
            Invoke("Pause", 3);
        }

        EnemigosRestantes.text = wallController.cantidadEnemigosGame.ToString();
        DianasRestantes.text = wallController.cantidadDianasGame.ToString();

        if( wallController.cantidadEnemigosGame == 0 &&  wallController.cantidadDianasGame == 0){
            animacion.SetBool("activo", true);
        }
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
