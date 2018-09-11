using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworkManager : NetworkManager
{
    [SerializeField]
    ScriptableString serverIpAdress;

    int serverPort = 7777;

    int i = 0;//temps

    private void Start()
    {
        serverIpAdress.valueChangedEvent += StartConnection;
    }

    public void StartConnection ()
    {
        networkPort = serverPort;
        networkAddress = serverIpAdress.value;
        StartClient();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        i = i + 1;
        Debug.Log("OnClientConnect : " + i);
    }
}
