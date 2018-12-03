using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerInformationDisplay : MonoBehaviour
{
    [SerializeField]
    ScriptableServerGameInformation serverGameInformation;

    [SerializeField]
    ScriptableLocalizeString gameRound;

    [SerializeField]
    ScriptableLocalizeString gameState;

    [SerializeField]
    ScriptableLocalizeString intermission;

    [SerializeField]
    ScriptableLocalizeString lobby;

    [SerializeField]
    ScriptableLocalizeString playingRound;

    [SerializeField]
    ScriptableLocalizeString tutorial;

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
        serverGameRoundText.text = gameRound.Value + " : " + serverGameInformation.GameRound;
    }

    void OnGameStateChanged()
    {
        string s = gameState.Value + " : ";

        if(serverGameInformation.GameState == ServerGameState.Lobby)
        {
            s += lobby.Value;
        }
        else if (serverGameInformation.GameState == ServerGameState.Tutorial)
        {
            s += tutorial.Value;
        }
        else if (serverGameInformation.GameState == ServerGameState.GameRound)
        {
            s += playingRound.Value;
        }
        else if (serverGameInformation.GameState == ServerGameState.Intermission)
        {
            s += intermission.Value;
        }

        serverGameStateText.text = s;
    }
}
