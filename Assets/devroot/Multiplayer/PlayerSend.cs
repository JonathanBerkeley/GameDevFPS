using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Jonathan Berkeley
//This is my class to send client computed player data to the server
public class PlayerSend : MonoBehaviour
{
    public GameObject cam;

    private Vector3 location;
    private Quaternion easyRotation;
    private Quaternion camRotation;
    private Quaternion finalRotation;

    private void FixedUpdate()
    {
        location = gameObject.transform.position;
        easyRotation = gameObject.transform.rotation;
        camRotation = cam.transform.rotation;

        finalRotation = new Quaternion(camRotation.x, easyRotation.y, camRotation.z, easyRotation.w);

        SendPlayerDataToServer();
    }

    private void SendPlayerDataToServer()
    {
        ClientSend.PlayerData(location, finalRotation);
    }
}
