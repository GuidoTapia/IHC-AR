using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class movimientoOso : MonoBehaviourPunCallbacks
{
    public Camera FPSCamera;
	public float horizontalSpeed;
	public float verticalSpeed;

	float h;
	float v;

    void Start(){
        PhotonView myPV = GetComponent<PhotonView>();
        if(!myPV.IsMine){
            FPSCamera.enabled = false;
            this.enabled = false;
        }
    }

	void Update (){
		h = horizontalSpeed * Input.GetAxis ("Mouse X");
		v = verticalSpeed * Input.GetAxis ("Mouse Y");
		
		transform.Rotate (0,h,0);
		FPSCamera.transform.Rotate(-v,0,0);

		if (Input.GetKey(KeyCode.W))
		{
			transform.Translate (0,0,0.1f);
		}
		else 
		{
			if (Input.GetKey(KeyCode.S))
			{
				transform.Translate (0,0,-0.1f);
			}
			else
			{
				if (Input.GetKey(KeyCode.A))
				{
					transform.Translate (-0.1f,0,0);
				}
				else
				{
					if (Input.GetKey(KeyCode.D))
					{
						transform.Translate(0.1f,0,0);
					}
				}
			}
		}
	}
}
