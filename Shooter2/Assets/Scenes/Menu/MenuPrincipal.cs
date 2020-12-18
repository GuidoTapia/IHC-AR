using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
public class MenuPrincipal : MonoBehaviour
{
    void Start()
    {
        VariablesConfig.etapaGame = 0; // 0: Menu Lobby
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void empezarJuego(){
        VariablesConfig.tipoDeJuego = 0;
        VariablesConfig.idJugador = 1;
        SceneManager.LoadScene("TutorialReal");
    }

    public void multiJugador(){
        VariablesConfig.tipoDeJuego = 1;
        SceneManager.LoadScene("lobby");
    }

    public void configuracion(){
        SceneManager.LoadScene("Configuracion");
    }

    public void guia(){
        SceneManager.LoadScene("GuiaMarcadores");
    }


    public void cerrarJuego(){
        Application.Quit();
        Debug.Log("SALIENDO DEL JUEGO");
    }
}
