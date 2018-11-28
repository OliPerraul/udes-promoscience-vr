using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerInformationDisplay : MonoBehaviour
{
    [SerializeField]
    ScriptableServerGameInformation serverGameInformation;

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
        serverGameRoundText.text = "Game Round : " + serverGameInformation.GameRound;
    }

    void OnGameStateChanged()
    {
        string s = "Game State : ";

        if(serverGameInformation.GameState == ServerGameState.Lobby)
        {
            s += "Lobby";
        }
        else if (serverGameInformation.GameState == ServerGameState.Tutorial)
        {
            s += "Tutorial";
        }
        else if (serverGameInformation.GameState == ServerGameState.GameRound)
        {
            s += "Playing round";
        }
        else if (serverGameInformation.GameState == ServerGameState.Intermission)
        {
            s += "Intermission";
        }

        serverGameStateText.text = s;
    }
}
