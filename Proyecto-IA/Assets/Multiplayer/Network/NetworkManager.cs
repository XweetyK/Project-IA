using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;

public class NetworkManager : Singleton<NetworkManager>, IReceiveData
{
    public IPAddress ipAddress;
    public int port;
    public bool isServer;
    public long clientSalt;
    public long serverSalt;

    public Client.State state
    {
        get; private set;
    }

    public int TimeOut = 30;

    public Action<byte[], IPEndPoint> OnReceiveEvent;

    private UdpConnection connection;

    private readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();
    private readonly Dictionary<IPEndPoint, int> ipToId = new Dictionary<IPEndPoint, int>();

    int clientId = 0; // This id should be generated during first handshake

    protected override void Initialize(){
        clientSalt = 0;
        state = Client.State.Disconnected;
    }

    public void StartServer(int port)
    {
        isServer = true;
        this.port = port;
        connection = new UdpConnection(port, this);
    }

    public bool StartClient(IPAddress ip, int port)
    {
        isServer = false;
        
        this.port = port;
        this.ipAddress = ip;
        
        connection = new UdpConnection(ip, port, this);

        return true;
    }

    void AddClient(IPEndPoint ip, long clientSalt, long serverSalt)
    {
        if (!ipToId.ContainsKey(ip))
        {
            Debug.Log("Adding client: " + ip.Address);

            int id = clientId;
            ipToId[ip] = clientId;
            
            clients.Add(clientId, new Client(ip, id, Time.realtimeSinceStartup,clientSalt,serverSalt));

            clientId ++;
        }
    }

    void RemoveClient(IPEndPoint ip)
    {
        if (ipToId.ContainsKey(ip))
        {
            Debug.Log("Removing client: " + ip.Address);
            clients.Remove(ipToId[ip]);
        }
    }

    public void OnReceiveData(byte[] data, IPEndPoint ip)
    {
        if (OnReceiveEvent != null)
            OnReceiveEvent.Invoke(data, ip);
    }

    public void SendToServer(byte[] data)
    {
        connection.Send(data);
    }

    public void Broadcast(byte[] data)
    {
        using (var iterator = clients.GetEnumerator())
        {
            while (iterator.MoveNext())
            {
                if(iterator.Current.Value.state == Client.State.Connected)
                connection.Send(data, iterator.Current.Value.ipEndPoint);
            }
        }
    }

    void Update()
    {
        if (connection != null)
            connection.FlushReceiveData();
    }
}
