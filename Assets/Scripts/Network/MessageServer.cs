using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageServer : MonoBehaviour
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

    NetworkServerSimple server = null;
    NetworkConnection clientConnection = null;

    private void Start()
    {
        pairedIpAdress.valueChangedEvent += StartServer;
    }

    void Update()
    {
        if (server != null)
        {
            server.Update();
        }
    }

    void StartServer()
    {
        server = new NetworkServerSimple();
        server.RegisterHandler(MsgType.Connect, OnConnect);
        server.RegisterHandler(MsgType.Disconnect, OnDisconnect);
        server.RegisterHandler(ActionMessage.GetCustomMsgType(), OnAction);
        server.RegisterHandler(PlayerPositionMessage.GetCustomMsgType(), OnPlayerPosition);
        server.RegisterHandler(PlayerRotationMessage.GetCustomMsgType(), OnPlayerRotation);
        server.RegisterHandler(PlayerPaintTileMessage.GetCustomMsgType(), OnPlayerPaintTile);
        server.RegisterHandler(ReturnToDivergencePointAnswerMessage.GetCustomMsgType(), OnReturnToDivergencePointAnswer);

        server.Listen(serverPort);
    }

    void StopServer()
    {
        server.Stop();
        server = null;
    }

    void OnConnect(NetworkMessage netMsg)
    {
        clientConnection = netMsg.conn;

        directive.valueChangedEvent += SendDirective;
        playerReachedTheEnd.action += SendEndReached;
        playerPositionRotationAndTiles.valueChangedEvent += SendPlayerPositionRotationAndTiles;
        returnToDivergencePointRequest.action += SendReturnToDivergencePointRequest;

        gameState.Value = GameState.ReadyTutorial;//Might need to be changed when doing reconnection
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        clientConnection = null;

        directive.valueChangedEvent -= SendDirective;
        playerReachedTheEnd.action -= SendEndReached;
        playerPositionRotationAndTiles.valueChangedEvent -= SendPlayerPositionRotationAndTiles;
        returnToDivergencePointRequest.action -= SendReturnToDivergencePointRequest;

        StopServer();//Might be changed when need reconnection?
    }

    void OnAction(NetworkMessage netMsg)
    {
        ActionMessage msg = netMsg.ReadMessage<ActionMessage>();
        action.Value = msg.actionId;
    }

    void OnPlayerPosition(NetworkMessage netMsg)
    {
        PlayerPositionMessage msg = netMsg.ReadMessage<PlayerPositionMessage>();
        playerPosition.Value = msg.position;
    }

    void OnPlayerRotation(NetworkMessage netMsg)
    {
        PlayerRotationMessage msg = netMsg.ReadMessage<PlayerRotationMessage>();
        playerRotation.Value = msg.rotation;
    }

    void OnPlayerPaintTile(NetworkMessage netMsg)
    {
        PlayerPaintTileMessage msg = netMsg.ReadMessage<PlayerPaintTileMessage>();
        playerPaintTile.SetTile(msg.tilePositionX, msg.tilePositionY, msg.tileColor);
    }

    void OnReturnToDivergencePointAnswer(NetworkMessage netMsg)
    {
        ReturnToDivergencePointAnswerMessage msg = netMsg.ReadMessage<ReturnToDivergencePointAnswerMessage>();
        returnToDivergencePointAnswer.Value = msg.answer;
    }

    void SendDirective()
    {
        DirectiveMessage msg = new DirectiveMessage();
        msg.directiveId = directive.Value;

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendEndReached()
    {
        PlayerReachedTheEndMessage msg = new PlayerReachedTheEndMessage();

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendPlayerPositionRotationAndTiles()
    {
        PlayerSetPositionRotationAndTilesMessage msg = new PlayerSetPositionRotationAndTilesMessage();
        msg.position = playerPositionRotationAndTiles.GetPosition();
        msg.rotation = playerPositionRotationAndTiles.GetRotation();
        msg.tiles = playerPositionRotationAndTiles.GetTiles();

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendReturnToDivergencePointRequest()
    {
        ReturnToDivergencePointRequestMessage msg = new ReturnToDivergencePointRequestMessage();

        clientConnection.Send(msg.GetMsgType(), msg);
    }

   
}
