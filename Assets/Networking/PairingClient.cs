﻿using System.Collections;
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