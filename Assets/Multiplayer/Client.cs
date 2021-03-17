using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using FlagTranslations;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;

    public string ip = "";
    public int port = 24745;
    public int myId = 0;
    public TCP tcp;
    public UDP udp;

    private bool isConnected = false;
    private delegate void PacketHandler(Packet _packet);
    private static Dictionary<int, PacketHandler> packetHandlers;

    private void Awake()
    {
        //Make this a singleton class
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already in existence, destroying self");
            Destroy(this);
        }
    }

    private void Start()
    {
        tcp = new TCP();
    }

    private void OnApplicationQuit()
    {
        Disconnect("OnApplicationQuit hooked");
    }

    //Without an IP (for testing)
    public void ConnectToServer()
    {
        InitializeClientData();
        isConnected = true;
        tcp.Connect();
    }

    public void ConnectToServer(string withIP)
    {
        //Checks if the IP is valid and the port is open on this games 
        //&& CheckPortOpen(withIP, port, new TimeSpan(0, 0, 5))
        if (IPAddress.TryParse(withIP, out _) )
        {
            this.ip = withIP;
            udp = new UDP();
            InitializeClientData();
            isConnected = true;
            tcp.Connect();

        }
        else
        {
            AsyncSlave.slave.AddTask(() =>
            {
                UIManager.instance.RestoreUI();
            });
        }
    }
    /*
    * With thanks to https://stackoverflow.com/questions/11837541/check-if-a-port-is-open
    */
    private bool CheckPortOpen(string host, int port, TimeSpan timeout)
    {
        try
        {
            using (var client = new TcpClient())
            {
                IAsyncResult response = client.BeginConnect(host, port, null, null);
                bool portStatus = response.AsyncWaitHandle.WaitOne(timeout);
                client.EndConnect(response);
                return portStatus;
            }
        }
        catch
        {
            return false;
        }
    }

    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private Packet receivedData;
        private byte[] receiveBuffer;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            try
            {
                socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
            }
            catch (SocketException sx)
            {
                Debug.Log($"Host not found: {sx}");
                Disconnect(sx.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult _result)
        {
            try
            {
                socket.EndConnect(_result);
            }
            catch (Exception ex)
            {
                Debug.Log($"ConnectCallback error: {ex}");
                Disconnect(ex.ToString());

                //Tell user the reason for the failed connection
                GameManager.instance.ProcessClientMessage(ClientCodeTranslations.connectionRefused);
            }
            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            receivedData = new Packet();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        public void SendData(Packet _packet)
        {
            try
            {
                if (socket != null)
                {
                    stream.BeginWrite(_packet.ToArray(), 0, _packet.Length(), null, null);
                }

            }
            catch (Exception ex)
            {
                Debug.Log($"Error sending data to server via TCP: {ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            //Below segment is identical to client servers
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    instance.Disconnect($"_byteLength <= 0(Fatal)");
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                receivedData.Reset(HandleData(_data));
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
            }
            catch (Exception ex)
            {
                Debug.Log($"ReceiveCallback caught exception: {ex}");
                Disconnect($"Due to exception {ex}");
                AsyncSlave.slave.AddTask(() =>
                {
                    GameManager.instance.LoadScene("MultiScene", ClientCodeTranslations.lostConnection);
                });
            }
        }

        private bool HandleData(byte[] _data)
        {
            int _packetLength = 0;

            receivedData.SetBytes(_data);

            if (receivedData.UnreadLength() >= 4)
            {
                _packetLength = receivedData.ReadInt();
                if (_packetLength <= 0)
                {
                    return true;
                }
            }
            while (_packetLength > 0 && _packetLength <= receivedData.UnreadLength())
            {
                byte[] _packetBytes = receivedData.ReadBytes(_packetLength);
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet _packet = new Packet(_packetBytes))
                    {
                        int _packetId = _packet.ReadInt();
                        packetHandlers[_packetId](_packet);
                    }
                });

                _packetLength = 0;
                if (receivedData.UnreadLength() >= 4)
                {
                    _packetLength = receivedData.ReadInt();
                    if (_packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            if (_packetLength <= 1)
            {
                return true;
            }

            return false;
        }

        internal void Disconnect(String reason)
        {
            instance.Disconnect(reason);

            stream = null;
            receivedData = null;
            receiveBuffer = null;
            socket = null;
        }

    }

    public class UDP
    {
        public UdpClient socket;
        public IPEndPoint endPoint;

        public UDP()
        {
            Debug.Log($"UDP - {IPAddress.Parse(instance.ip)} {instance.port}");
            endPoint = new IPEndPoint(IPAddress.Parse(instance.ip), instance.port);
        }

        public void Connect(int _localPort)
        {
            socket = new UdpClient(_localPort);

            socket.Connect(endPoint);
            socket.BeginReceive(ReceiveCallback, null);

            using (Packet _packet = new Packet())
            {
                SendData(_packet);
            }
        }

        public void SendData(Packet _packet)
        {
            try
            {
                _packet.InsertInt(instance.myId);
                if (socket != null)
                {
                    socket.BeginSend(_packet.ToArray(), _packet.Length(), null, null);
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Error sending data to server through UDP: {ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                byte[] _data = socket.EndReceive(_result, ref endPoint);
                socket.BeginReceive(ReceiveCallback, null);

                //Checks if there is a full packet, if not, returns outside of method
                if (_data.Length < 4)
                {
                    instance.Disconnect("Unfilled packet (Fatal)");
                    return;
                }

                HandleData(_data);
            }
            catch (Exception ex)
            {
                Disconnect($"UDP ReceiveCallback exception {ex}");
            }
        }

        private void HandleData(byte[] _data)
        {
            using (Packet _packet = new Packet(_data))
            {
                int _packetLength = _packet.ReadInt();
                _data = _packet.ReadBytes(_packetLength);

            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet _packet = new Packet(_data))
                {
                    int _packetId = _packet.ReadInt();
                    packetHandlers[_packetId](_packet);
                }
            });
        }

        internal void Disconnect(String reason)
        {
            instance.Disconnect(reason);
            endPoint = null;
            socket = null;
        }
    }

    private void InitializeClientData()
    {
        packetHandlers = new Dictionary<int, PacketHandler>()
        {
            { (int)ServerPackets.welcome, ClientHandle.Welcome },
            { (int)ServerPackets.spawnPlayer, ClientHandle.SpawnPlayer },
            { (int)ServerPackets.playerDisconnected, ClientHandle.PlayerDisconnected },
            { (int)ServerPackets.playerPosition, ClientHandle.PlayerPosition },
            { (int)ServerPackets.playerRotation, ClientHandle.PlayerRotation },
            { (int)ServerPackets.projectileData, ClientHandle.ProjectileData },
            { (int)ServerPackets.userMessage, ClientHandle.ClientChat },
            { (int)ServerPackets.serverControlComms, ClientHandle.ServerControlMessages }
            
            //{ (int)ServerPackets.udpTest, ClientHandle.UDPTest }
        };
        Debug.Log("Initialized packets.");
    }

    //Disconnects client connection
    private void Disconnect(String reason)
    {
        if (isConnected)
        {
            isConnected = false;

            //Reset dictionaries
            GameManager.instance.ResetDictionary();
            try
            {
                tcp.socket.Close();
                udp.socket.Close();
                Debug.Log($"Disconnected from server with reason: {reason}");
            }
            catch (NullReferenceException ex)
            {
                Debug.Log("Exception in Client Disconnect: " + ex + " while attempting disconnect for reason: " + reason);
            }
            catch (Exception ex)
            {
                Debug.Log("Exception in Client Disconnect: " + ex + " while attempting disconnect for reason: " + reason);
            }
            finally
            {
                AsyncSlave.slave.AddTask(() =>
                {
                    UIManager.instance.RestoreUI();
                });
            }
        }
    }

    //For others scripts to request disconnect
    public void RequestClientDisconnect(String type)
    {
        if (type == "UDP")
        {
            instance.udp.Disconnect($"RequestClientDisconnect {type}");
        }
        else if (type == "TCP")
        {
            instance.tcp.Disconnect($"RequestClientDisconnect {type}");
        }
        else
        {
            instance.udp.Disconnect($"RequestClientDisconnect {type}");
            instance.tcp.Disconnect($"RequestClientDisconnect {type}");
        }
        GameManager.instance.ResetDictionary();
    }

    public bool GetClientConnected()
    {
        return this.isConnected;
    }


    //For other scripts to set ip
    public void SetIP(string newIP)
    {
        this.ip = newIP;
    }
}
