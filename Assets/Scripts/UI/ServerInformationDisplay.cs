using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerInformationDisplay : MonoBehaviour
{
    [SerializeField]
    ScriptableServerGameInformation serverGameInformation;

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

	void Start ()
    {
        serverGameInformation.gameRoundChangedEvent += OnGameRoundChanged;
        serverGameInformation.gameStateChangedEvent += OnGameStateChanged;
        OnGameRoundChanged();
        OnGameStateChanged();

    }
	
    void OnGameRoundChanged()
    {
        serverGameRoundText.text = gameRoundString.Value + " : " + serverGameInformation.GameRound;
    }

    void OnGameStateChanged()
    {
        string s = gameStateString.Value + " : ";

        if(serverGameInformation.GameState == ServerGameState.Lobby)
        {
            s += lobbyString.Value;
        }
        else if (serverGameInformation.GameState == ServerGameState.Tutorial)
        {
            s += tutorialString.Value;
        }
        else if (serverGameInformation.GameState == ServerGameState.GameRound)
        {
            s += playingRoundString.Value;
        }
        else if (serverGameInformation.GameState == ServerGameState.Intermission)
        {
            s += intermissionString.Value;
        }

        serverGameStateText.text = s;
    }
}
