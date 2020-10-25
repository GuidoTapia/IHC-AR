using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPlayer : MonoBehaviour {

public GameObject diana;
private float inicioDisparar;

 void Start () {
 }
 
 // Update is called once per frame
 void Update () {
        
 }

 public void instanciarBloque(){
     float tiempoDisparo = 0.5f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit. collider)
            {   if(Time.time > inicioDisparar ){
                    inicioDisparar = Time.time + tiempoDisparo;
                    Instantiate(diana, hit.point, Quaternion.identity);     
                }          
            }
        }
 }

}