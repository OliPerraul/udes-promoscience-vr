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

        // Tablet, Headset
        public List<string> headsets;
        public List<string> tablets;
        public Dictionary<NetworkConnection, string> connections;
        public Dictionary<string, NetworkConnection> connectionsInverse;

        public int serverPort = 9995;

        private void Start()
        {
            StartServer();
        }

        public void Awake()
        {
            headsets = new List<string>();
            tablets = new List<string>();
            connections = new Dictionary<NetworkConnection, string>();
            connectionsInverse = new Dictionary<string, NetworkConnection>();

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
            string deviceId = null;

            if (connections.TryGetValue(netMsg.conn, out deviceId))
            {
                // Try remove from headest
                headsets.Remove(deviceId);
                // Otherwise try remove from tablets
                tablets.Remove(deviceId);
            }
        }

        void OnPairingRequest(NetworkMessage netMsg)
        {
            PairingRequestMessage msg = netMsg.ReadMessage<PairingRequestMessage>();

            string headsetId = null;
            string tabletId = null;

            NetworkConnection tabletCon;
            NetworkConnection headsetCon;

            if (connections.TryGetValue(netMsg.conn, out headsetId))
            {
                if (msg.deviceType == Utils.DeviceType.Tablet)
                {
                    tabletId = msg.deviceId;

                    if (headsets.Count != 0)
                    {
                        headsetId = headsets[0];
                        if (connectionsInverse.TryGetValue(headsetId, out headsetCon))
                        {
                            Pair(netMsg.conn, tabletId, headsetCon, headsetId);
                        }                        
                    }
                    else
                    {
                        tablets.Add(tabletId);
                    }

                    connections.Add(netMsg.conn, headsetId);
                    connectionsInverse.Add(headsetId, netMsg.conn);
                }
                else if (msg.deviceType == Utils.DeviceType.Headset)
                {
                    headsetId = msg.deviceId;

                    if (headsets.Count != 0)
                    {
                        tabletId = tablets[0];
                        if (connectionsInverse.TryGetValue(tabletId, out tabletCon))
                        {
                            Pair(tabletCon, tabletId, netMsg.conn, headsetId);
                        }
                    }
                    else
                    {
                        headsets.Add(headsetId);
                    }

                    connections.Add(netMsg.conn, headsetId);
                    connectionsInverse.Add(headsetId, netMsg.conn);
                }
                else
                {
                    SendTargetPairingResult(netMsg.conn, false);
                }
            }
        }

        private bool Pair(NetworkConnection tabletConnection, string tabletId, NetworkConnection headsetConnection, string headsetId)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            SQLiteUtilities.InsertPairing(tabletId, headsetId);
#endif
            SendPairingResult(true, tabletConnection, headsetConnection);
            return true;
        }

        public void SendPairingResult(bool isPairingSuccess, NetworkConnection tabletConnection, NetworkConnection headsetConnection)
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