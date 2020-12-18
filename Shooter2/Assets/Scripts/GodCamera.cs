using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GodCamera : MonoBehaviour
{
    public Camera FPSCamera;
    public bool gameOver = false;
    public joystick js;

    public Text EnemigosRestantes;
    public Text DianasRestantes;

    bool pause = false;
    // private TutoController wallController;

    public GameObject winText;
    Animator animacion;

    Scene currentScene;
    string sceneName;
    static float sensibilidad;


    void Start()
    {
        // wallController = GameObject.FindGameObjectWithTag("WallController").GetComponent<TutoController>();
        animacion = winText.GetComponent<Animator>();
        sensibilidad = VariablesConfig.sensibilidad;

        // Debug.Log("SENSI");
        // Debug.Log(sensibilidad);
    }

    // Update is called once per frame
    void Update()
    {

        Start();
        if (!gameOver)
        {
            transform.Translate(FPSCamera.transform.right.x * -js.Vertical() / sensibilidad, FPSCamera.transform.right.z * -js.Vertical() / sensibilidad, 0f);
            transform.Translate(FPSCamera.transform.forward.x * js.Horizontal() / sensibilidad, FPSCamera.transform.forward.z * js.Horizontal() / sensibilidad, 0f);
        }
        else
        {
            pause = false;
            Invoke("Pause", 3);
        }

        EnemigosRestantes.text = VariablesConfig.cantidadEnemigos.ToString();
        DianasRestantes.text = VariablesConfig.cantidadDianas.ToString();

        if (VariablesConfig.cantidadEnemigos == 0 && VariablesConfig.cantidadDianas == 0)
        {
            animacion.SetBool("activo", true);
        }
    }


    public void Pause()
    {
        pause = pause ? false : true;
        if (pause)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Reset()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Juego");
        Debug.Log("RESET");
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
