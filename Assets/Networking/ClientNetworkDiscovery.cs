using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Network
{
    public class ClientNetworkDiscovery : ConnectionDiscovery
    {
        [SerializeField]
        private UnityEngine.UI.Text text;

        [SerializeField]
        ScriptableString serverIpAdress;

        void Start()
        {
            StartListening();
        }

        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            base.OnReceivedBroadcast(fromAddress, data);

            text.text = fromAddress;
            //Restarting network transport is a workaround to avoid an error cause by StopBroadcast() on pc, the update function of the network discovery still run for nothing and deleting it cause errors
            //NetworkTransport.Shutdown();
            //NetworkTransport.Init();

            //Causes an error on all devices but stop updating after the error and doesn't seems to afect other modules
            StopBroadcast();

            serverIpAdress.Value = fromAddress;
        }
    }
}
