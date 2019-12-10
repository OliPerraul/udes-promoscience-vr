using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;
using UdeS.Promoscience.UI;

using System.Threading;
using System;


// TODO put pairing in different scene

namespace UdeS.Promoscience.Network
{
    public class PairingServer : Cirrus.BaseSingleton<PairingServer>
    {
        private NetworkServerSimple server = null;

        private List<NetworkConnection> clientConnectionList = new List<NetworkConnection>();

        // Tablet, Headset
        public List<string> headsets;

        public List<string> tablets;
        
        public Dictionary<string, NetworkConnection> connections;

        [SerializeField]
        public int serverPort = 9995;

        private Mutex pairingMutex = new Mutex();

        private Mutex startedMutex = new Mutex();

        private bool isStarted = false;

        public void Awake()
        {
            Persist();

            headsets = new List<string>();
            tablets = new List<string>();
            connections = new Dictionary<string, NetworkConnection>();
        }

        void Update()
        {
            if (server != null)
            {
                server.Update();
            }
        }


        public void StartServer()
        {
            startedMutex.WaitOne();

            isStarted = true;

            server = new NetworkServerSimple();

            server.RegisterHandler(MsgType.Connect, OnConnect);

            server.RegisterHandler((short)CustomMsgType.PairingRequest, OnPairingRequest);

            server.RegisterHandler((short)CustomMsgType.UnpairingRequest, OnUnpairingRequest);

            server.Listen(serverPort);

            startedMutex.ReleaseMutex();
        }

        public void StopServer()
        {
            startedMutex.WaitOne();

            isStarted = false;

            server.Stop();

            server = null;

            startedMutex.ReleaseMutex();
        }

        void OnConnect(NetworkMessage netMsg)
        {
            clientConnectionList.Add(netMsg.conn);
        }

        private void OnUnpairingRequest(NetworkMessage netMsg)
        {
            clientConnectionList.Remove(netMsg.conn);

            PairingRequestMessage msg = netMsg.ReadMessage<PairingRequestMessage>();
            string deviceId = msg.deviceId;

            NetworkConnection conn;

            pairingMutex.WaitOne();

            headsets.Remove(deviceId);
            tablets.Remove(deviceId);
            connections.Remove(deviceId);

            pairingMutex.ReleaseMutex();
        }

        private void OnPairingRequest(NetworkMessage netMsg)
        {
            PairingRequestMessage msg = netMsg.ReadMessage<PairingRequestMessage>();
            string headsetId = null;
            string tabletId = null;
            NetworkConnection tabletCon;
            NetworkConnection headsetCon;

            pairingMutex.WaitOne();

            tabletId = msg.deviceId;
            headsetId = msg.deviceId;
            tabletCon = netMsg.conn;
            headsetCon = netMsg.conn;

            if (!connections.ContainsKey(tabletId))
            {
                connections.Add(headsetId, headsetCon);

                if (msg.deviceType == DeviceType.Tablet)
                {
                    if (headsets.Count != 0)
                    {
                        headsetId = headsets[0];
                        if (connections.TryGetValue(headsetId, out headsetCon))
                        {
                            Pair(tabletCon, tabletId, headsetCon, headsetId);
                            tablets.Remove(tabletId);
                            headsets.Remove(headsetId);
                        }
                    }
                    else
                    {
                        tablets.Add(tabletId);
                    }
                }
                else if (msg.deviceType == DeviceType.Headset)
                {
                    if (tablets.Count != 0)
                    {
                        tabletId = tablets[0];
                        if (connections.TryGetValue(tabletId, out tabletCon))
                        {
                            Pair(tabletCon, tabletId, headsetCon, headsetId);
                            tablets.Remove(tabletId);
                            headsets.Remove(headsetId);
                        }
                    }
                    else
                    {
                        headsets.Add(headsetId);
                    }
                }
                else
                {
                    SendTargetPairingResult(netMsg.conn, false);
                }
            }
            

            pairingMutex.ReleaseMutex();
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