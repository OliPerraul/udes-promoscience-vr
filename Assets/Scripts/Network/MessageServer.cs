using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageServer : MonoBehaviour
{
    [SerializeField]
    ScriptableGameAction action;

    [SerializeField]
    ScriptableFloat algorithmRespect;

    [SerializeField]
    ScriptableDirective directive;

    [SerializeField]
    ScriptableClientGameState gameState;

    [SerializeField]
    ScriptableVector3 playerPosition;

    [SerializeField]
    ScriptableQuaternion playerRotation;

    [SerializeField]
    ScriptableTile playerPaintTile;

    [SerializeField]
    ScriptableAction playerReachedTheEnd;
    /*
    [SerializeField]
    ScriptablePositionRotationAndTile playerPositionRotationAndTiles;*/

    [SerializeField]
    ScriptableBoolean returnToDivergencePointAnswer;

    [SerializeField]
    ScriptableAction returnToDivergencePointRequest;

    [SerializeField]
    ScriptableString pairedIpAdress;

    NetworkServerSimple server = null;
    NetworkConnection clientConnection = null;

    public int serverPort = 9996;

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
        server.RegisterHandler(DirectiveMessage.GetCustomMsgType(), OnDirective);
        //server.RegisterHandler(PlayerSetPositionRotationAndTilesMessage.GetCustomMsgType(), OnPlayerSetPositionRotationAndTiles);
        server.RegisterHandler(ReturnToDivergencePointRequestMessage.GetCustomMsgType(), OnReturnToDivergencePointRequest);

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

        action.valueChangedEvent += SendAction;
        algorithmRespect.valueChangedEvent += SendAlgorithmRespect;
        playerReachedTheEnd.action += SendEndReached;
        playerPosition.valueChangedEvent += SendPlayerPosition;
        playerRotation.valueChangedEvent += SendPlayerRotation;
        playerPaintTile.valueChangedEvent += SendPlayerPaintTile;
        returnToDivergencePointAnswer.valueChangedEvent += SendReturnToDivergencePointAnswer;

        gameState.Value = ClientGameState.Ready;
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        clientConnection = null;
        StopServer();//Might be changed when need reconnection?

        action.valueChangedEvent -= SendAction;
        algorithmRespect.valueChangedEvent -= SendAlgorithmRespect;
        playerReachedTheEnd.action -= SendEndReached;
        playerPosition.valueChangedEvent -= SendPlayerPosition;
        playerRotation.valueChangedEvent -= SendPlayerRotation;
        playerPaintTile.valueChangedEvent -= SendPlayerPaintTile;
        returnToDivergencePointAnswer.valueChangedEvent -= SendReturnToDivergencePointAnswer;
    }

    void OnDirective(NetworkMessage netMsg)
    {
        DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
        directive.Value = msg.directive;
    }

    /*
    void OnPlayerSetPositionRotationAndTiles(NetworkMessage netMsg)
    {
        PlayerSetPositionRotationAndTilesMessage msg = netMsg.ReadMessage<PlayerSetPositionRotationAndTilesMessage>();
        playerPositionRotationAndTiles.SetPositionRotationAndTiles(msg.position, msg.rotation, msg.tiles);
    }*/

    void OnReturnToDivergencePointRequest(NetworkMessage netMsg)
    {
        returnToDivergencePointRequest.FireAction();
    }

    void SendAction()
    {
        ActionMessage msg = new ActionMessage();
        msg.action = action.Value;

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendAlgorithmRespect()
    {
        AlgorithmRespectMessage msg = new AlgorithmRespectMessage();
        msg.algorithmRespect = algorithmRespect.Value;

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendEndReached()
    {
        PlayerReachedTheEndMessage msg = new PlayerReachedTheEndMessage();

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendPlayerPosition()
    {
        PlayerPositionMessage msg = new PlayerPositionMessage();
        msg.position = playerPosition.Value;

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendPlayerRotation()
    {
        PlayerRotationMessage msg = new PlayerRotationMessage();
        msg.rotation = playerRotation.Value;

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendPlayerPaintTile()
    {
        PlayerPaintTileMessage msg = new PlayerPaintTileMessage();
        msg.tilePositionX = playerPaintTile.TilePosition.x;
        msg.tilePositionY = playerPaintTile.TilePosition.y;
        msg.tileColor = playerPaintTile.TileColor;

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendReturnToDivergencePointAnswer()
    {
        ReturnToDivergencePointAnswerMessage msg = new ReturnToDivergencePointAnswerMessage();
        msg.answer = returnToDivergencePointAnswer.Value;

        clientConnection.Send(msg.GetMsgType(), msg);
    }
}
