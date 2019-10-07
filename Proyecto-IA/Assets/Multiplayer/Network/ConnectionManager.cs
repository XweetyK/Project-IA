using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public struct Client {
    public enum State { Disconnected, Connected }
    public float timeStamp;
    public long id;
    public IPEndPoint ipEndPoint;
    public State state;
    public long clientSalt;
    public long serverSalt;

    public Client(IPEndPoint ipEndPoint, long id, float timeStamp, long clientSalt, long serverSalt) {
        this.timeStamp = timeStamp;
        this.id = id;
        this.ipEndPoint = ipEndPoint;
        this.state = State.Disconnected;
        this.clientSalt = clientSalt;
        this.serverSalt = serverSalt;
    }
}

public class ConnectionManager : Singleton<ConnectionManager> {
    public readonly Dictionary<long, Client> clients = new Dictionary<long, Client>();
    public readonly Dictionary<IPEndPoint, long> IDs = new Dictionary<IPEndPoint, long>();
    private const float resendTime = 0.1f;
    private System.Action<bool> onConnect;

    public enum State { ConnectionPending, ChallengeRequest, ChallengeResponse, Connected}

    public State state { get; private set;}
    public bool isServer { get { return NetworkManager.Instance.isServer; } }
    public long clientID { get; private set; }
    public long clientSalt { get; private set; }
    public long serverSalt { get; private set; }
    protected override void Initialize() {
        state = State.ConnectionPending;
        clientSalt = 0;
        state = State.ConnectionPending;
        //PacketsManager.Instance.onPacketReceived += OnPacketReceived;
    }

    public void startServer(int port) {
        NetworkManager.Instance.StartServer(port);
        state = State.Connected;
    }

    public void connectToServer(IPAddress ip, int port, System.Action<bool> onConnectCallback) {
        if (!NetworkManager.Instance.StartClient(ip, port)) {
            if (onConnectCallback != null) { onConnectCallback(false); }
            return;
        }
        if (onConnectCallback != null) {
            onConnect += onConnectCallback;
        }

        clientSalt = LongGenerator.Range(0, long.MaxValue);
        state = State.ChallengeRequest;
        MsgConnectionRequest();
    }
    private void MsgConnectionRequest() {
        ConnectionRequestPacket request = new ConnectionRequestPacket();
        request.payload.clientsalt = clientSalt;
        SendToServer(request);
    }
    private void MsgChallengeRequest() {
        //TODO
    }
    private void MsgChallengeResponse() {
        //TODO
    }
    private void MsgConnected() {
        //TODO
    }
}
