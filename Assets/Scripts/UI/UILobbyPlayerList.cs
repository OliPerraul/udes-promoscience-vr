using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILobbyPlayerList : MonoBehaviour
{
    [SerializeField]
    List<GameObject> lobbyPlayers;

    [SerializeField]
    GameObject lobbyPlayerPrefab;

    // Use this for initialization
    void Start ()
    {
        lobbyPlayers = new List<GameObject>();
        PlayerList.instance.addPlayerEvent += AddLobbyPlayer;
        PlayerList.instance.removePlayerEvent += RemoveLobbyPlayer;
    }

    private void OnDestroy()
    {
        PlayerList.instance.addPlayerEvent -= AddLobbyPlayer;
        PlayerList.instance.removePlayerEvent -= RemoveLobbyPlayer;
    }

    void AddLobbyPlayer()
    {
        GameObject playerLobby = Instantiate(lobbyPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
        playerLobby.GetComponent<UILobbyPlayer>().SetPlayer(PlayerList.instance.GetLastPlayerId());
        lobbyPlayers.Add(playerLobby);
    }

    void RemoveLobbyPlayer(int id)
    {
        Destroy(lobbyPlayers[id]);
        lobbyPlayers.RemoveAt(id);
    }
}
