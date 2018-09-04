using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    //DeviceType deviceType;
    public string deviceName;
    public string teamName;
    public Color teamColor;
    int labiryntheId;
    //int[,] labInfo
    //Might use scriptable object in witch save the labyrinth information and the algorithme id that the differents games managers will use,
    //this way it will be more universal and only a few scriptable object will be useless instead of entire classes
    string pairedIpAdress;
    int gameStatus;//Might be a scriptable object for the sames reasons
    public int playerStatus = 0;


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        base.OnStartAuthority();
        deviceName = "TestName";
        CmdSetDeviceName(deviceName);
        UITextManager.instance.SetText("OnStartLocalPlayer");
        GameManager.instance.localPlayer = this;
        UITextManager.instance.ShowReadyButton();
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
        gameStatus = id;
    }

    //Server send to the owner of the player, is labirintheId
    [TargetRpc]
    public void TargetSetLabiryntheId(NetworkConnection target,int id)
    {
        labiryntheId = id;
    }
}
