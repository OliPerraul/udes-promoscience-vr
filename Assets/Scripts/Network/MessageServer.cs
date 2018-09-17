using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageServer : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger currentGameState;

    [SerializeField]
    ScriptableString pairedIpAdress;

    [SerializeField]
    ScriptableInteger directive;

    NetworkServerSimple server = null;
    NetworkConnection clientConnection = null;

    public int serverPort = 9999;

    private void Start()
    {
        pairedIpAdress.valueChangedEvent += StartServer;
        directive.valueChangedEvent += SendDirective;
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
        clientConnection = netMsg.conn;
        currentGameState.value = Constants.READY;
    }

    void OnDisconnect(NetworkMessage netMsg)
    {

        UITextManager.instance.HideMessageButtonGroup();//temp
        clientConnection = null;
        StopServer();//Might be changed when need reconnection?
    }

    void OnAction(NetworkMessage netMsg)
    {
        ActionMessage msg = netMsg.ReadMessage<ActionMessage>();
        UITextManager.instance.SetMessageText("Action id: " + msg.actionId);//temp
    }

    public void SendDirective()
    {
        DirectiveMessage directiveMsg = new DirectiveMessage();
        directiveMsg.directiveId = directive.value;

        clientConnection.Send(CustomMsgType.Directive, directiveMsg);
    }

}
