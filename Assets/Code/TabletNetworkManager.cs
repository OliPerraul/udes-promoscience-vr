using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TabletNetworkManager : NetworkManager
{
    string serverIpAdress =  "192.168.0.102";//"10.44.88.31";
    int serverPort = 7777;

    void Start ()
    {
        networkPort = serverPort;
        networkAddress = serverIpAdress;
        StartClient();  
	}

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log("Client connected to a server");
        Debug.Log(conn.address);
    }
}
