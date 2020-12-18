using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gyroGodCam : MonoBehaviour
{
    public Camera FPS_Camera;
    private bool gyroEnabled;
    private Gyroscope gyro;

    private GameObject cameraContainer;
    private GameObject pj;
    private Quaternion rot;

    float h;
    float v;


    private void Start()
    {
        cameraContainer = GameObject.FindGameObjectWithTag("God");
        // pj = GameObject.FindGameObjectWithTag("Player");
        // cameraContainer.transform.position = transform.position;
        transform.SetParent(cameraContainer.transform);

        // if (Application.platform == RuntimePlatform.Android){
            gyroEnabled = EnableGyro();
        // }

    }

    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;

            // cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 0f);
            rot = new Quaternion(0, 0, 1, 0);

            return true;
        }
        return false;
    }
    private void Update()
    {
        if (gyroEnabled)
        {
            transform.localRotation = gyro.attitude * rot;

        }
    }
}
