using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class pruebasGod : MonoBehaviour
{

    public GameObject instanciarxd;
    // Start is called before the first frame update
    void Start()
    {
        GameObject Player = Instantiate(instanciarxd, new Vector3(0 , 15, 0), Quaternion.Euler(90,120,0));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
