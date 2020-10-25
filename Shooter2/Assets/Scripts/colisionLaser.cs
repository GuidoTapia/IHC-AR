using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colisionLaser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(("hola1"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

private void OnCollisionEnter(Collision col) {
    if(col.gameObject.tag == "Player"){
        Debug.Log(("hola"));
    }
}

}

