using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class victoriaMeta : MonoBehaviour
{

    public Material[] material;
    Renderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player"){
            rend.sharedMaterial = material[1];
        }
    }
}
