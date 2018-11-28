using System;
using System.Collections.Generic;


public class PlayerInformation
{
    Player player;

    int playerCourseId;
    int playerTeamId;
    int playerTeamInformationId;

    string playerDeviceUniqueIdentifier;

    ClientGameState playerGameState;

    public Player Player
    {
        get
        {
            return player;
        }
    }

    public int PlayerCourseId
    {
        get
        {
            return playerCourseId;
        }
    }

    public int PlayerTeamId
    {
        get
        {
            return playerTeamId;
        }
    }

    public int PlayerTeamInformationId
    {
        get
        {
            return playerTeamInformationId;
        }
    }

    public string PlayerDeviceUniqueIdentifier
    {
        get
        {
            return playerDeviceUniqueIdentifier;
        }
    }

    public ClientGameState PlayerGameState
    {
        get
        {
            return playerGameState;
        }
    }

    public Action playerChangedEvent;
    public Action playerTeamIdChangedEvent;
    public Action playerGameStateChangedEvent;

    public PlayerInformation(Player p)
    {
        player = p;
        playerDeviceUniqueIdentifier = player.deviceUniqueIdentifier;

        playerCourseId = player.ServerCourseId;
        playerTeamId = player.ServerTeamId;
        playerTeamInformationId = player.ServerTeamInformationId;
        playerGameState = player.ServerPlayerGameState;

        player.serverCourseIdChangedEvent += OnPlayerCourseIdChanged;
        player.serverTeamIdChangedEvent += OnPlayerTeamIdChanged;
        player.serverTeamInformationIdChangedEvent += OnPlayerTeamInformationIdChanged;
        player.serverPlayerStatusChangedEvent += OnPlayerGameStateChanged;

        player.TargetSetGameState(player.connectionToClient, ClientGameState.Pairing);
    }

    public PlayerInformation(int courseId, int teamId, int teamInformationId, string deviceUniqueIdentifier, ClientGameState gameState)
    {
        player = null;
        playerCourseId = courseId;
        playerTeamId = teamId;
        playerTeamInformationId = teamInformationId;
        playerDeviceUniqueIdentifier = deviceUniqueIdentifier;
        playerGameState = gameState;
    }

    void OnPlayerChanged()
    {

        if (playerChangedEvent != null)
        {
            playerChangedEvent();
        }
    }

    void OnPlayerCourseIdChanged()
    {
        playerCourseId = player.ServerCourseId;
    }

    void OnPlayerTeamIdChanged()
    {
        playerTeamId = player.ServerTeamId;
    }

    void OnPlayerTeamInformationIdChanged()
    {
        playerTeamInformationId = player.ServerTeamInformationId;

        if (playerTeamIdChangedEvent != null)
        {
            playerTeamIdChangedEvent();
        }
    }

    void OnPlayerGameStateChanged()
    {
        playerGameState = player.ServerPlayerGameState;

        if (playerGameStateChangedEvent != null)
        {
            playerGameStateChangedEvent();
        }
    }

    public void OnPlayerDisconnect()
    {
        player = null;

        if (playerGameStateChangedEvent != null)
        {
            playerGameStateChangedEvent();
        }
    }

    public void OnPlayerReconnect(Player p)
    {
        player = p;

        if (player.serverDeviceType == DeviceType.Headset && playerGameState != ClientGameState.Pairing && playerGameState != ClientGameState.NoAssociatedPair)
        {
            player.ServerCourseId = playerCourseId;
            player.ServerTeamId = playerTeamId;
            player.ServerTeamInformationId = playerTeamInformationId;

            player.serverCourseIdChangedEvent += OnPlayerCourseIdChanged;
            player.serverPlayerStatusChangedEvent += OnPlayerGameStateChanged;

            player.TargetSetGameState(player.connectionToClient, ClientGameState.Reconnecting);
            player.TargetSetTeamInformation(player.connectionToClient, playerTeamInformationId);
        }
        else
        {
            player.serverCourseIdChangedEvent += OnPlayerCourseIdChanged;
            player.serverTeamIdChangedEvent += OnPlayerTeamIdChanged;
            player.serverTeamInformationIdChangedEvent += OnPlayerTeamInformationIdChanged;
            player.serverPlayerStatusChangedEvent += OnPlayerGameStateChanged;

            player.TargetSetGameState(player.connectionToClient, ClientGameState.Pairing);
        }

        OnPlayerChanged();
    }
}
