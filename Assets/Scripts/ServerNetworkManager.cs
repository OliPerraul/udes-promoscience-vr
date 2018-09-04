using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkManager : NetworkManager
{
    [SerializeField]
    ScriptablePlayerList players;

    [SerializeField]
    GameObject playerLobbyPrefab;

    [SerializeField]
    GameObject playerLobbyList;

    // Use this for initialization
    void Start ()
    {
        StartServer();
	}

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        players.AddPlayer(player.GetComponent<Player>());
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        //temp, should be handle by UI
        GameObject playerLobby = Instantiate(playerLobbyPrefab, new Vector3(0, 0, 0), Quaternion.identity, playerLobbyList.transform);

        //Temp 
        Debug.Log(conn.address);
    }

}
