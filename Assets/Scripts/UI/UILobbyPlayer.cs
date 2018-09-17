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
        PlayerList.instance.GetPlayerWithId(playerId).deviceNameChangedEvent -= UpdateLobbyPlayerName;
        PlayerList.instance.GetPlayerWithId(playerId).deviceNameChangedEvent -= UpdateLobbyPlayerImage;
        PlayerList.instance.GetPlayerWithId(playerId).teamNameChangedEvent -= UpdateLobbyPlayerTeamName;
        PlayerList.instance.GetPlayerWithId(playerId).teamColorChangedEvent -= UpdateLobbyPlayerColor;
        PlayerList.instance.GetPlayerWithId(playerId).playerStatusChangedEvent -= UpdateLobbyPlayerStatusText;
    }

    public void SetPlayer(int id)
    {
        playerId = id;

        PlayerList.instance.GetPlayerWithId(playerId).deviceNameChangedEvent += UpdateLobbyPlayerName;
        PlayerList.instance.GetPlayerWithId(playerId).deviceNameChangedEvent += UpdateLobbyPlayerImage;
        PlayerList.instance.GetPlayerWithId(playerId).teamNameChangedEvent += UpdateLobbyPlayerTeamName;
        PlayerList.instance.GetPlayerWithId(playerId).teamColorChangedEvent += UpdateLobbyPlayerColor;
        PlayerList.instance.GetPlayerWithId(playerId).playerStatusChangedEvent += UpdateLobbyPlayerStatusText;

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
        nameText.text = PlayerList.instance.GetPlayerWithId(playerId).deviceName;
    }

    void UpdateLobbyPlayerTeamName()
    {
        teamNameText.text = PlayerList.instance.GetPlayerWithId(playerId).teamName;
    }

    void UpdateLobbyPlayerColor()
    {
        color.color = PlayerList.instance.GetPlayerWithId(playerId).teamColor;
    }

    void UpdateLobbyPlayerStatusText()
    {
        if(PlayerList.instance.GetPlayerWithId(playerId).playerStatus == Constants.NOT_READY)
        {
            statusText.text = "Not ready";
        }
        else if(PlayerList.instance.GetPlayerWithId(playerId).playerStatus == Constants.PAIRING)
        {
            statusText.text = "Pairing";
        }
        else if(PlayerList.instance.GetPlayerWithId(playerId).playerStatus == Constants.NO_ASSOCIATED_PAIR)
        {
            statusText.text = "No associated pair";
        }
        else if (PlayerList.instance.GetPlayerWithId(playerId).playerStatus == Constants.PAIRED)
        {
            statusText.text = "Paired";
        }
        else if (PlayerList.instance.GetPlayerWithId(playerId).playerStatus == Constants.READY)
        {
            statusText.text = "Ready!";
        }
    }
}
