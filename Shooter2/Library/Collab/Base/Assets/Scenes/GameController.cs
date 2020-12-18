using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameController : MonoBehaviour
{   

    public GameObject shooter;
    public GameObject dios;

    // Start is called before the first frame update
    void Start()
    {
        if(VariablesConfig.idJugador == 1){
            // shooter.SetActive(false);
            GameObject Player = PhotonNetwork.Instantiate("PHPlayer", new Vector3(0 , 0.5f, 0), new Quaternion(90 , 90, 0,1), 0);
        }
        else{
            GameObject Dios = PhotonNetwork.Instantiate("PHGod", new Vector3(0 , 200, 0), new Quaternion(90 , 120, 0,1), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log("Un soldado ha dejado el campo de batalla");
    }
}
