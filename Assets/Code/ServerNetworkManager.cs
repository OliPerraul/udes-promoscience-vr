using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkManager : NetworkManager
{
    //List of player script might be changed to scriptable object for UI access
    List<Player> players;
    List<GameObject> playersGameObject;

    [SerializeField]
    GameObject playerLobbyPrefab;

    [SerializeField]
    GameObject playerLobbyList;

    // Use this for initialization
    void Start ()
    {
        Init();
        StartServer();
	}

    private void Init()
    {
        players = new List<Player>();
        playersGameObject = new List<GameObject>();
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        playersGameObject.Add(player);
        players.Add(player.GetComponent<Player>());
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

        //temp, should be handle by UI
        GameObject playerLobby = Instantiate(playerLobbyPrefab, new Vector3(0, 0, 0), Quaternion.identity, playerLobbyList.transform);

        //Temp 
        Debug.Log(conn.address);
    }

}
