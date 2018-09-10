using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageClient : MonoBehaviour
{
    [SerializeField]
    ScriptableString pairedIpAdress;
    public int chatServerPort = 9999;

    NetworkClient client = null;

    private void Start()
    {
        pairedIpAdress.valueChangedEvent += StartClient;
    }

    void StartClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnect);
        client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
        client.RegisterHandler(CustomMsgType.Directive, OnDirective);

        client.Connect(pairedIpAdress.value, chatServerPort);
    }

    void StopClient()
    {
        client.Disconnect();
        client = null;
    }

    void OnConnect(NetworkMessage netMsg)
    {
        Debug.Log("Client connected!");
        UITextManager.instance.SetMessageText("Client connected!");
        UITextManager.instance.ShowMessageButtonGroup();
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        Debug.Log("Server disconnect");
        UITextManager.instance.SetMessageText("Server disconnected!");
        UITextManager.instance.HideMessageButtonGroup();
        StopClient();//Might be changed when need reconnection?
    }

    void OnDirective(NetworkMessage netMsg)
    {
        DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
        UITextManager.instance.SetMessageText("Action id: " + msg.directiveId);
    }

    public void SendAction(int id)
    {
        ActionMessage actionMsg = new ActionMessage();
        actionMsg.actionId = id;

        client.Send(CustomMsgType.Action, actionMsg);
    }

}
