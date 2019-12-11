using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace UdeS.Promoscience.Network
{

    public class ServerNetworkDiscovery : NetworkDiscoveryFix
    {
        public void DoStart()
        {
            StartBroadcasting("algorinthe", "HELLO");
        }

        public void DoStop()
        {
            Stop();
        }
    }
}
