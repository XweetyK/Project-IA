using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System;

public struct Client
{
    public enum State{ Disconnected, Connected }
    public float timeStamp;
    public int id;
    public IPEndPoint ipEndPoint;
    public State state;
    public long clientSalt;
    public long serverSalt;

    public Client(IPEndPoint ipEndPoint, int id, float timeStamp, long clientSalt, long serverSalt)
    {
        this.timeStamp = timeStamp;
        this.id = id;
        this.ipEndPoint = ipEndPoint;
        this.state = State.Disconnected;
        this.clientSalt = clientSalt;
        this.serverSalt = serverSalt;
    }
}

public class NetworkManager : Singleton<NetworkManager>, IReceiveData
{
    public IPAddress ipAddress
    {
        get; private set;
    }

    public int port
    {
        get; private set;
    }

    public bool isServer
    {
        get; private set;
    }

    public long clientSalt
    {
        get; private set;
    }

    public long serverSalt
    {
        get; private set;
    }

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

    public void StartClient(IPAddress ip, int port, long clientSalt, long serverSalt)
    {
        isServer = false;
        
        this.port = port;
        this.ipAddress = ip;
        
        connection = new UdpConnection(ip, port, this);

        AddClient(new IPEndPoint(ip, port), clientSalt, serverSalt);
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
