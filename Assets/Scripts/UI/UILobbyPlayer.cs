using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyPlayer : MonoBehaviour
{
    [SerializeField]
    Image image;

    [SerializeField]
    Text nameText;

    [SerializeField]
    Text teamNameText;

    [SerializeField]
    Image color;

    [SerializeField]
    Text statusText;

    int mPlayerId;

    private void OnDestroy()
    {
        PlayerList.instance.GetPlayerWithId(mPlayerId).serverDeviceNameChangedEvent -= UpdateLobbyPlayerName;
        PlayerList.instance.GetPlayerWithId(mPlayerId).serverDeviceNameChangedEvent -= UpdateLobbyPlayerImage;
        PlayerList.instance.GetPlayerWithId(mPlayerId).serverTeamNameChangedEvent -= UpdateLobbyPlayerTeamName;
        PlayerList.instance.GetPlayerWithId(mPlayerId).serverTeamColorChangedEvent -= UpdateLobbyPlayerColor;
        PlayerList.instance.GetPlayerWithId(mPlayerId).serverPlayerStatusChangedEvent -= UpdateLobbyPlayerStatusText;
    }

    public void SetPlayer(int id)
    {
        mPlayerId = id;

        PlayerList.instance.GetPlayerWithId(mPlayerId).serverDeviceNameChangedEvent += UpdateLobbyPlayerName;
        PlayerList.instance.GetPlayerWithId(mPlayerId).serverDeviceNameChangedEvent += UpdateLobbyPlayerImage;
        PlayerList.instance.GetPlayerWithId(mPlayerId).serverTeamNameChangedEvent += UpdateLobbyPlayerTeamName;
        PlayerList.instance.GetPlayerWithId(mPlayerId).serverTeamColorChangedEvent += UpdateLobbyPlayerColor;
        PlayerList.instance.GetPlayerWithId(mPlayerId).serverPlayerStatusChangedEvent += UpdateLobbyPlayerStatusText;

        UpdateLobbyPlayerInformation();
    }

    void UpdateLobbyPlayerInformation()
    {
        UpdateLobbyPlayerImage();
        UpdateLobbyPlayerName();
        UpdateLobbyPlayerTeamName();
        UpdateLobbyPlayerColor();
        UpdateLobbyPlayerStatusText();
    }

    void UpdateLobbyPlayerImage()
    {
        //Should change depending on the device type between a heaset or a tablet
    }

    void UpdateLobbyPlayerName()
    {
        nameText.text = PlayerList.instance.GetPlayerWithId(mPlayerId).ServerDeviceName;
    }

    void UpdateLobbyPlayerTeamName()
    {
        teamNameText.text = PlayerList.instance.GetPlayerWithId(mPlayerId).ServerTeamName;
    }

    void UpdateLobbyPlayerColor()
    {
        color.color = PlayerList.instance.GetPlayerWithId(mPlayerId).ServerTeamColor;
    }

    void UpdateLobbyPlayerStatusText()
    {
        if(PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == GameState.NotReady)
        {
            statusText.text = "Not ready";
        }
        else if(PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == GameState.Pairing)
        {
            statusText.text = "Pairing";
        }
        else if(PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == GameState.NoAssociatedPair)
        {
            statusText.text = "No associated pair";
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == GameState.Paired)
        {
            statusText.text = "Paired";
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == GameState.Ready)
        {
            statusText.text = "Ready";
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == GameState.PlayingTutorial)
        {
            statusText.text = "Playing tutorial";
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == GameState.Playing)
        {
            string text = "Playing";

            if(PlayerList.instance.GetPlayerWithId(mPlayerId).serverAlgorithmId == Constants.SHORTEST_FLIGHT_DISTANCE_ALGORITHM)
            {
                text += " - Shortest Flight";
            }
            else if (PlayerList.instance.GetPlayerWithId(mPlayerId).serverAlgorithmId == Constants.LONGEST_STRAIGHT_ALGORITHM)
            {
                text += " - Longest Straight";
            }
            else if (PlayerList.instance.GetPlayerWithId(mPlayerId).serverAlgorithmId == Constants.STANDARD_ALGORITHM)
            {
                text += " - Standard algorithm";
            }

            statusText.text = text;
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == GameState.WaitingForNextRound)
        {
            statusText.text = "Waiting for next round";
        }
    }
}
