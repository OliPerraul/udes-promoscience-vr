using Cirrus.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;


namespace UdeS.Promoscience.Network
{
    public enum ServerPairingState
    {
        None,
        Tablet,
        Headset,
        Both
    }

    public class PairingServer : Cirrus.BaseSingleton<PairingServer>
    {
        private NetworkServerSimple server = null;

        private List<NetworkConnection> clientConnectionList = new List<NetworkConnection>();

        private NetworkConnection tabletConnection = null;

        private NetworkConnection headsetConnection = null;

        [SerializeField]
        public string TabletId = "";

        [SerializeField]
        public string HeadsetId = "";

        public Cirrus.ObservableValue<ServerPairingState> PairingState = new Cirrus.ObservableValue<ServerPairingState>(Network.ServerPairingState.None);

        public int serverPort = 9995;

        private Mutex pairingMutex = new Mutex();

        private Mutex startedMutex = new Mutex();

        private bool isStarted = false;


        public void Awake()
        {
            Persist();
        }

        public void Update()
        {
            if (server != null)
            {
                server.Update();
            }
        }

        public void StartServer()
        {
            if (isStarted) return;

            startedMutex.WaitOne();

            isStarted = true;

            PairingState.Value = ServerPairingState.None;

            server = new NetworkServerSimple();

            server.RegisterHandler(MsgType.Connect, OnConnect);

            server.RegisterHandler(MsgType.Disconnect, OnDisconnect);

            server.RegisterHandler(PairingRequestMessage.GetCustomMsgType(), OnPairingRequest);

            server.Listen(serverPort);

            startedMutex.ReleaseMutex();
        }

        public void StopServer()
        {
            if (!isStarted) return;

            startedMutex.WaitOne();

            isStarted = false;

            server.Stop();

            server = null;

            HeadsetId = null;

            TabletId = null;

            startedMutex.ReleaseMutex();
        }

        private void OnConnect(NetworkMessage netMsg)
        {
            clientConnectionList.Add(netMsg.conn);
        }

        private void OnDisconnect(NetworkMessage netMsg)
        {
            clientConnectionList.Remove(netMsg.conn);
            if (netMsg.conn == tabletConnection)
            {
                TabletId = null;
                tabletConnection = null;
            }
            else if (netMsg.conn == headsetConnection)
            {
                HeadsetId = null;
                headsetConnection = null;
            }
        }

        private void OnPairingRequest(NetworkMessage netMsg)
        {
            PairingRequestMessage msg = netMsg.ReadMessage<PairingRequestMessage>();
            if (msg.deviceType == DeviceType.Tablet && TabletId.IsNullOrEmpty())
            {
                TabletId = msg.deviceId;
                tabletConnection = netMsg.conn;
                PairingState.Value = ServerPairingState.Tablet;
                Pairing();
            }
            else if (msg.deviceType == DeviceType.Headset && HeadsetId.IsNullOrEmpty())
            {
                HeadsetId = msg.deviceId;
                headsetConnection = netMsg.conn;
                PairingState.Value = ServerPairingState.Headset;
                Pairing();
            }
            else
            {
                PairingState.Value = ServerPairingState.None;
                SendTargetPairingResult(netMsg.conn, false);
            }
        }

        private void Pairing()
        {
            if (!TabletId.IsNullOrEmpty() && !HeadsetId.IsNullOrEmpty())
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                SQLiteUtilities.InsertPairing(TabletId, HeadsetId);
#endif
                PairingState.Value = ServerPairingState.Both;
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