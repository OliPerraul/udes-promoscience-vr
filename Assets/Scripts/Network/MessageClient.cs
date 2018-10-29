using System.Collections;
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
    ScriptablePositionRotationAndTile playerPositionRotationAndTiles;

    [SerializeField]
    ScriptableBoolean returnToDivergencePointAnswer;

    [SerializeField]
    ScriptableAction returnToDivergencePointRequest;

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
        client.RegisterHandler(PlayerSetPositionRotationAndTilesMessage.GetCustomMsgType(), OnPlayerSetPositionRotationAndTiles);
        client.RegisterHandler(ReturnToDivergencePointRequestMessage.GetCustomMsgType(), OnReturnToDivergencePointRequest);

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
        returnToDivergencePointAnswer.valueChangedEvent += SendReturnToDivergencePointAnswer;
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        action.valueChangedEvent -= SendAction;
        playerPosition.valueChangedEvent -= SendPlayerPosition;
        playerRotation.valueChangedEvent -= SendPlayerRotation;
        playerPaintTile.valueChangedEvent -= SendPlayerPaintTile;
        returnToDivergencePointAnswer.valueChangedEvent -= SendReturnToDivergencePointAnswer;

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

    void OnPlayerSetPositionRotationAndTiles(NetworkMessage netMsg)
    {
        PlayerSetPositionRotationAndTilesMessage msg = netMsg.ReadMessage<PlayerSetPositionRotationAndTilesMessage>();
        playerPositionRotationAndTiles.SetPositionRotationAndTiles(msg.position, msg.rotation, msg.tiles);
    }

    void OnReturnToDivergencePointRequest(NetworkMessage netMsg)
    {
        returnToDivergencePointRequest.FireAction();
    }

    void SendAction()
    {
        ActionMessage msg = new ActionMessage();
        msg.actionId = action.Value;

        client.Send(msg.GetMsgType(), msg);
    }

    void SendPlayerPosition()
    {
        PlayerPositionMessage msg = new PlayerPositionMessage();
        msg.position = playerPosition.Value;

        client.Send(msg.GetMsgType(), msg);
    }

    void SendPlayerRotation()
    {
        PlayerRotationMessage msg = new PlayerRotationMessage();
        msg.rotation = playerRotation.Value;

        client.Send(msg.GetMsgType(), msg);
    }

    void SendPlayerPaintTile()
    {
        PlayerPaintTileMessage msg = new PlayerPaintTileMessage();
        msg.tilePositionX = playerPaintTile.TilePosition.x;
        msg.tilePositionY = playerPaintTile.TilePosition.y;
        msg.tileColor = playerPaintTile.TileColor;

        client.Send(msg.GetMsgType(), msg);
    }

    void SendReturnToDivergencePointAnswer()
    {
        ReturnToDivergencePointAnswerMessage msg = new ReturnToDivergencePointAnswerMessage();
        msg.answer = returnToDivergencePointAnswer.Value;

        client.Send(msg.GetMsgType(), msg);
    }
}
