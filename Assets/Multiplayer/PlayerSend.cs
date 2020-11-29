using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Jonathan Berkeley
//This is my class to send client computed player data to the server
public class PlayerSend : MonoBehaviour
{
    private Vector3 location;
    private Quaternion rotation;

    private void FixedUpdate()
    {
        location = gameObject.transform.position;
        rotation = gameObject.transform.rotation;
        SendPlayerDataToServer();
    }

    private void SendPlayerDataToServer()
    {
        ClientSend.PlayerData(location, rotation);
    }
}
