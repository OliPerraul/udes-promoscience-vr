using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UnityEngine;

namespace UdeS.Promoscience.Networking
{
    public class Lobby : MonoBehaviour
    {
        private bool enabled = false;

        [SerializeField]
        ScriptableServerGameInformation serverGameState;

        public void Awake()
        {
            serverGameState.gameStateChangedEvent += OnGameStateChanged;
        }

        public void OnGameStateChanged()
        {
            //if (serverGameState.GameState == ServerGameState.ViewingPlayback)
            //{
            //    Enabled = false;
            //}
        }

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
                transform.GetChild(0).gameObject.SetActive(enabled);
            }
        }
    }
}
