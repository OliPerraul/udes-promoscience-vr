using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public string deviceUniqueIdentifier = "";
    public int deviceType = -1;// Could be changed

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

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    //Could be change so that the logic is in the scriptable object for the algorithm, doing so will allow to add new algorithm without hardcoding it?
    [SerializeField]
    ScriptableInteger algorithmId;

    [SerializeField]
    ScriptableInteger gameState;
   


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

        if (deviceName == Constants.SAMSUNG_TABLET_SMT380)
        {
            deviceType = Constants.ANDROID_TABLET;
        }
        else if (deviceName == Constants.OCCULUS_GO_PACIFIC)
        {
            deviceType = Constants.OCCULUS_GO_HEADSET;
        }
        CmdSetDeviceType(deviceType);
        CmdSetDeviceName(deviceName);
        CmdSetUniqueIdentifier(deviceUniqueIdentifier);

        gameState.value = Constants.PAIRING;
    }

    [Client]
    void SendCmdPlayerGameState()
    {
        CmdSetPlayerStatus(gameState.value);
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
        deviceType = dType;
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

    [ClientRpc]
    void RpcSetGameStatus(int id)
    {
        gameState.value = id;
    }

    //Server send to the owner of the player, is labirintheId
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

    public void TargetSetGame(NetworkConnection target, int[] map, int sizeX,int sizeY, int startPosX, int startPosY, int labyrinthId, int algorithmIdentifier)//Unet permet pas le [,]
    {
        UITextManager.instance.SetText("Game data receive : getting ready to start");//temp
        labyrinthData.SetLabyrithDataWitId(map, sizeX, sizeY, startPosX, startPosY, labyrinthId);

        algorithmId.value = algorithmIdentifier;

        if(algorithmIdentifier == Constants.TUTORIAL_ALGORITH)
        {
            gameState.value = Constants.PLAYING_TUTORIAL;
        }
        else
        {
            gameState.value = Constants.PLAYING;
        }
    }
}
