﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public string deviceUniqueIdentifier = "";
    public int sDeviceType;

    string mDeviceName = "";
    string mTeamName = "";
    Color mTeamColor = Color.white;
    int mPlayerStatus = 0;
    int mPlayerAction;


    public string deviceName
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

    public string teamName
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

    public Color teamColor
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

    public int playerStatus
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

    public int playerAction
    {
        get
        {
            return mPlayerAction;
        }
        set
        {
            mPlayerAction = value;
            playerActionChangedEvent();
        }
    }

    public Action deviceNameChangedEvent;
    public Action teamNameChangedEvent;
    public Action teamColorChangedEvent;
    public Action playerStatusChangedEvent;
    public Action playerActionChangedEvent;

    [SerializeField]
    ScriptableDeviceType deviceType;

    [SerializeField]
    ScriptableString pairedIpAdress;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    [SerializeField]
    ScriptableInteger algorithmId;

    [SerializeField]
    ScriptableInteger gameState;

    [SerializeField]
    ScriptableInteger action;



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
        deviceName = SystemInfo.deviceModel;

        if (deviceType.value == Constants.DEVICE_TABLET)
        {
            action.valueChangedEvent += SendCmdPlayerAction;//Is it were it should be?
        }

        CmdSetDeviceType(deviceType.value);
        CmdSetDeviceName(deviceName);
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

    void OnDeviceNameChanged()
    {
        if (deviceNameChangedEvent != null)
        {
            deviceNameChangedEvent();
        }
    }

    void OnTeamNameChanged()
    {
        if (teamNameChangedEvent != null)
        {
            teamNameChangedEvent();
        }
    }

    void OnTeamColorChanged()
    {
        if (teamColorChangedEvent != null)
        {
            teamColorChangedEvent();
        }
    }

    void OnPlayerStatusChanged()
    {
        if (playerStatusChangedEvent != null)
        {
            playerStatusChangedEvent();
        }
    }

    [Command]
    void CmdSetDeviceType(int dType)
    {
        sDeviceType = dType;
    }

    [Command]
    void CmdSetDeviceName(string dName)
    {
        deviceName = dName;
    }

    [Command]
    public void CmdSetUniqueIdentifier(string id)
    {
        deviceUniqueIdentifier = id;
    }

    [Command]
    public void CmdSetPlayerStatus(int id)
    {
        playerStatus = id;
    }

    [Command]
    public void CmdSetPlayerAction(int actionId)
    {
        playerAction = actionId;
    }

    [TargetRpc]
    public void TargetSetPlayerStatus(NetworkConnection target, int id)
    {
        playerStatus = id;
    }

    [TargetRpc]
    public void TargetSetLabiryntheData(NetworkConnection target,int id)
    {
        //labiryntheId = id;
    }

    [TargetRpc]
    public void TargetSetPairedIpAdress(NetworkConnection target, string ipAdress)
    {
        UITextManager.instance.SetText("PairedIP : " + ipAdress);//temp
        pairedIpAdress.value = ipAdress;
        gameState.value = Constants.PAIRED;//Temp?
    }

    [TargetRpc]
    public void TargetSetGame(NetworkConnection target, int[] map, int sizeX,int sizeY, int labyrinthId, int algorithmIdentifier)//Unet permet pas le [,]
    {
        UITextManager.instance.SetText("Game data receive : getting ready to start");//temp
        labyrinthData.SetLabyrithDataWitId(map, sizeX, sizeY, labyrinthId);

        algorithmId.value = algorithmIdentifier;

        if(algorithmIdentifier == Constants.TUTORIAL_ALGORITH)
        {
            gameState.value = Constants.READY_TUTORIAL;
        }
        else
        {
            gameState.value = Constants.LABYRITH_READY;
        }
    }
}