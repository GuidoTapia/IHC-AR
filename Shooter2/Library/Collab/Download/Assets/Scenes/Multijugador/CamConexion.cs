using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class CamConexion : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
        PhotonNetwork.ConnectUsingSettings();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Cuarto",new RoomOptions { MaxPlayers = 5 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.Instantiate("WikitudeCamera",new Vector3(0,0.5f,35),Quaternion.identity);
        //PhotonNetwork.Instantiate("MarcadoresFinalesTracker", new Vector3(0, 0, 0), Quaternion.identity);
    }
}
