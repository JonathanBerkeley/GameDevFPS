using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public GameObject projectilePrefab;
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        //Connecting UDP via the existing TCP connection
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        
        //Debug.Log("Position was read for player with ID " + _id + " : " + _position);
        GameManager.players[_id].transform.position = _position;
    }
    
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        
        //Debug.Log("Rotation was read for player with ID " + _id + " : " + _rotation);
        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void ProjectileData(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _location = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();


    }

    /* For testing UDP
    public static void UDPTest(Packet _packet)
    {
        string _msg = _packet.ReadString();
        Debug.Log($"Received packet through UDP with message: {_msg}");
        ClientSend.UDPTestReceived();
    }
    */
}
