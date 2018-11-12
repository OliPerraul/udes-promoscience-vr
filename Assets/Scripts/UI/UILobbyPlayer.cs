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
        if (PlayerList.instance.GetPlayerWithId(mPlayerId) != null)
        {
            PlayerList.instance.GetPlayerWithId(mPlayerId).serverDeviceNameChangedEvent -= UpdateLobbyPlayerName;
            PlayerList.instance.GetPlayerWithId(mPlayerId).serverDeviceNameChangedEvent -= UpdateLobbyPlayerImage;
            PlayerList.instance.GetPlayerWithId(mPlayerId).serverTeamNameChangedEvent -= UpdateLobbyPlayerTeamName;
            PlayerList.instance.GetPlayerWithId(mPlayerId).serverTeamColorChangedEvent -= UpdateLobbyPlayerColor;
            PlayerList.instance.GetPlayerWithId(mPlayerId).serverPlayerStatusChangedEvent -= UpdateLobbyPlayerStatusText;
        }
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
        if(PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == ClientGameState.NotReady)
        {
            statusText.text = "Not ready";
        }
        else if(PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == ClientGameState.Pairing)
        {
            statusText.text = "Pairing";
        }
        else if(PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == ClientGameState.NoAssociatedPair)
        {
            statusText.text = "No associated pair";
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == ClientGameState.Paired)
        {
            statusText.text = "Paired";
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == ClientGameState.Ready)
        {
            statusText.text = "Ready";
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == ClientGameState.PlayingTutorial)
        {
            statusText.text = "Playing tutorial";
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == ClientGameState.Playing)
        {
            string text = "Playing";

            if(PlayerList.instance.GetPlayerWithId(mPlayerId).serverAlgorithm == Algorithm.ShortestFlightDistance)
            {
                text += " - Shortest Flight";
            }
            else if (PlayerList.instance.GetPlayerWithId(mPlayerId).serverAlgorithm == Algorithm.LongestStraight)
            {
                text += " - Longest Straight";
            }
            else if (PlayerList.instance.GetPlayerWithId(mPlayerId).serverAlgorithm == Algorithm.Standard)
            {
                text += " - Standard algorithm";
            }

            statusText.text = text;
        }
        else if (PlayerList.instance.GetPlayerWithId(mPlayerId).ServerPlayerGameState == ClientGameState.WaitingForNextRound)
        {
            statusText.text = "Waiting for next round";
        }
    }
}
