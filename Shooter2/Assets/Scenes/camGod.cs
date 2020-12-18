using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class camGod : MonoBehaviour
{

    public Camera FPSCamera;
    public bool gameOver = false;
    Animator animacion;
    Animator GOAnimacion;
    static float sensibilidad;
    public joystick js;
    public GameObject winText;
    public GameObject gameOverText;

    float v;
    float h;

    bool pause = false;
    // Start is called before the first frame update
    void Start()
    {

        PhotonView myPV = GetComponent<PhotonView>();
        animacion = winText.GetComponent<Animator>();
        GOAnimacion = gameOverText.GetComponent<Animator>();
        
        if(!myPV.IsMine){
            FPSCamera.enabled = false;
            gameObject.transform.Find("Canvas").gameObject.SetActive(false);
            // this.enabled = false;
        }
        
        sensibilidad = VariablesConfig.sensibilidad;
    }

    // Update is called once per frame
    void Update()
    {
        if(VariablesConfig.idJugador > 1)
            VariablesConfig.ayudaAnimation = GameObject.FindGameObjectWithTag("ayudaG").gameObject.GetComponent<Animator>();
            
        Start();
        if (!gameOver)
        {
                transform.Translate(FPSCamera.transform.right.x * -js.Vertical()/sensibilidad,FPSCamera.transform.right.z * -js.Vertical()/sensibilidad,0f);
                transform.Translate(FPSCamera.transform.forward.x * js.Horizontal()/sensibilidad,FPSCamera.transform.forward.z * js.Horizontal()/sensibilidad,0f);
        }
        else
        {
            pause = false;
            Invoke("Pause", 3);
        }

        if (VariablesConfig.cantidadEnemigos == 0 && VariablesConfig.cantidadDianas == 0)
        {
            animacion.SetBool("activo", true);
        }

        if(VariablesConfig.vidaJugador <= 0.0f){
            GOAnimacion.SetBool("activo", true);
        }
    }
}
