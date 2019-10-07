using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ConnectionManager : Singleton<ConnectionManager>
{
   public enum State
    {
        ConnectionPending ,ChallengeRequest, ChallengeResponse, Connected
    }

    public State state
    {
        get; private set;
    }

    protected override void Initialize()
    {
        state = State.ConnectionPending;
    }

    public void startServer(int port){
        NetworkManager.Instance.StartServer(port);
        state = State.Connected;
    }

    public void connectToServer()
    {
        //TODO
    }
    private void MsgChallengeRequest()
    {
        //TODO
    }
    private void MsgChallengeResponse()
    {
        //TODO
    }
    private void MsgConnected()
    {
        //TODO
    }
}
