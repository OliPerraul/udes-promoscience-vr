using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Network
{
    public class PairingClient : MonoBehaviour
    {
        [SerializeField]
        private ScriptablePairingStatus pairingStatus;

        [SerializeField]
        ScriptableDeviceType deviceType;

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
            client.RegisterHandler(PairingResultMessage.GetCustomMsgType(), OnPairingResult);

            client.Connect(serverIpAdress.Value, serverPort);
        }

        void StopClient()
        {
            client.Disconnect();
            client = null;
        }

        void OnConnect(NetworkMessage netMsg)
        {
            SendPairingRequest();

            pairingStatus.Value = ScriptablePairingStatus.ConnectionStatus.Pairing;
        }
                       
        void OnDisconnect(NetworkMessage netMsg)
        {
            SendPairingRequest(false);

            StopClient();
        }

        void OnPairingResult(NetworkMessage netMsg)
        {
            if (pairingStatus.Value == ScriptablePairingStatus.ConnectionStatus.PairingSuccess)
                return;

            PairingResultMessage msg = netMsg.ReadMessage<PairingResultMessage>();

            if (msg.isPairingSucess)
            {
                pairingStatus.Value = ScriptablePairingStatus.ConnectionStatus.PairingSuccess;
            }
            else
            {
                pairingStatus.Value = ScriptablePairingStatus.ConnectionStatus.PairingFail;
            }
        }

 
        public void SendPairingRequest(bool pairing=true)
        {
            PairingRequestMessage pairingRequestMsg = new PairingRequestMessage();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            // Use project path as ID to enable debugging from the same device
            pairingRequestMsg.deviceId = Application.dataPath;// SystemInfo.deviceUniqueIdentifier;
#else
            pairingRequestMsg.deviceId = SystemInfo.deviceUniqueIdentifier;
#endif

            string deviceName = SystemInfo.deviceModel;

            if (deviceType.Value == Promoscience.DeviceType.Tablet)
            {
                pairingRequestMsg.deviceType = Promoscience.DeviceType.Tablet;
            }
            else if (deviceType.Value == Promoscience.DeviceType.Headset)
            {
                pairingRequestMsg.deviceType = Promoscience.DeviceType.Headset;
            }

            client.Send(
                (short) (pairing ? CustomMsgType.PairingRequest : CustomMsgType.UnpairingRequest), 
                pairingRequestMsg);
        }

    }
}