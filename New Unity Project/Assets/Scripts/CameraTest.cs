using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CameraTest : NetworkBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public Camera cam;

    void Update()
    {

        if (!isLocalPlayer)
        {
            cam.enabled = false;
            return;
        }
    }
}