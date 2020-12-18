using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VariablesConfig : MonoBehaviour
{
    public Slider slider_sensibilidad;
    public Slider slider_velocidad;
    public Slider slider_alcance;
    public Slider slider_vida;
    public Slider slider_danio;

    public Text text_sensibilidad;
    public Text text_velocidad;
    public Text text_alcance;
    public Text text_vida;
    public Text text_danio;

    static public float sensibilidad = 10;
    static public float velocidad = 3;
    static public float alcance = 6;
    static public float vida = 5;
    static public float danio = 10;

    static public bool salio = false;


    static public double cantidadEnemigos = 0;
    static public double cantidadDianas = 0;

    static public int idJugador = 0;
    static public int nTotalJugadores = 0;

    static public int tipoDeJuego = 0; // 0: un jugador ---- 1: multiplayer

    static public float vidaJugador = 100;
    static public int nBalasJugador = 12;

    static public int etapaGame = 0; // 0: menu/lobby - 1: tutorial - 2: construccion - 3: juego
    static public int estadoGame =0; // 0: enJuego - 1: Win - 2: GameOver

    static public bool ShooterSeFue = false;

    static public Animator ayudaAnimation;

    // public static GameObject[] dianasGlobal = new GameObject[64];
    // public static GameObject[] enemigosGlobal = new GameObject[64];

    public static List<Vector3> dianasGlobalPos = new List<Vector3>();
    public static List<Vector3> enemigosGlobalPos = new List<Vector3>();
    public static List<int> idJugadoresDisponibles = new List<int>();

    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {   
        
        sensibilidad = 20 - slider_sensibilidad.value;
        velocidad = slider_velocidad.value;
        alcance = slider_alcance.value;
        vida = slider_vida.value;
        danio = slider_danio.value;

        text_sensibilidad.text = slider_sensibilidad.value.ToString();
        text_velocidad.text = velocidad.ToString();
        text_alcance.text = alcance.ToString();
        text_vida.text = vida.ToString();
        text_danio.text = danio.ToString();

    }

    public void back(){
        SceneManager.LoadScene("Menu");
    }


}
