using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkManager : NetworkManager
{
    public static ServerNetworkManager instance;

    void Start ()
    {
        if (instance == null)
        {
            instance = this;
            StartServer();
        }
        else
        {
            Destroy(this);
        }
	}

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Player p = player.GetComponent<Player>();
        PlayerList.instance.AddPlayer(p);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
