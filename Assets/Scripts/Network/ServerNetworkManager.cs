using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkManager : NetworkManager
{
    public static ServerNetworkManager instance;

    [SerializeField]
    ScriptableServerGameInformation serverGameInformation;

    [SerializeField]
    ScriptableServerPlayerInformation serverPlayerInformation;

    Action<String> playerDisconnectfromServerEvent;

    const float informationSavingFrequencie = 10.0f;

    float timer = 0;

    bool isServerStarted = false;

    void Start ()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
	}

    private void Update()
    {

        if (!isServerStarted)
        {
            isServerStarted = true;
            serverGameInformation.LoadGameInformationFromDatabase();
            serverPlayerInformation.LoadPlayerInformationFromDatabase();
            playerDisconnectfromServerEvent += serverPlayerInformation.OnPlayerDisconnect;
            StartServer();
        }
        else
        {
            timer += Time.deltaTime;

            if (timer >= informationSavingFrequencie)
            {
                timer = 0;

                serverPlayerInformation.SavePlayerInformationToDatabase();
            }
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Player p = player.GetComponent<Player>();
        PlayerList.instance.AddPlayer(p);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if (playerDisconnectfromServerEvent != null)
        {
            Player player = PlayerList.instance.GetPlayerWithConnection(conn);

            if (player != null)
            {
                playerDisconnectfromServerEvent(player.deviceUniqueIdentifier);
            }
        }
        base.OnServerDisconnect(conn);
    }

    public void CloseServer()
    {
        StopServer();

        serverGameInformation.ClearGameInformation();
        serverPlayerInformation.ClearPlayerInformation();

        Application.Quit();
    }
}
