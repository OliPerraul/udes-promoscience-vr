using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworkManager : NetworkManager
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableBoolean isConnectedToServer;

    [SerializeField]
    ScriptableString serverIpAdress;

    int serverPort = 7777;

    private void Start()
    {
        serverIpAdress.valueChangedEvent += StartConnection;
    }

    public void StartConnection ()
    {
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

        controls.IsControlsEnabled = false;//Should be moved to deconection/reconnection logic
        Debug.Log("You lost connection with the server!!!");//Same

        //StopClient();
        StartClient();

    }
}
