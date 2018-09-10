using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkDiscovery : NetworkDiscovery
{
	void Start ()
    {
        Initialize();
        StartAsServer();
    }
}
