using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PairingServer : MonoBehaviour
{
    NetworkServerSimple server = null;
    List<NetworkConnection> clientConnectionList = new List<NetworkConnection>();


    NetworkConnection tabletConnection = null;
    NetworkConnection headsetConnection = null;
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
        clientConnectionList.Add(netMsg.conn);
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        clientConnectionList.Remove(netMsg.conn);
        if (netMsg.conn == tabletConnection)
        {
            tabletId = null;
            tabletConnection = null;
        }
        else if (netMsg.conn == headsetConnection)
        {
            headsetId = null;
            headsetConnection = null;
        }
    }

    void OnPairingRequest(NetworkMessage netMsg)
    {
        PairingRequestMessage msg = netMsg.ReadMessage<PairingRequestMessage>();
        if(msg.deviceType == Constants.DEVICE_TABLET && tabletId == null)
        {
            tabletId = msg.deviceId;
            tabletConnection = netMsg.conn;
            Debug.Log("Pairing request tablet!");//to be deleted
            Pairing();
        }
        else if (msg.deviceType == Constants.DEVICE_HEADSET && headsetId == null)
        {
            headsetId = msg.deviceId;
            headsetConnection = netMsg.conn;
            Debug.Log("Pairing request headset!");//to be deleted
            Pairing();
        }
        else
        {
            SendTargetPairingResult(netMsg.conn, false, "Pairing failed : Already device type pairing!");
        }
    }

    void Pairing()
    {
        if (tabletId != null && headsetId != null)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            SQLiteUtilities.AddPairing(tabletId, headsetId);
#endif
            SendPairingResult(true, "Pairing was a success!");
            Debug.Log("Pairing success!");//to be deleted
        }
    }

    public void SendPairingResult(bool isPairingSuccess, string message)
    {
        PairingResultMessage pairingResultMsg = new PairingResultMessage();
        pairingResultMsg.isPairingSucess = isPairingSuccess;
        pairingResultMsg.message = message;

        tabletConnection.Send(CustomMsgType.PairingResult, pairingResultMsg);
        headsetConnection.Send(CustomMsgType.PairingResult, pairingResultMsg);
    }

    public void SendTargetPairingResult(NetworkConnection target, bool isPairingSuccess,string message)
    {
        PairingResultMessage pairingResultMsg = new PairingResultMessage();
        pairingResultMsg.isPairingSucess = isPairingSuccess;
        pairingResultMsg.message = message;

        target.Send(CustomMsgType.PairingResult, pairingResultMsg);
    }

}
