using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class FrameScriptCam : MonoBehaviourPunCallbacks
{
    PhotonView myPV;
    Camera myCamera;
    // Start is called before the first frame update
    void Start()
    {
        myPV = GetComponent<PhotonView>();
        myCamera = GetComponent<Camera>();
        if (!myPV.IsMine)
        {
            myCamera.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
