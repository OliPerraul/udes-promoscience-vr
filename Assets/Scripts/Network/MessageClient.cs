﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageClient : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableInteger directive;

    [SerializeField]
    ScriptableGameState gameState;

    [SerializeField]
    ScriptableVector3 playerPosition;

    [SerializeField]
    ScriptableQuaternion playerRotation;

    [SerializeField]
    ScriptableTile playerPaintTile;

    [SerializeField]
    ScriptableAction playerReachedTheEnd;

    [SerializeField]
    ScriptableString pairedIpAdress;

    public int serverPort = 9999;

    NetworkClient client = null;

    private void Start()
    {
        pairedIpAdress.valueChangedEvent += StartClient;
    }

    void StartClient()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnect);
        client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
        client.RegisterHandler(DirectiveMessage.GetCustomMsgType(), OnDirective);
        client.RegisterHandler(PlayerReachedTheEndMessage.GetCustomMsgType(), OnPlayerReachedTheEnd);

        client.Connect(pairedIpAdress.Value, serverPort);
    }

    void StopClient()
    {
        client.Disconnect();
        client = null;
    }

    void OnConnect(NetworkMessage netMsg)
    {
        gameState.Value = GameState.ReadyTutorial;

        action.valueChangedEvent += SendAction;
        playerPosition.valueChangedEvent += SendPlayerPosition;
        playerRotation.valueChangedEvent += SendPlayerRotation;
        playerPaintTile.valueChangedEvent += SendPlayerPaintTile;
        //forwardDirection.valueChangedEvent += SendFowardDirection;
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        action.valueChangedEvent -= SendAction;
        playerPosition.valueChangedEvent -= SendPlayerPosition;
        playerRotation.valueChangedEvent -= SendPlayerRotation;
        playerPaintTile.valueChangedEvent -= SendPlayerPaintTile;
        //forwardDirection.valueChangedEvent -= SendFowardDirection;

        StopClient();//Might be changed when need reconnection?
    }

    void OnDirective(NetworkMessage netMsg)
    {
        DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
        directive.Value = msg.directiveId;
    }

    void OnPlayerReachedTheEnd(NetworkMessage netMsg)
    {
        playerReachedTheEnd.FireAction();
    }

    public void SendAction()
    {
        ActionMessage actionMsg = new ActionMessage();
        actionMsg.actionId = action.Value;

        client.Send(actionMsg.GetMsgType(), actionMsg);
    }

    public void SendPlayerPosition()
    {
        PlayerPositionMessage movementTargetPositionMsg = new PlayerPositionMessage();
        movementTargetPositionMsg.targetPosition = playerPosition.Value;

        client.Send(movementTargetPositionMsg.GetMsgType(), movementTargetPositionMsg);
    }

    public void SendPlayerRotation()
    {
        PlayerRotationMessage headRotationMsg = new PlayerRotationMessage();
        headRotationMsg.rotation = playerRotation.Value;

        client.Send(headRotationMsg.GetMsgType(), headRotationMsg);
    }

    public void SendPlayerPaintTile()
    {
        PlayerPaintTileMessage playerPaintTileMessage = new PlayerPaintTileMessage();
        playerPaintTileMessage.tilePositionX = playerPaintTile.TilePosition.x;
        playerPaintTileMessage.tilePositionY = playerPaintTile.TilePosition.y;
        playerPaintTileMessage.tileColor = playerPaintTile.TileColor;

        client.Send(playerPaintTileMessage.GetMsgType(), playerPaintTileMessage);
    }
}
