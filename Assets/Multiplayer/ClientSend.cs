﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    //My custom function for sending client computed data directly to server
    //Instead of doing computation for player movement on the server
    public static void PlayerData(Vector3 _location, Quaternion _rotation)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerData))
        {
            _packet.Write(_location);
            _packet.Write(_rotation);

            SendUDPData(_packet);
        }
    }

    /* For testing UDP
    public static void UDPTestReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.udpTestReceived))
        {
            _packet.Write("Client received UDP packet!");
            SendUDPData(_packet);
        }
    }
    */
    #endregion
}