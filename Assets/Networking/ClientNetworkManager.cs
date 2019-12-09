using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;

namespace UdeS.Promoscience.Network
{
    public class ClientNetworkManager : NetworkManager
    {
        [SerializeField]
        ControlsAsset controls;

        [SerializeField]
        ScriptableBoolean isConnectedToServer;

        [SerializeField]
        ScriptableString serverIpAdress;

        int serverPort = 7777;

        private void Start()
        {
            // TODO: Asset Network manager to hold these values
            //isConnectedToServer.Value = false;
            //serverIpAdress.Value = "";

            serverIpAdress.valueChangedEvent += StartConnection;
        }

        public void StartConnection()
        {
            //if (serverIpAdress.Value == "")
            //    return;

            networkPort = serverPort;
            networkAddress = serverIpAdress.Value;
            StartClient();
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
            isConnectedToServer.Value = true;
        }

        public override void OnClientDisconnect(NetworkConnection conn)
        {
            base.OnClientDisconnect(conn);

            isConnectedToServer.Value = false;

            StartClient();
        }

    }
}