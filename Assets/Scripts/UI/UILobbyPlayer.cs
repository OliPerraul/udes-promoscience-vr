using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILobbyPlayer : MonoBehaviour
{
    public int playerId;

    Player player;

    [SerializeField]
    ScriptablePlayerList players;

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
        //UnregisterToAllEvents
    }

    public void SetPlayerId(int id)
    {
        playerId = id;
        //registers to events
        UpdateLobbyPlayerInformation();
    }

    void UpdateLobbyPlayerInformation()
    {
        Player player = players.GetPlayerWithId(playerId);

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
        nameText.text = player.deviceName;
    }

    void UpdateLobbyPlayerTeamName()
    {
        teamNameText.text = player.teamName;
    }

    void UpdateLobbyPlayerColor()
    {
        color.color = player.teamColor;
    }

    void UpdateLobbyPlayerStatusText()
    {
        if(player.playerStatus == 0)
        {
            statusText.text = "Not ready";
        }
        else if (player.playerStatus == 0)
        {
            statusText.text = "Ready!";
        }
    }
}
