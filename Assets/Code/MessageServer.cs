using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageServer : MonoBehaviour {

    NetworkServerSimple server = null;

    public int serverPort = 9999;

    private void Start()
    {
        StartServer();
    }

    void Update()
    {
        if (server != null)
        {
            server.Update();
        }
    }

    void StartServer()
    {
        server = new NetworkServerSimple();
        server.RegisterHandler(MsgType.Connect, OnConnect);
        server.RegisterHandler(MsgType.Disconnect, OnDisconnect);
        //server.RegisterHandler(ChatMsg.ChannelJoin, OnChannelJoin);
        //server.RegisterHandler(ChatMsg.ChannelLeave, OnChannelLeave);
        //server.RegisterHandler(ChatMsg.ChannelCreate, OnChannelCreate);
        //server.RegisterHandler(ChatMsg.Talk, OnTalk);
        //server.RegisterHandler(ChatMsg.Login, OnLogin);

        server.Listen(serverPort);
    }

    void StopServer()
    {
        server.Stop();
        server = null;
    }

    void OnConnect(NetworkMessage netMsg)
    {
        Debug.Log("Client connect");
        UITextManager.instance.SetMessageText("Client connected!");
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        Debug.Log("Client disconnect");
        UITextManager.instance.SetMessageText("Client disconnected!");
    }
}
