using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyPlayerList : MonoBehaviour
{
    [SerializeField]
    ScriptablePlayerList players;

    [SerializeField]
    List<GameObject> lobbyPlayers;

    [SerializeField]
    GameObject lobbyPlayerPrefab;

    // Use this for initialization
    void Start ()
    {
        lobbyPlayers = new List<GameObject>();
        //RegisterToOnRemovePlayerEvent += 
        //registerToOnAddPlayerEvent += AddLobbyPlayer();
    }

    void AddLobbyPlayer()
    {
        GameObject playerLobby = Instantiate(lobbyPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        playerLobby.GetComponent<UILobbyPlayer>().SetPlayerId(lobbyPlayers.Count);
        lobbyPlayers.Add(playerLobby);
    }

    void RemoveLobbyPlayer(int id)
    {
        Destroy(lobbyPlayers[id]);
        lobbyPlayers.RemoveAt(id);
    }
}
