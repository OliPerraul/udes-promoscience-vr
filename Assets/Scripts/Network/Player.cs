using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public string deviceUniqueIdentifier = "";

    #region Server

    string mDeviceName = "";
    string mTeamName = "";
    Color mTeamColor = Color.white;
    int mPlayerStatus = 0;
    int mPlayerAction;

    public int sDeviceType = -1;
    public int sAlgorithmId;
    public int sTeamId;
    public int sCourseId;
    public int sLabyrinthId;

    public string sDeviceName
    {
        get
        {
            return mDeviceName;
        }
        set
        {
            mDeviceName = value;
            OnDeviceNameChanged();
        }
    }

    public string sTeamName
    {
        get
        {
            return mTeamName;
        }
        set
        {
            mTeamName = value;
            OnTeamNameChanged();
        }
    }

    public Color sTeamColor
    {
        get
        {
            return mTeamColor;
        }
        set
        {
            mTeamColor = value;
            OnTeamColorChanged();
        }
    }

    public int sPlayerStatus
    {
        get
        {
            return mPlayerStatus;
        }
        set
        {
            mPlayerStatus = value;
            OnPlayerStatusChanged();
        }
    }

    public int sPlayerAction
    {
        get
        {
            return mPlayerAction;
        }
        set
        {
            mPlayerAction = value;
            sPlayerActionChangedEvent();
        }
    }

    public Action sDeviceNameChangedEvent;
    public Action sTeamNameChangedEvent;
    public Action sTeamColorChangedEvent;
    public Action sPlayerStatusChangedEvent;
    public Action sPlayerActionChangedEvent;

    void OnDeviceNameChanged()
    {
        if (sDeviceNameChangedEvent != null)
        {
            sDeviceNameChangedEvent();
        }
    }

    void OnTeamNameChanged()
    {
        if (sTeamNameChangedEvent != null)
        {
            sTeamNameChangedEvent();
        }
    }

    void OnTeamColorChanged()
    {
        if (sTeamColorChangedEvent != null)
        {
            sTeamColorChangedEvent();
        }
    }

    void OnPlayerStatusChanged()
    {
        if (sPlayerStatusChangedEvent != null)
        {
            sPlayerStatusChangedEvent();
        }
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
    ScriptableInteger gameState;

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
        sDeviceName = SystemInfo.deviceModel;

        if (deviceType.value == Constants.DEVICE_TABLET)
        {
            action.valueChangedEvent += SendCmdPlayerAction;//Is it were it should be?
        }

        CmdSetDeviceType(deviceType.value);
        CmdSetDeviceName(sDeviceName);
        CmdSetUniqueIdentifier(deviceUniqueIdentifier);

        gameState.value = Constants.PAIRING;
    }

    [Client]
    void SendCmdPlayerGameState()
    {
        CmdSetPlayerStatus(gameState.value);
    }

    [Client]
    void SendCmdPlayerAction()
    {
        if (gameState.value == Constants.PLAYING)
        {
            CmdSetPlayerAction(action.value);
        }
    }

    #endregion

    #region Command

    [Command]
    void CmdSetDeviceType(int dType)
    {
        sDeviceType = dType;
    }

    [Command]
    void CmdSetDeviceName(string dName)
    {
        sDeviceName = dName;
    }

    [Command]
    public void CmdSetUniqueIdentifier(string id)
    {
        deviceUniqueIdentifier = id;
    }

    [Command]
    public void CmdSetPlayerStatus(int id)
    {
        sPlayerStatus = id;
    }

    [Command]
    public void CmdSetPlayerAction(int actionId)
    {
        sPlayerAction = actionId;
    }

    #endregion

    #region TargetRpc

    [TargetRpc]
    public void TargetSetPlayerStatus(NetworkConnection target, int id)
    {
        sPlayerStatus = id;
    }

    [TargetRpc]
    public void TargetSetPairedIpAdress(NetworkConnection target, string ipAdress)
    {
        pairedIpAdress.value = ipAdress;
        gameState.value = Constants.PAIRED;//Temp?
    }

    [TargetRpc]
    public void TargetSetPlayerAlgorithmId(NetworkConnection target, int id)
    {
        algorithmId.value = id;
    }

    [TargetRpc]
    public void TargetSetGame(NetworkConnection target, int[] data, int sizeX,int sizeY, int labyrinthId)
    {
        labyrinthData.SetLabyrithData(data, sizeX, sizeY, labyrinthId);

        if(algorithmId.value == Constants.TUTORIAL_ALGORITH)
        {
            gameState.value = Constants.READY_TUTORIAL;
        }
        else
        {
            gameState.value = Constants.LABYRITH_READY;
        }
    }
    #endregion

}
