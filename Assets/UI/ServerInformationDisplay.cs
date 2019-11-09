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
        [SerializeField]
        private GameRoundManagerAsset gameRoundManager;

        [SerializeField]
        private LocalizeStringAsset gameRoundString;

        [SerializeField]
        private LocalizeStringAsset gameStateString;

        [SerializeField]
        private LocalizeStringAsset intermissionString;

        [SerializeField]
        private LocalizeStringAsset lobbyString;

        [SerializeField]
        private LocalizeStringAsset playingRoundString;

        [SerializeField]
        private LocalizeStringAsset tutorialString;

        [SerializeField]
        private Text serverGameRoundText;

        [SerializeField]
        private Text serverGameStateText;

        void OnEnable()
        {
            if (Promoscience.Server.Instance != null)
            {
                gameRoundManager.Round.OnValueChangedHandler += OnGameRoundChanged;
                Promoscience.Server.Instance.gameStateChangedEvent += OnGameStateChanged;
            }

            OnGameStateChanged();
        }

        void OnGameRoundChanged(int round)
        {
            serverGameRoundText.text = gameRoundString.Value + " : " + Promoscience.Server.Instance.GameRound;
        }

        void OnGameStateChanged()
        {
            string s = gameStateString.Value + " : ";

            if (Promoscience.Server.Instance.GameState == ServerGameState.Lobby)
            {
                s += lobbyString.Value;
            }
            else if (Promoscience.Server.Instance.GameState == ServerGameState.Tutorial)
            {
                s += tutorialString.Value;
            }
            else if (Promoscience.Server.Instance.GameState == ServerGameState.GameRound)
            {
                s += playingRoundString.Value;
            }
            else if (Promoscience.Server.Instance.GameState == ServerGameState.Intermission)
            {
                s += intermissionString.Value;
            }

            serverGameStateText.text = s;
        }
    }
}
