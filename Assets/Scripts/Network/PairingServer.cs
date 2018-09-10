using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PairingServer : MonoBehaviour
{
    NetworkServerSimple server = null;
    List<NetworkConnection> clientConnectionList = new List<NetworkConnection>();

    string tabletId = null;
    string headsetId = null;

    public int serverPort = 9995;

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
        server.RegisterHandler(CustomMsgType.PairingRequest, OnPairingRequest);

        server.Listen(serverPort);
    }

    void StopServer()
    {
        server.Stop();
        server = null;
    }

    void OnConnect(NetworkMessage netMsg)
    {
        Debug.Log("Pairing Client connect");//to be deleted
        if(clientConnectionList.Count<2)
        {
            clientConnectionList.Add(netMsg.conn);
        }
        else
        {
            SendPairingResult(false, "Pairing failed : Already 2 device pairing!");
        }
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        clientConnectionList.Remove(netMsg.conn);
        if (clientConnectionList.Count == 0)
        {
            tabletId = null;
            headsetId = null;
        }
    }

    void OnPairingRequest(NetworkMessage netMsg)
    {
        PairingRequestMessage msg = netMsg.ReadMessage<PairingRequestMessage>();
        if(msg.deviceType == Constants.ANDROID_TABLET)
        {
            tabletId = msg.deviceId;
            Debug.Log("Pairing request tablet!");//to be deleted
        }
        else if (msg.deviceType == Constants.OCCULUS_GO_HEADSET)
        {
            headsetId = msg.deviceId;
            Debug.Log("Pairing request headset!");//to be deleted
        }

        if(tabletId != null && headsetId != null)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            SQLiteUtilities.AddPairing(tabletId, headsetId);
#endif
            tabletId = null;
            headsetId = null;
            SendPairingResult(true, "Pairing was a success!");
            Debug.Log("Pairing success!");//to be deleted
        }
    }

    public void SendPairingResult(bool isPairingSuccess,string message)
    {
        PairingResultMessage pairingResultMsg = new PairingResultMessage();
        pairingResultMsg.isPairingSucess = isPairingSuccess;
        pairingResultMsg.message = message;

        foreach (NetworkConnection conn in clientConnectionList)
        {
            conn.Send(CustomMsgType.PairingResult, pairingResultMsg);
        }
    }

}
