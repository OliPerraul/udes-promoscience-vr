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
            playersInformation.orderChangedEvent -= OnOrderChanged;
        }
    }

    public void SetId(int id)
    {
        slotId = id;

        playersInformation.orderChangedEvent += OnOrderChanged;

        SetPlayer();
    }

    void OnOrderChanged()
    {
        if (playerInformation != null)
        {
            playerInformation.playerChangedEvent -= SetPlayer;
            playerInformation.playerTeamIdChangedEvent -= UpdateLobbyPlayerTeam;
            playerInformation.playerGameStateChangedEvent -= UpdateLobbyPlayerStatusText;
        }

        SetPlayer();
    }

    void SetPlayer()
    {
        playerInformation = playersInformation.GetPlayerInformationWithId(slotId);

        playerInformation.playerChangedEvent += SetPlayer;
        playerInformation.playerTeamIdChangedEvent += UpdateLobbyPlayerTeam;
        playerInformation.playerGameStateChangedEvent += UpdateLobbyPlayerStatusText;

        SetLobbyPlayerImage();
        SetLobbyPlayerName();
        UpdateLobbyPlayerTeam();
        UpdateLobbyPlayerStatusText();
    }

    void SetLobbyPlayerImage()
    {
        if (playerInformation.Player != null)
        {
            //Should change depending on the device type between a heaset or a tablet
        }
    }

    void SetLobbyPlayerName()
    {
        if (playerInformation.Player != null)
        {
            nameText.text = playerInformation.Player.ServerDeviceName;
        }
    }

    void UpdateLobbyPlayerTeam()
    {
        if (playerInformation.PlayerTeamInformationId != -1)
        {
            ScriptableTeam team = teamList.GetScriptableTeamWithId(playerInformation.PlayerTeamInformationId);
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
        if (playerInformation.Player == null)
        {
            statusText.text = "Disconnected";
        }
        else
        {
            if (playerInformation.PlayerGameState == ClientGameState.Connecting)
            {
                statusText.text = "Connecting";
            }
            else if (playerInformation.PlayerGameState == ClientGameState.NotReady)
            {
                statusText.text = "Not ready";
            }
            else if (playerInformation.PlayerGameState == ClientGameState.Pairing)
            {
                statusText.text = "Pairing";
            }
            else if (playerInformation.PlayerGameState == ClientGameState.NoAssociatedPair || playerInformation.PlayerGameState == ClientGameState.ReconnectingNoAssociatedPair)
            {
                statusText.text = "No associated pair";
            }
            else if (playerInformation.PlayerGameState == ClientGameState.Paired)
            {
                statusText.text = "Paired";
            }
            else if (playerInformation.PlayerGameState == ClientGameState.Ready)
            {
                statusText.text = "Ready";
            }
            else if (playerInformation.PlayerGameState == ClientGameState.PlayingTutorial)
            {
                statusText.text = "Playing tutorial";
            }
            else if (playerInformation.PlayerGameState == ClientGameState.Playing)
            {
                string text = "Playing";

                if (playerInformation.Player.serverDeviceType == DeviceType.Headset)
                {
                    if (playerInformation.Player.serverAlgorithm == Algorithm.ShortestFlightDistance)
                    {
                        text += " - Shortest Flight";
                    }
                    else if (playerInformation.Player.serverAlgorithm == Algorithm.LongestStraight)
                    {
                        text += " - Longest Straight";
                    }
                    else if (playerInformation.Player.serverAlgorithm == Algorithm.Standard)
                    {
                        text += " - Standard algorithm";
                    }
                }

                statusText.text = text;
            }
            else if (playerInformation.PlayerGameState == ClientGameState.WaitingForNextRound)
            {
                statusText.text = "Waiting for next round";
            }
            else if (playerInformation.PlayerGameState == ClientGameState.Reconnecting)
            {
                statusText.text = "Reconnecting";
            }
            else if (playerInformation.PlayerGameState == ClientGameState.WaitingForPairConnection)
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
