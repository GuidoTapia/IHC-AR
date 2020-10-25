using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void empezarJuego(){
        SceneManager.LoadScene("TutorialReal");
    }

    public void tutorial(){
        SceneManager.LoadScene("TutorialReal");
    }

    public void cerrarJuego(){
        Application.Quit();
        Debug.Log("SALIENDO DEL JUEGO");
    }
}
