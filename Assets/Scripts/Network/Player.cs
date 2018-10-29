using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public string deviceUniqueIdentifier = "";
    string deviceName = "";

    #region Server

    string serverTeamName = "";
    Color serverTeamColor = Color.white;
    GameState serverPlayerGameState = 0;
    int serverPlayerAction;

    public DeviceType serverDeviceType = DeviceType.NoType;
    public int serverAlgorithmId;
    public int serverTeamId;
    public int serverCourseId;
    public int serverLabyrinthId;

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

    public string ServerTeamName
    {
        get
        {
            return serverTeamName;
        }
        set
        {
            serverTeamName = value;
            OnTeamNameChanged();
        }
    }

    public Color ServerTeamColor
    {
        get
        {
            return serverTeamColor;
        }
        set
        {
            serverTeamColor = value;
            OnTeamColorChanged();
        }
    }

    public GameState ServerPlayerGameState
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

    public int ServerPlayerAction
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

    public Action serverDeviceNameChangedEvent;
    public Action serverTeamNameChangedEvent;
    public Action serverTeamColorChangedEvent;
    public Action serverPlayerStatusChangedEvent;
    public Action serverPlayerActionChangedEvent;

    void OnDeviceNameChanged()
    {
        if (serverDeviceNameChangedEvent != null)
        {
            serverDeviceNameChangedEvent();
        }
    }

    void OnTeamNameChanged()
    {
        if (serverTeamNameChangedEvent != null)
        {
            serverTeamNameChangedEvent();
        }
    }

    void OnTeamColorChanged()
    {
        if (serverTeamColorChangedEvent != null)
        {
            serverTeamColorChangedEvent();
        }
    }

    void OnPlayerStatusChanged()
    {
        if (serverPlayerStatusChangedEvent != null)
        {
            serverPlayerStatusChangedEvent();
        }
    }

    [Server]
    private void OnDestroy()
    {
        PlayerList.instance.RemovePlayer(this);
    }

    #endregion

    #region Client

    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableInteger algorithmId;

    [SerializeField]
    ScriptableDeviceType deviceType;

    [SerializeField]
    ScriptableGameState gameState;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    [SerializeField]
    ScriptableString pairedIpAdress;

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
            action.valueChangedEvent += SendCmdPlayerAction;
        }

        CmdSetDeviceType(deviceType.Value);
        CmdSetDeviceName(ServerDeviceName);
        CmdSetUniqueIdentifier(deviceUniqueIdentifier);

        gameState.Value = GameState.Pairing;
    }

    [Client]
    void SendCmdPlayerGameState()
    {
        CmdSetPlayerGameState(gameState.Value);
    }

    [Client]
    void SendCmdPlayerAction()
    {
        if (gameState.Value == GameState.Playing)
        {
            CmdSetPlayerAction(action.Value);
        }
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
    public void CmdSetPlayerGameState(GameState state)
    {
        ServerPlayerGameState = state;
    }

    [Command]
    public void CmdSetPlayerAction(int actionId)
    {
        ServerPlayerAction = actionId;
    }

    #endregion

    #region TargetRpc

    [TargetRpc]
    public void TargetSetGameState(NetworkConnection target, GameState state)
    {
        ServerPlayerGameState = state;
    }

    [TargetRpc]
    public void TargetSetPairedIpAdress(NetworkConnection target, string ipAdress)
    {
        pairedIpAdress.Value = ipAdress;
        gameState.Value = GameState.Paired;
    }

    [TargetRpc]
    public void TargetSetPlayerAlgorithmId(NetworkConnection target, int id)
    {
        algorithmId.Value = id;
    }

    [TargetRpc]
    public void TargetSetGame(NetworkConnection target, int[] data, int sizeX, int sizeY, int labyrinthId)
    {
        labyrinthData.SetLabyrithData(data, sizeX, sizeY, labyrinthId);

        if(algorithmId.Value == Constants.TUTORIAL_ALGORITHM)
        {
            gameState.Value = GameState.ReadyTutorial;
        }
        else
        {
            gameState.Value = GameState.LabyrithReady;
        }
    }
    #endregion

}
