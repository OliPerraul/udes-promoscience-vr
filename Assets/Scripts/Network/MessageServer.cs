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
    ScriptableBoolean isConnectedToPair;

    [SerializeField]
    ScriptablePlayerInformation playerInformation;

    [SerializeField]
    ScriptableVector3 playerPosition;

    [SerializeField]
    ScriptableQuaternion playerRotation;

    [SerializeField]
    ScriptableTile playerPaintTile;

    [SerializeField]
    ScriptableAction playerReachedTheEnd;

    [SerializeField]
    ScriptableBoolean returnToDivergencePointAnswer;

    [SerializeField]
    ScriptableAction returnToDivergencePointRequest;

    [SerializeField]
    ScriptableString pairedIpAdress;

    [SerializeField]
    GameLabyrinth labyrinth;

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
        if (server == null)
        {
            server = new NetworkServerSimple();
            server.RegisterHandler(MsgType.Connect, OnConnect);
            server.RegisterHandler(MsgType.Disconnect, OnDisconnect);
            server.RegisterHandler(DirectiveMessage.GetCustomMsgType(), OnDirective);
            server.RegisterHandler(RequestForGameInformationMessage.GetCustomMsgType(), OnRequestForGameInformation);
            server.RegisterHandler(ReturnToDivergencePointRequestMessage.GetCustomMsgType(), OnReturnToDivergencePointRequest);

            server.Listen(serverPort);
        }
    }

    void StopServer()
    {
        if (server != null)
        {
            server.Stop();
            server = null;
        }
    }

    private void OnDestroy()
    {
        StopServer();
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

        if(playerInformation.IsInitialize)
        {
            SendPlayerInformation();
        }
        else
        {
            playerInformation.playerInformationChangedEvent += SendPlayerInformation;
        }

        isConnectedToPair.Value = true;
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        isConnectedToPair.Value = false;

        clientConnection = null;

        action.valueChangedEvent -= SendAction;
        algorithmRespect.valueChangedEvent -= SendAlgorithmRespect;
        playerReachedTheEnd.action -= SendEndReached;
        playerPosition.valueChangedEvent -= SendPlayerPosition;
        playerRotation.valueChangedEvent -= SendPlayerRotation;
        playerPaintTile.valueChangedEvent -= SendPlayerPaintTile;
        returnToDivergencePointAnswer.valueChangedEvent -= SendReturnToDivergencePointAnswer;

        playerInformation.playerInformationChangedEvent -= SendPlayerInformation;

        Debug.Log("message disconnected");//temp
    }

    void OnDirective(NetworkMessage netMsg)
    {
        DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
        directive.Value = msg.directive;
    }

    void OnReturnToDivergencePointRequest(NetworkMessage netMsg)
    {
        returnToDivergencePointRequest.FireAction();
    }

    void OnRequestForGameInformation(NetworkMessage netMsg)
    {
        if (gameState.Value == ClientGameState.Playing || gameState.Value == ClientGameState.PlayingTutorial)
        {
            SendPlayerPosition();
            SendPlayerRotation();
            SendAlgorithmRespect();
            SendPlayerTilesToPaint();
        }
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

    void SendPlayerInformation()
    {
        PlayerInformationMessage msg = new PlayerInformationMessage();
        msg.teamInformationId = playerInformation.PlayerTeamInformationId;

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

    void SendPlayerTilesToPaint()
    {
        PlayerTilesToPaintMessage msg = new PlayerTilesToPaintMessage();
        msg.tiles = labyrinth.GetTilesToPaint();

        clientConnection.Send(msg.GetMsgType(), msg);
    }

    void SendReturnToDivergencePointAnswer()
    {
        ReturnToDivergencePointAnswerMessage msg = new ReturnToDivergencePointAnswerMessage();
        msg.answer = returnToDivergencePointAnswer.Value;

        clientConnection.Send(msg.GetMsgType(), msg);
    }
}
