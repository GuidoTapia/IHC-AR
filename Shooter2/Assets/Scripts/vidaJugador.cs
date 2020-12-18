using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vidaJugador : MonoBehaviour
{
    // public float vida = 100;
    public Image barraDeVida;
    public Image pantallaRoja;
    public GameObject gameOverText;
    public GameObject pauseText;
    Animator animacion;
    Animator pausaAnimacion;
    FPS_Camera movimiento;

    bool pause = false;

    // Start is called before the first frame update
    void Start()
    {
        animacion = gameOverText.GetComponent<Animator>();
        pausaAnimacion = pauseText.GetComponent<Animator>();
        movimiento = GetComponent<FPS_Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        VariablesConfig.vidaJugador = Mathf.Clamp(VariablesConfig.vidaJugador,0,100);
        barraDeVida.fillAmount = VariablesConfig.vidaJugador / 100;
        Debug.Log("VIDAJUGAOR" + VariablesConfig.vidaJugador / 100 );

        if(VariablesConfig.vidaJugador <= 0.0f){
            animacion.SetBool("activo", true);
            movimiento.gameOver = true;
        }
    }

    public void Pause(){
        pause = pause ? false : true;
        if(pause){
            Debug.Log("pUAS");
            pausaAnimacion.SetBool("pausa", true);
            Invoke("p", 0.5f);
        }
        else
        {
            Resume();
        }
    }

    public void Resume(){
        Time.timeScale = 1f;
        pausaAnimacion.SetBool("pausa", false);
        
    }

    void p(){
        Time.timeScale = 0f;
    }
}
