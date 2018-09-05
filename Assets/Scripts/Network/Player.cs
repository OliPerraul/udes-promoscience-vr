using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    string mDeviceName = "";
    string mTeamName = "";
    Color mTeamColor = Color.blue;
    int mPlayerStatus = 0;

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

    public Action deviceNameChangedEvent;
    public Action teamNameChangedEvent;
    public Action teamColorChangedEvent;
    public Action playerStatusChangedEvent;

    [SerializeField]
    ScriptableString pairedIpAdress;

    //[SerializeField]
    //ScriptableIntMap labyrintheData;

    //Could be change so that the logic is in the scriptable object for the algorithm, doing so will allow to add new algorithm without hardcoding it?
    [SerializeField]
    ScriptableInteger algorithmId;

    [SerializeField]
    ScriptableInteger gameState;
   


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        base.OnStartAuthority();
        deviceName = SystemInfo.deviceModel;
        CmdSetDeviceName(deviceName);
        UITextManager.instance.SetText("OnStartLocalPlayer");
        GameManager.instance.localPlayer = this;
        UITextManager.instance.ShowReadyButton();
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
    void CmdSetDeviceName(string dName)
    {
        deviceName = dName;
    }

    [Command]
    public void CmdSetPlayerStatus(int id)
    {
        playerStatus = id;
    }

    [ClientRpc]
    void RpcSetGameStatus(int id)
    {
        gameState.value = id;
    }

    //Server send to the owner of the player, is labirintheId
    [TargetRpc]
    public void TargetSetLabiryntheData(NetworkConnection target,int id)
    {
        //labiryntheId = id;
    }
}
