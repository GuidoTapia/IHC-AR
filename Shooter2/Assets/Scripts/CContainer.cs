using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CContainer : MonoBehaviour
{   
    private GameObject pj;
    // Start is called before the first frame update
    void Start()
    {
        pj = GameObject.FindGameObjectWithTag("Player");
        transform.SetParent(pj.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
