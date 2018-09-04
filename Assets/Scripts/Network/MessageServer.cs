using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageServer : MonoBehaviour {

    NetworkServerSimple server = null;
    NetworkConnection clientConnection = null;

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
        server.RegisterHandler(CustomMsgType.Action, OnAction);

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
        clientConnection = netMsg.conn;
        UITextManager.instance.ShowMessageButtonGroup();
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        Debug.Log("Client disconnect");
        UITextManager.instance.SetMessageText("Client disconnected!");
        UITextManager.instance.HideMessageButtonGroup();
        clientConnection = null;
        StopServer();//Might be changed when need reconnection?
    }

    void OnAction(NetworkMessage netMsg)
    {
        ActionMessage msg = netMsg.ReadMessage<ActionMessage>();
        UITextManager.instance.SetMessageText("Action id: " + msg.actionId);
    }

    public void SendDirective(int id)
    {
        DirectiveMessage directiveMsg = new DirectiveMessage();
        directiveMsg.directiveId = id;

        clientConnection.Send(CustomMsgType.Directive, directiveMsg);
    }

}
