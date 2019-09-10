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
        ScriptableDeviceType deviceType;

        [SerializeField]
        ScriptableString serverIpAdress;

        [SerializeField]
        GameObject connectingToPairingServerPanel;

        [SerializeField]
        GameObject pairingRequestSentPanel;

        [SerializeField]
        GameObject pairingResultSuccessPanel;

        [SerializeField]
        GameObject pairingResultFailedPanel;

        [SerializeField]
        private UnityEngine.UI.Text text;// pairingResultFailedPanel;


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

            connectingToPairingServerPanel.SetActive(false);

            pairingRequestSentPanel.SetActive(true);
        }

        void OnDisconnect(NetworkMessage netMsg)
        {
            StopClient();
        }

        void OnPairingResult(NetworkMessage netMsg)
        {
            PairingResultMessage msg = netMsg.ReadMessage<PairingResultMessage>();

            pairingRequestSentPanel.SetActive(false);

            if (msg.isPairingSucess)
            {
                pairingResultSuccessPanel.SetActive(true);
                StopClient();
            }
            else
            {
                pairingResultFailedPanel.SetActive(true);
                StopClient();
            }
        }

        public void SendPairingRequest()
        {
            PairingRequestMessage pairingRequestMsg = new PairingRequestMessage();
            pairingRequestMsg.deviceId = SystemInfo.deviceUniqueIdentifier;

            string deviceName = SystemInfo.deviceModel;

            if (deviceType.Value == Utils.DeviceType.Tablet)
            {
                pairingRequestMsg.deviceType = Utils.DeviceType.Tablet;
            }
            else if (deviceType.Value == Utils.DeviceType.Headset)
            {
                pairingRequestMsg.deviceType = Utils.DeviceType.Headset;
            }

            client.Send(pairingRequestMsg.GetMsgType(), pairingRequestMsg);
        }

    }
}