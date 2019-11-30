using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.UI
{

    public class ServerInformationDisplay : MonoBehaviour
    {
        //[SerializeField]
        //private GameRoundManagerAsset gameRoundManager;

        [SerializeField]
        private LocalizeString gameRoundString = new LocalizeString("Round");

        [SerializeField]
        private LocalizeString gameStateString = new LocalizeString("Game State");

        [SerializeField]
        private LocalizeString intermissionString = new LocalizeString("Intermission");

        [SerializeField]
        private LocalizeString lobbyString = new LocalizeString("Lobby");

        [SerializeField]
        private LocalizeString playingString = new LocalizeString("Playing");

        [SerializeField]
        private LocalizeString tutorialString = new LocalizeString("Tutorial");

        [SerializeField]
        private Text serverGameRoundText;

        [SerializeField]
        private Text serverGameStateText;

        void OnEnable()
        {
            if (Server.Instance != null)
            {
                //gameRoundManager.Round.OnValueChangedHandler += OnGameRoundChanged;
                Server.Instance.State.OnValueChangedHandler += OnServerStateChanged;
            }

            OnServerStateChanged(Server.Instance.State.Value);
        }

        void OnGameRoundChanged(int round)
        {
            serverGameRoundText.text = gameRoundString.Value + " : " + GameManager.Instance.CurrentGame.Round.Value;
        }

        void OnServerStateChanged(ServerState state)
        {
            string s = gameStateString.Value + " : ";

            if (Server.Instance.State.Value == ServerState.Lobby)
            {
                s += lobbyString.Value;
            }
            else if (Server.Instance.State.Value == ServerState.Quickplay)
            {
                s += tutorialString.Value;
            }
            else if (Server.Instance.State.Value == ServerState.Round)
            {
                s += playingString.Value;
            }
            else if (Server.Instance.State.Value == ServerState.Intermission)
            {
                s += intermissionString.Value;
            }

            serverGameStateText.text = s;
        }
    }
}
