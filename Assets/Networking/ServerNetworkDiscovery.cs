using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UdeS.Promoscience.Network
{

    public class ServerNetworkDiscovery : ConnectionDiscovery
    {
        void Start()
        {
            StartBroadcasting("algorinthe", "HELLO");
        }
    }
}
