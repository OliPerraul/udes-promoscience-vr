using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageClient : MonoBehaviour
{
    public string chatServerHost = "10.44.88.31";//10.236.55.233
    public int chatServerPort = 9999;

    NetworkClient client = null;

    private void Start()
    {
        StartClient();
    }

    void StartClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnect);
        //client.RegisterHandler(ChatMsg.Login, OnLogin);
       // client.RegisterHandler(ChatMsg.Talk, OnTalk);

        client.Connect(chatServerHost, chatServerPort);
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
    }

}
