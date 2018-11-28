using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public string deviceUniqueIdentifier = "";
    string deviceName = "";

    private void OnDestroy()
    {
        if (isServer)
        {
            PlayerList.instance.RemovePlayer(this);
        }

        if (isClient)
        {
            gameState.valueChangedEvent -= SendCmdPlayerGameState;

            if (deviceType.Value == DeviceType.Headset)
            {
                action.valueChangedEvent -= SendCmdPlayerAction;
            }
            else if (deviceType.Value == DeviceType.Tablet)
            {
                playerInformation.playerInformationChangedEvent -= SendCmdPlayerInformation;
            }
        }
    }

    #region Server

    ClientGameState serverPlayerGameState = 0;
    GameAction serverPlayerAction;

    int serverCourseId = -1;
    int serverTeamId = -1;
    int serverTeamInformationId = -1;

    public DeviceType serverDeviceType = DeviceType.NoType;
    public Algorithm serverAlgorithm;

    public int serverLabyrinthId;

    public int ServerCourseId
    {
        get
        {
            return serverCourseId;
        }
        set
        {
            serverCourseId = value;
            OnCourseIdChanged();
        }
    }

    public string ServerDeviceName
    {
        get
        {
            return deviceName;
        }
        set
        {
            deviceName = value;
            OnDeviceNameChanged();
        }
    }

    public int ServerTeamId
    {
        get
        {
            return serverTeamId;
        }
        set
        {
            serverTeamId = value;
            OnTeamIdChanged();
        }
    }

    public int ServerTeamInformationId
    {
        get
        {
            return serverTeamInformationId;
        }
        set
        {
            serverTeamInformationId = value;
            OnTeamInformationIdChanged();
        }
    }

    public ClientGameState ServerPlayerGameState
    {
        get
        {
            return serverPlayerGameState;
        }
        set
        {
            serverPlayerGameState = value;
            OnPlayerStatusChanged();
        }
    }

    public GameAction ServerPlayerAction
    {
        get
        {
            return serverPlayerAction;
        }
        set
        {
            serverPlayerAction = value;
            serverPlayerActionChangedEvent();
        }
    }

    public Action serverCourseIdChangedEvent;
    public Action serverDeviceNameChangedEvent;
    public Action serverTeamIdChangedEvent;
    public Action serverTeamInformationIdChangedEvent;
    public Action serverPlayerStatusChangedEvent;
    public Action serverPlayerActionChangedEvent;

    void OnCourseIdChanged()
    {
        if (serverCourseIdChangedEvent != null)
        {
            serverCourseIdChangedEvent();
        }
    }

    void OnDeviceNameChanged()
    {
        if (serverDeviceNameChangedEvent != null)
        {
            serverDeviceNameChangedEvent();
        }
    }

    void OnTeamIdChanged()
    {
        if (serverTeamIdChangedEvent != null)
        {
            serverTeamIdChangedEvent();
        }
    }

    void OnTeamInformationIdChanged()
    {
        if (serverTeamInformationIdChangedEvent != null)
        {
            serverTeamInformationIdChangedEvent();
        }
    }

    void OnPlayerStatusChanged()
    {
        if (serverPlayerStatusChangedEvent != null)
        {
            serverPlayerStatusChangedEvent();
        }
    }

    #endregion

    #region Client

    [SerializeField]
    ScriptableGameAction action;

    [SerializeField]
    ScriptableAlgorithm algorithm;

    [SerializeField]
    ScriptableDeviceType deviceType;

    [SerializeField]
    ScriptableClientGameState gameState;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    [SerializeField]
    ScriptableString pairedIpAdress;

    [SerializeField]
    ScriptablePlayerInformation playerInformation;

    [SerializeField]
    ScriptableIntegerArray playerStartSteps;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        gameState.valueChangedEvent += SendCmdPlayerGameState;

        ClientInitialize();
    }

    [Client]
    void ClientInitialize()
    {
        deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
        ServerDeviceName = SystemInfo.deviceModel;

        if (deviceType.Value == DeviceType.Tablet)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            playerInformation.playerInformationChangedEvent += SendCmdPlayerInformation;
        }
        else if(deviceType.Value == DeviceType.Headset)
        {
            action.valueChangedEvent += SendCmdPlayerAction;
        }

        CmdSetDeviceType(deviceType.Value);
        CmdSetDeviceName(ServerDeviceName);
        CmdSetUniqueIdentifier(deviceUniqueIdentifier);

        gameState.Value = ClientGameState.Connecting;
    }

    [Client]
    void SendCmdPlayerAction()
    {
        if (gameState.Value == ClientGameState.Playing)
        {
            CmdSetPlayerAction(action.Value);
        }
    }

    [Client]
    void SendCmdPlayerGameState()
    {
        CmdSetPlayerGameState(gameState.Value);
    }

    [Client]
    void SendCmdPlayerInformation()
    {
        CmdSetPlayerInformation(playerInformation.PlayerTeamInformationId);
    }

    #endregion

    #region Command

    [Command]
    void CmdSetDeviceType(DeviceType type)
    {
        serverDeviceType = type;
    }

    [Command]
    void CmdSetDeviceName(string dName)
    {
        ServerDeviceName = dName;
    }

    [Command]
    public void CmdSetUniqueIdentifier(string id)
    {
        deviceUniqueIdentifier = id;
    }

    [Command]
    public void CmdSetPlayerAction(GameAction action)
    {
        ServerPlayerAction = action;
    }

    [Command]
    public void CmdSetPlayerGameState(ClientGameState state)
    {
        ServerPlayerGameState = state;
    }

    [Command]
    public void CmdSetPlayerInformation(int teamInformationId)
    {
        ServerTeamInformationId = teamInformationId;
    }

    #endregion

    #region TargetRpc

    [TargetRpc]
    public void TargetSetGameState(NetworkConnection target, ClientGameState state)
    {
        gameState.Value = state;
    }

    [TargetRpc]
    public void TargetSetPairedIpAdress(NetworkConnection target, string ipAdress)
    {
        pairedIpAdress.Value = ipAdress;

        if (gameState.Value == ClientGameState.Pairing)
        {
            gameState.Value = ClientGameState.Paired;
        }
    }

    [TargetRpc]
    public void TargetSetGame(NetworkConnection target, int[] data, int sizeX, int sizeY, int labyrinthId, Algorithm algo)
    {
        playerStartSteps.Value = new int[0];

        labyrinthData.SetLabyrithData(data, sizeX, sizeY, labyrinthId);
        algorithm.Value = algo;

        if (algorithm.Value == Algorithm.Tutorial)
        {
            gameState.Value = ClientGameState.TutorialLabyrinthReady;
        }
        else
        {
            gameState.Value = ClientGameState.LabyrithReady;
        }
    }

    [TargetRpc]
    public void TargetSetGameWithSteps(NetworkConnection target, int[] steps, int[] data, int sizeX, int sizeY, int labyrinthId, Algorithm algo)
    {
        Debug.Log("StartWithSteps!");//temp

        playerStartSteps.Value = steps;

        labyrinthData.SetLabyrithData(data, sizeX, sizeY, labyrinthId);
        algorithm.Value = algo;

        if (algorithm.Value == Algorithm.Tutorial)
        {
            gameState.Value = ClientGameState.TutorialLabyrinthReady;
        }
        else
        {
            gameState.Value = ClientGameState.LabyrithReady;
        }
    }

    [TargetRpc]
    public void TargetSetTeamInformation(NetworkConnection target, int teamInformationId)
    {
        playerInformation.SetPlayerInformation(teamInformationId);
    }
    #endregion

}
