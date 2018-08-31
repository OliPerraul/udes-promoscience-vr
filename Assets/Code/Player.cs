﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    //DeviceType deviceType;
    public string deviceName;
    string teamName;
    Color teamColor;
    int labiryntheId;
    //int[,] labInfo
    //Might use scriptable object in witch save the labyrinth information and the algorithme id that the differents games managers will use,
    //this way it will be more universal and only a few scriptable object will be useless instead of entire classes
    string pairedIpAdress;
    int gameStatus;//Might be a scriptable object for the sames reasons


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        base.OnStartAuthority();
        deviceName = "TestName";
        CmdSetDeviceName(deviceName);
        UITextManager.instance.SetText("OnStartLocalPlayer");
    }

    [Command]
    void CmdSetDeviceName(string dName)
    {
        deviceName = dName;
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
