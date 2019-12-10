using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UdeS.Promoscience.Network
{

    public class ServerNetworkDiscovery : NetworkDiscoveryFix
    {
        void Start()
        {
            StartBroadcasting("algorinthe", "HELLO");
        }

        private void OnDestroy()
        {
            Stop();
        }
    }
}
