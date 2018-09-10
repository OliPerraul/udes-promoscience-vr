using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PairingClient : MonoBehaviour
{
    [SerializeField]
    ScriptableString serverIpAdress;

    public int serverPort = 9995;

    NetworkClient client = null;

    private void Start()
    {
        serverIpAdress.valueChangedEvent += StartClient;
    }

    void StartClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnect);
        client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
        client.RegisterHandler(CustomMsgType.PairingResult, OnPairingResult);

        client.Connect(serverIpAdress.value, serverPort);
    }

    void StopClient()
    {
        client.Disconnect();
        client = null;
    }

    void OnConnect(NetworkMessage netMsg)
    {
        SendPairingRequest();
        UITextManager.instance.SetText("Pairing : pairing request sent" );//Should be changed when UI changed
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        StopClient();
    }

    void OnPairingResult(NetworkMessage netMsg)
    {
        PairingResultMessage msg = netMsg.ReadMessage<PairingResultMessage>();
        if(msg.isPairingSucess)
        {
            UITextManager.instance.SetText("Pairing : " + msg.message);//Should be changed when UI changed
            StopClient();
        }
        else
        {
            UITextManager.instance.SetText("Pairing : " + msg.message);//Should be changed when UI changed
        }
    }

    public void SendPairingRequest()
    {
        PairingRequestMessage pairingRequestMsg = new PairingRequestMessage();
        pairingRequestMsg.deviceId = SystemInfo.deviceUniqueIdentifier;

        string deviceName = SystemInfo.deviceModel;

        if (deviceName == Constants.SAMSUNG_TABLET_SMT380)//Could be refactored if more devices are added, repeated in Player
        {
            pairingRequestMsg.deviceType = Constants.ANDROID_TABLET;
        }
        else if (deviceName == Constants.OCCULUS_GO_PACIFIC)
        {
            pairingRequestMsg.deviceType = Constants.OCCULUS_GO_HEADSET;
        }

        client.Send(CustomMsgType.PairingRequest, pairingRequestMsg);
    }

}
