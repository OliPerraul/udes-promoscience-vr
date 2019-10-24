using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience.UI
{

    public class ServerInformationDisplay : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableServerGameInformation serverGameInformation;

        [SerializeField]
        ScriptableLocalizeString gameRoundString;

        [SerializeField]
        ScriptableLocalizeString gameStateString;

        [SerializeField]
        ScriptableLocalizeString intermissionString;

        [SerializeField]
        ScriptableLocalizeString lobbyString;

        [SerializeField]
        ScriptableLocalizeString playingRoundString;

        [SerializeField]
        ScriptableLocalizeString tutorialString;

        [SerializeField]
        Text serverGameRoundText;

        [SerializeField]
        Text serverGameStateText;

        void OnEnable()
        {
            ServerGame.Instance.gameRoundChangedEvent += OnGameRoundChanged;
            ServerGame.Instance.gameStateChangedEvent += OnGameStateChanged;
            OnGameRoundChanged();
            OnGameStateChanged();

        }

        void OnGameRoundChanged()
        {
            serverGameRoundText.text = gameRoundString.Value + " : " + ServerGame.Instance.GameRound;
        }

        void OnGameStateChanged()
        {
            string s = gameStateString.Value + " : ";

            if (ServerGame.Instance.GameState == ServerGameState.Lobby)
            {
                s += lobbyString.Value;
            }
            else if (ServerGame.Instance.GameState == ServerGameState.Tutorial)
            {
                s += tutorialString.Value;
            }
            else if (ServerGame.Instance.GameState == ServerGameState.GameRound)
            {
                s += playingRoundString.Value;
            }
            else if (ServerGame.Instance.GameState == ServerGameState.Intermission)
            {
                s += intermissionString.Value;
            }

            serverGameStateText.text = s;
        }
    }
}
