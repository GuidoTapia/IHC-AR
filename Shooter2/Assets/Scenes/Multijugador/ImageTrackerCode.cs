using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Wikitude;
using Photon.Pun;
using Photon.Realtime;


public class ImageTrackerCode : MonoBehaviourPunCallbacks
{
    public GameObject torre;
    public GameObject enemigo;
    public GameObject trackable;
    GameObject tickobj1, tickobj2;
    GameObject muros;
    // Start is called before the first frame update
    public void OnImageRecognized(ImageTarget target)
    {
        Debug.Log("Muro inicializado");
        //muros = trackable.transform.Find("Muros").gameObject;
        Invoke("CheckMarkers",0.1f);
    }
    public void OnImageLost(ImageTarget target)
    {
        Invoke("eliminar",0.1f);
    }
    void eliminar()
    {
        if (tickobj2)
        {
            PhotonNetwork.Destroy(tickobj2);
        }
        if (tickobj1)
        {
            PhotonNetwork.Destroy(tickobj1);
        }
    }
    void CheckMarkers()
    {
        Transform[] allTransforms = trackable.GetComponentsInChildren<Transform>();
        List<Transform> markers = new List<Transform>();
        foreach (Transform t in allTransforms)
        {
            if (t.parent.gameObject == trackable)
                markers.Add(t);
        }
        for (int i = 0; i < markers.Count; i++)
        {
            if (markers[i].gameObject.name.StartsWith("tower_"))
            {
                tickobj1 = PhotonNetwork.Instantiate(torre.name, markers[i].position, markers[i].rotation);
                tickobj1.transform.parent = markers[i];
            }
            if (markers[i].gameObject.name.StartsWith("enemy_"))
            {
                tickobj2 = PhotonNetwork.Instantiate(enemigo.name, markers[i].position, markers[i].rotation);
                tickobj2.transform.parent = markers[i];
            }
        }
    }
}
