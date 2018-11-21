using System;
using System.Collections.Generic;


public class PlayerInformation
{
    public int playerCourseId;
    public int playerId;//Should be changed for reerance to player instead to avoid error when a player from to of the list is removed id should change
    public int playerTeamId;
    public int playerTeamInformationId;

    public string playerDeviceUniqueIdentifier;

    public ClientGameState playerGameState;

    public Action playerTeamIdChangedEvent;
    public Action playerGameStateChangedEvent;

    public PlayerInformation(int id, string deviceUniqueIdentifier)
    {
        playerId = id;
        playerDeviceUniqueIdentifier = deviceUniqueIdentifier;

        Player player = PlayerList.instance.GetPlayerWithId(id);

        playerCourseId = player.ServerCourseId;
        playerTeamId = player.ServerTeamId;
        playerTeamInformationId = player.ServerTeamInformationId;
        playerGameState = player.ServerPlayerGameState;

        player.serverCourseIdChangedEvent += OnPlayerCourseIdChanged;
        player.serverTeamIdChangedEvent += OnPlayerTeamIdChanged;//Should change only once if not disconnected before it does
        player.serverTeamInformationIdChangedEvent += OnPlayerTeamInformationIdChanged;//Should change only once if not disconnected before it does
        player.serverPlayerStatusChangedEvent += OnPlayerGameStateChanged;

        player.TargetSetGameState(player.connectionToClient, ClientGameState.Pairing);
    }

    void OnPlayerCourseIdChanged()
    {
        Player player = PlayerList.instance.GetPlayerWithId(playerId);
        playerCourseId = player.ServerCourseId;
    }

    void OnPlayerTeamIdChanged()
    {
        Player player = PlayerList.instance.GetPlayerWithId(playerId);
        playerTeamId = player.ServerTeamId;
    }

    void OnPlayerTeamInformationIdChanged()
    {
        Player player = PlayerList.instance.GetPlayerWithId(playerId);
        playerTeamInformationId = player.ServerTeamInformationId;

        if (playerTeamIdChangedEvent != null)
        {
            playerTeamIdChangedEvent();
        }
    }

    void OnPlayerGameStateChanged()
    {
        Player player = PlayerList.instance.GetPlayerWithId(playerId);
        playerGameState = player.ServerPlayerGameState;

        if (playerGameStateChangedEvent != null)
        {
            playerGameStateChangedEvent();
        }
    }

    public void OnPlayerDisconnect()
    {
        playerId = -1;

        if (playerGameStateChangedEvent != null)
        {
            playerGameStateChangedEvent();
        }
    }

    public void OnPlayerReconnect(int id)
    {
        playerId = id;

        Player player = PlayerList.instance.GetPlayerWithId(id);

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
    }
}
