using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class AutoLobby : MonoBehaviourPunCallbacks
{
    public Button ConnectButton;
    public Button JoinRandonButton;
    public GameObject StartButton;
    public Text Log;
    public Text PlayerCount;
    public Text desconexx;
    public byte maxPlayersPerRoom = 4;
    static public int playersCount;

    public GameObject textoDesconexion;


    void Start()
    {
        VariablesConfig.idJugador = 0;
    }

    void Update()
    {
        if(VariablesConfig.etapaGame == 0){
            if(PhotonNetwork.CurrentRoom != null ){
                playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
                PlayerCount.text = playersCount + "/" + maxPlayersPerRoom;
            }
            
            if(playersCount>=2 ){
                StartButton.SetActive(true);
            }
            else{
                StartButton.SetActive(false);
            }

            if(VariablesConfig.idJugador == 0){
                if(playersCount != 0){
                    VariablesConfig.idJugador = playersCount;
                    Debug.Log("MI ID ES: " + VariablesConfig.idJugador);
                }
            }
        }

    }
 
    public void Connect()
    {
        if(!PhotonNetwork.IsConnected){
            if(PhotonNetwork.ConnectUsingSettings())
                Log.text += "\nConectado al Servidor ...";
            else
            {
                Log.text += "\nLa conexión al servidor falló";
            }
        }
    }

    public override void OnConnectedToMaster(){
        ConnectButton.interactable = false;
        JoinRandonButton.interactable = true;
    }

    public void JoinRandom(){
        if(!PhotonNetwork.JoinRandomRoom()){
            Log.text += "\nLa unión Falló";
        }
    }

    public override void  OnJoinRandomFailed(short returnCode, string message){
        Log.text += "\nNo hay salas para unirse, Creando una sala ...";
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CleanupCacheOnLeave = false;
        
        //ANTIGUO SCRIPT PARA CREAR SALA
        // if(PhotonNetwork.CreateRoom(null,new Photon.Realtime.RoomOptions(){MaxPlayers = maxPlayersPerRoom})){
        //     Log.text += "\nSala Creada";
        // }
        
        if(PhotonNetwork.CreateRoom(null,roomOptions)){
            Log.text += "\nSala Creada";
        }
        else{
            Log.text += "\nFallo al crear sala";
        }
    }

    public override void OnJoinedRoom(){
        Log.text += "\nUnido correctamente";
        JoinRandonButton.interactable = false;
    }

    public void startGame(){
        VariablesConfig.nTotalJugadores = playersCount;
        PhotonNetwork.LoadLevel(7);
        DontDestroyOnLoad(this);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {   
        base.OnPlayerLeftRoom(otherPlayer);
        VariablesConfig.nTotalJugadores -= 1;
        VariablesConfig.salio = true;
    }

}
