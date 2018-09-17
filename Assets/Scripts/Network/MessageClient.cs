using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageClient : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger currentGameState;

    [SerializeField]
    ScriptableString pairedIpAdress;

    [SerializeField]
    ScriptableInteger directive;

    [SerializeField]
    //ScriptableVect2Int action; Might need a other format depending on movement type

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
        currentGameState.value = Constants.READY;

        //ScriptableVect2Int.valueChangedEvent += SendAction
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        UITextManager.instance.SetMessageText("Server disconnected!");//temp
        UITextManager.instance.HideMessageButtonGroup();//temp
        StopClient();//Might be changed when need reconnection?
    }

    void OnDirective(NetworkMessage netMsg)
    {
        DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
        directive.value = msg.directiveId;
        UITextManager.instance.SetMessageText("Action id: " + msg.directiveId);//temp
    }

    public void SendAction()
    {
        ActionMessage actionMsg = new ActionMessage();
        //actionMsg.actionId = ScriptableVect2Int.value;

        client.Send(CustomMsgType.Action, actionMsg);
    }

    public void SendAction(int id)//Temp
    {
        ActionMessage actionMsg = new ActionMessage();
        actionMsg.actionId = id;

        client.Send(CustomMsgType.Action, actionMsg);
    }

}
