using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworkDiscovery : NetworkDiscovery
{
    [SerializeField]
    ScriptableString serverIpAdress;

	void Start ()
    {
        Initialize();
        StartAsClient();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        //Restarting network transport is a workaround to avoid an error cause by StopBroadcast(), the update function of the network discovery still run for nothing and deleting it cause errors
        NetworkTransport.Shutdown();
        NetworkTransport.Init();
        serverIpAdress.value = fromAddress;
    }
}
