using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyPlayer : MonoBehaviour
{
    int playerId;

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

    private void OnDestroy()
    {
        PlayerList.instance.GetPlayerWithId(playerId).sDeviceNameChangedEvent -= UpdateLobbyPlayerName;
        PlayerList.instance.GetPlayerWithId(playerId).sDeviceNameChangedEvent -= UpdateLobbyPlayerImage;
        PlayerList.instance.GetPlayerWithId(playerId).sTeamNameChangedEvent -= UpdateLobbyPlayerTeamName;
        PlayerList.instance.GetPlayerWithId(playerId).sTeamColorChangedEvent -= UpdateLobbyPlayerColor;
        PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatusChangedEvent -= UpdateLobbyPlayerStatusText;
    }

    public void SetPlayer(int id)
    {
        playerId = id;

        PlayerList.instance.GetPlayerWithId(playerId).sDeviceNameChangedEvent += UpdateLobbyPlayerName;
        PlayerList.instance.GetPlayerWithId(playerId).sDeviceNameChangedEvent += UpdateLobbyPlayerImage;
        PlayerList.instance.GetPlayerWithId(playerId).sTeamNameChangedEvent += UpdateLobbyPlayerTeamName;
        PlayerList.instance.GetPlayerWithId(playerId).sTeamColorChangedEvent += UpdateLobbyPlayerColor;
        PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatusChangedEvent += UpdateLobbyPlayerStatusText;

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
        nameText.text = PlayerList.instance.GetPlayerWithId(playerId).sDeviceName;
    }

    void UpdateLobbyPlayerTeamName()
    {
        teamNameText.text = PlayerList.instance.GetPlayerWithId(playerId).sTeamName;
    }

    void UpdateLobbyPlayerColor()
    {
        color.color = PlayerList.instance.GetPlayerWithId(playerId).sTeamColor;
    }

    void UpdateLobbyPlayerStatusText()
    {
        if(PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatus == Constants.NOT_READY)
        {
            statusText.text = "Not ready";
        }
        else if(PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatus == Constants.PAIRING)
        {
            statusText.text = "Pairing";
        }
        else if(PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatus == Constants.NO_ASSOCIATED_PAIR)
        {
            statusText.text = "No associated pair";
        }
        else if (PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatus == Constants.PAIRED)
        {
            statusText.text = "Paired";
        }
        else if (PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatus == Constants.READY)
        {
            statusText.text = "Ready";
        }
        else if (PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatus == Constants.PLAYING_TUTORIAL)
        {
            statusText.text = "Playing tutorial";
        }
        else if (PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatus == Constants.PLAYING)
        {
            statusText.text = "Playing";
        }
        else if (PlayerList.instance.GetPlayerWithId(playerId).sPlayerStatus == Constants.WAITING_FOR_NEXT_ROUND)
        {
            statusText.text = "Waiting for next round";
        }
    }
}
