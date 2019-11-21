using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Network.UI
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField]
        private GameObject target;

        public void Awake()
        {
            Server.Instance.gameStateChangedEvent += OnServerGameStateChanged;
        }

        public bool Enabled
        {
            set
            {
                target?.gameObject?.SetActive(value);
            }
        }

        public void OnServerGameStateChanged()
        {
            switch (Server.Instance.GameState)
            {
                case ServerGameState.GameRound:
                case ServerGameState.Lobby:
                case ServerGameState.Tutorial:
                case ServerGameState.Intermission:
                    Enabled = true;
                    break;

                default:
                    Enabled = false;
                    break;
            }
        }
    }
}