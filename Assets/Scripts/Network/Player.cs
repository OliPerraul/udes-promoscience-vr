using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    //DeviceType deviceType;//Could replace device name and be use for Image and name in the lobby
    public string deviceName;
    public string teamName;
    public Color teamColor;
    public int playerStatus = 0;

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
        gameState.value = id;
    }

    //Server send to the owner of the player, is labirintheId
    [TargetRpc]
    public void TargetSetLabiryntheData(NetworkConnection target,int id)
    {
        //labiryntheId = id;
    }
}
