using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.UI
{

    public class UILobbyPlayerList : MonoBehaviour
    {
        [SerializeField]
        ScriptableServerPlayerInformation playerInfomation;

        [SerializeField]
        Stack<GameObject> lobbyPlayers;

        [SerializeField]
        GameObject lobbyPlayerPrefab;

        void OnEnable()
        {
            lobbyPlayers = new Stack<GameObject>();
            playerInfomation.addPlayerEvent += AddLobbyPlayer;
            playerInfomation.removePlayerEvent += RemoveLobbyPlayer;
        }

        private void OnDestroy()
        {
            playerInfomation.addPlayerEvent -= AddLobbyPlayer;
            playerInfomation.removePlayerEvent -= RemoveLobbyPlayer;
        }

        void AddLobbyPlayer()
        {
            GameObject playerLobby = Instantiate(lobbyPlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity, gameObject.transform);
            playerLobby.GetComponent<UILobbyPlayer>().SetId(lobbyPlayers.Count);
            lobbyPlayers.Push(playerLobby);
        }

        void RemoveLobbyPlayer()
        {
            Destroy(lobbyPlayers.Pop());
        }

        void RemoveAllLobbyPlayer()
        {
            while (lobbyPlayers.Count > 0)
            {
                Destroy(lobbyPlayers.Pop());
            }
        }
    }
}
