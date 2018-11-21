using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyPlayer : MonoBehaviour
{
    [SerializeField]
    ScriptableServerPlayerInformation playersInformation;

    [SerializeField]
    ScriptableTeamList teamList;

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

    PlayerInformation playerInformation;

    int slotId;

    private void OnDestroy()
    {
        if (playerInformation != null)
        {
            playersInformation.orderChangedEvent -= ChangePlayerId;
        }
    }

    public void SetId(int id)
    {
        slotId = id;

        playersInformation.orderChangedEvent += ChangePlayerId;

        ChangePlayerId();
    }

    void ChangePlayerId()
    {
        if(playerInformation != null)
        {
            playerInformation.playerTeamIdChangedEvent -= UpdateLobbyPlayerTeam;
            playerInformation.playerGameStateChangedEvent -= UpdateLobbyPlayerStatusText;
        }

        playerInformation = playersInformation.GetPlayerInformationWithId(slotId);

        playerInformation.playerTeamIdChangedEvent += UpdateLobbyPlayerTeam;
        playerInformation.playerGameStateChangedEvent += UpdateLobbyPlayerStatusText;

        SetLobbyPlayerImage();
        SetLobbyPlayerName();
        UpdateLobbyPlayerTeam();
        UpdateLobbyPlayerStatusText();
    }

    void SetLobbyPlayerImage()
    {
        //Should change depending on the device type between a heaset or a tablet
    }

    void SetLobbyPlayerName()
    {
        nameText.text = PlayerList.instance.GetPlayerWithId(playerInformation.playerId).ServerDeviceName;
    }

    void UpdateLobbyPlayerTeam()
    {
        if (playerInformation.playerTeamInformationId != -1)
        {
            ScriptableTeam team = teamList.GetScriptableTeamWithId(playerInformation.playerTeamInformationId);
            teamNameText.text = team.TeamName;
            color.color = team.TeamColor;
        }
        else
        {
            teamNameText.text = "";
            color.color = Color.white;
        }
    }

    void UpdateLobbyPlayerStatusText()
    {
        if (playerInformation.playerId == -1)
        {
            statusText.text = "Disconnected";
        }
        else
        {
            if (playerInformation.playerGameState == ClientGameState.Connecting)
            {
                statusText.text = "Connecting";
            }
            else if (playerInformation.playerGameState == ClientGameState.NotReady)
            {
                statusText.text = "Not ready";
            }
            else if (playerInformation.playerGameState == ClientGameState.Pairing)
            {
                statusText.text = "Pairing";
            }
            else if (playerInformation.playerGameState == ClientGameState.NoAssociatedPair || playerInformation.playerGameState == ClientGameState.ReconnectingNoAssociatedPair)
            {
                statusText.text = "No associated pair";
            }
            else if (playerInformation.playerGameState == ClientGameState.Paired)
            {
                statusText.text = "Paired";
            }
            else if (playerInformation.playerGameState == ClientGameState.Ready)
            {
                statusText.text = "Ready";
            }
            else if (playerInformation.playerGameState == ClientGameState.PlayingTutorial)
            {
                statusText.text = "Playing tutorial";
            }
            else if (playerInformation.playerGameState == ClientGameState.Playing)
            {
                string text = "Playing";

                if (PlayerList.instance.GetPlayerWithId(slotId).serverAlgorithm == Algorithm.ShortestFlightDistance)
                {
                    text += " - Shortest Flight";
                }
                else if (PlayerList.instance.GetPlayerWithId(slotId).serverAlgorithm == Algorithm.LongestStraight)
                {
                    text += " - Longest Straight";
                }
                else if (PlayerList.instance.GetPlayerWithId(slotId).serverAlgorithm == Algorithm.Standard)
                {
                    text += " - Standard algorithm";
                }

                statusText.text = text;
            }
            else if (playerInformation.playerGameState == ClientGameState.WaitingForNextRound)
            {
                statusText.text = "Waiting for next round";
            }
            else if (playerInformation.playerGameState == ClientGameState.Reconnecting)
            {
                statusText.text = "Reconnecting";
            }
            else if (playerInformation.playerGameState == ClientGameState.WaitingForPairConnection)
            {
                statusText.text = "Waiting for pair connection";
            }
            else
            {
                statusText.text = "Unknown Status";
            }
        }
    }
}
