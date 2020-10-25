using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class colisionBala : MonoBehaviour
{
    public GameObject fragmento;
    public int cantidadBalas = 1;
    public int puntos = 0;
    int balas = 0;

    public TextMeshProUGUI points;
    int puntosActuales;

    Scene currentScene;
    string sceneName;

    bool flag =true;
    private TutoController wallController;

    // Start is called before the first frame update
    void Start()
    {
        if(sceneName == "Juego"){
        points = GameObject.FindGameObjectWithTag("Puntos").GetComponent<TextMeshProUGUI>();
        wallController = GameObject.FindGameObjectWithTag("WallController").GetComponent<TutoController>();
        }
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
        }
    }

    private void OnCollisionEnter(Collision col) {
        
        if(col.gameObject.tag == "Bala"){
            balas ++;
            if(balas == cantidadBalas){
                var clone = Instantiate(fragmento, transform.position, Quaternion.identity);
                Destroy(col.gameObject);
                Destroy(gameObject);
                Destroy(clone,3f);
                puntosActuales = int.Parse(points.text);
                puntosActuales += puntos;
                points.text = puntosActuales.ToString();
                if(puntos == 1)
                    wallController.cantidadDianasGame --;
                else
                    wallController.cantidadEnemigosGame --;
               
            }
        }
    }
}
