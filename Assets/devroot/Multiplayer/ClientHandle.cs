using System.Collections;
using System.Collections.Generic;
using FlagTranslations;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{

    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();
        ulong _key = _packet.ReadULong();
        ulong _token = _packet.ReadULong();

        Debug.Log($"Message from server: {_msg}");
        Debug.Log($"Validation data received: {_key} {_token}");

        ulong _responseToken = GenerateValidationResponse(_key);

        Debug.Log($"Token: {_token} should match {_responseToken}");

        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived(_responseToken);


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

    public static void PlayerDisconnected(Packet _packet)
    {
        //The ID of the disconnected player
        int _id = _packet.ReadInt();

        //Destroy the in-game prefab of this player
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        
        //Debug.Log("Position was read for player with ID " + _id + " : " + _position);

        /*
        foreach (var p in GameManager.players)
        {
            Debug.Log($"Key : {p.Key}, Value: {p.Value.username}");
        }
        */

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].transform.position = _position;
        }
        
    }
    
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        
        //Debug.Log("Rotation was read for player with ID " + _id + " : " + _rotation);

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].transform.rotation = _rotation;
        }
        
    }

    public static void ProjectileData(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _location = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.CreateProjectile(_id, _location, _rotation);
    }

    public static void ClientChat(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _message = _packet.ReadString();
        Debug.Log($"Message from {GameManager.players[_id].username}: {_message}");

        GameManager.instance.ReceiveChat(_id, _message);

    }

    public static void ServerControlMessages(Packet _packet)
    {
        ServerCodeTranslations _msg = (ServerCodeTranslations)_packet.ReadInt();

        GameManager.instance.ProcessServerMessage(_msg);
    }

    private static ulong GenerateValidationResponse(ulong _key)
    {
        ulong _output = _key ^ 0xDCEDCCCAAFFC;
        _output = (_output & 0xAFCFEFBEECE) >> 4 | (_output & 0xCACCADFFEFBCB) << 4;
        _output ^= 0xFC0C3FB10FF65435;

        return _output;
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
