using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.UI;


namespace UdeS.Promoscience.Network
{
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
            server.RegisterHandler(PairingRequestMessage.GetCustomMsgType(), OnPairingRequest);

            server.Listen(serverPort);
        }

        void StopServer()
        {
            server.Stop();
            server = null;
        }

        void OnConnect(NetworkMessage netMsg)
        {
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
            if (msg.deviceType == Utils.DeviceType.Tablet && tabletId == null)
            {
                tabletId = msg.deviceId;
                tabletConnection = netMsg.conn;
                Pairing();
            }
            else if (msg.deviceType == Utils.DeviceType.Headset && headsetId == null)
            {
                headsetId = msg.deviceId;
                headsetConnection = netMsg.conn;
                Pairing();
            }
            else
            {
                SendTargetPairingResult(netMsg.conn, false);
            }
        }

        void Pairing()
        {
            if (tabletId != null && headsetId != null)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                SQLiteUtilities.InsertPairing(tabletId, headsetId);
#endif
                SendPairingResult(true);
            }
        }

        public void SendPairingResult(bool isPairingSuccess)
        {
            PairingResultMessage pairingResultMsg = new PairingResultMessage();
            pairingResultMsg.isPairingSucess = isPairingSuccess;

            tabletConnection.Send(pairingResultMsg.GetMsgType(), pairingResultMsg);
            headsetConnection.Send(pairingResultMsg.GetMsgType(), pairingResultMsg);
        }

        public void SendTargetPairingResult(NetworkConnection target, bool isPairingSuccess)
        {
            PairingResultMessage pairingResultMsg = new PairingResultMessage();
            pairingResultMsg.isPairingSucess = isPairingSuccess;

            target.Send(pairingResultMsg.GetMsgType(), pairingResultMsg);
        }
    }
}