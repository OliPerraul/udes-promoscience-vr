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
            Initialize();
            StartAsServer();
        }
    }
}
