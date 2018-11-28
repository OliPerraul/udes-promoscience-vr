using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageClient : MonoBehaviour
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
    ScriptableBoolean isDiverging;

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
    ScriptableTileArray playerTilesToPaint;

    [SerializeField]
    ScriptableBoolean returnToDivergencePointAnswer;

    [SerializeField]
    ScriptableAction returnToDivergencePointRequest;

    [SerializeField]
    ScriptableString pairedIpAdress;

    NetworkClient client = null;

    public int serverPort = 9996;

    private void Start()
    {
        pairedIpAdress.valueChangedEvent += StartServer;
    }

    void StartServer()
    {
        client = new NetworkClient();
        client.RegisterHandler(MsgType.Connect, OnConnect);
        client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
        client.RegisterHandler(ActionMessage.GetCustomMsgType(), OnAction);
        client.RegisterHandler(PlayerInformationMessage.GetCustomMsgType(), OnPlayerInformation);
        client.RegisterHandler(PlayerPositionMessage.GetCustomMsgType(), OnPlayerPosition);
        client.RegisterHandler(PlayerRotationMessage.GetCustomMsgType(), OnPlayerRotation);
        client.RegisterHandler(PlayerPaintTileMessage.GetCustomMsgType(), OnPlayerPaintTile);
        client.RegisterHandler(PlayerReachedTheEndMessage.GetCustomMsgType(), OnPlayerReachedTheEnd);
        client.RegisterHandler(PlayerTilesToPaintMessage.GetCustomMsgType(), OnPlayerTilesToPaint);
        client.RegisterHandler(ReturnToDivergencePointAnswerMessage.GetCustomMsgType(), OnReturnToDivergencePointAnswer);
        client.RegisterHandler(AlgorithmRespectMessage.GetCustomMsgType(), OnAlgorithmRespect);

        client.Connect(pairedIpAdress.Value, serverPort);
    }

    void StopClient()
    {
        client.Disconnect();
        client = null;
    }

    void OnGameStateChanged()
    {
        if (gameState.Value == ClientGameState.Playing || gameState.Value == ClientGameState.PlayingTutorial)
        {
            SendRequestForGameInformation();
        }
    }

    private void OnDestroy()
    {
        StopClient();
    }

    void OnConnect(NetworkMessage netMsg)
    {
        directive.valueChangedEvent += SendDirective;
        returnToDivergencePointRequest.action += SendReturnToDivergencePointRequest;
        gameState.valueChangedEvent += OnGameStateChanged;

        gameState.Value = ClientGameState.Ready;

        isConnectedToPair.Value = true;
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        isConnectedToPair.Value = false;

        gameState.valueChangedEvent -= OnGameStateChanged;
        directive.valueChangedEvent -= SendDirective;
        returnToDivergencePointRequest.action -= SendReturnToDivergencePointRequest;

        Debug.Log("message disconnected");//Temp

        client.Connect(pairedIpAdress.Value, serverPort);
    }

    void OnAction(NetworkMessage netMsg)
    {
        ActionMessage msg = netMsg.ReadMessage<ActionMessage>();
        action.Value = msg.action;
    }

    void OnPlayerInformation(NetworkMessage netMsg)
    {
        PlayerInformationMessage msg = netMsg.ReadMessage<PlayerInformationMessage>();
        playerInformation.SetPlayerInformation(msg.teamInformationId);
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

    void OnPlayerReachedTheEnd(NetworkMessage netMsg)
    {
        playerReachedTheEnd.FireAction();
    }

    void OnPlayerTilesToPaint(NetworkMessage netMsg)
    {
        PlayerTilesToPaintMessage msg = netMsg.ReadMessage<PlayerTilesToPaintMessage>();
        playerTilesToPaint.Value = msg.tiles;
    }

    void OnReturnToDivergencePointAnswer(NetworkMessage netMsg)
    {
        ReturnToDivergencePointAnswerMessage msg = netMsg.ReadMessage<ReturnToDivergencePointAnswerMessage>();
        returnToDivergencePointAnswer.Value = msg.answer;
    }

    void OnAlgorithmRespect(NetworkMessage netMsg)
    {
        AlgorithmRespectMessage msg = netMsg.ReadMessage<AlgorithmRespectMessage>();
        algorithmRespect.Value = msg.algorithmRespect;

        if(algorithmRespect.Value >= 1 && isDiverging.Value)
        {
            isDiverging.Value = false;
        }
        else if(algorithmRespect.Value < 1 && !isDiverging.Value)
        {
            isDiverging.Value = true;
        }
    }

    void SendDirective()
    {
        DirectiveMessage msg = new DirectiveMessage();
        msg.directive = directive.Value;

        client.Send(msg.GetMsgType(), msg);
    }
    
    void SendRequestForGameInformation()
    {
        RequestForGameInformationMessage msg = new RequestForGameInformationMessage();
        client.Send(msg.GetMsgType(), msg);
    }

    void SendReturnToDivergencePointRequest()
    {
        ReturnToDivergencePointRequestMessage msg = new ReturnToDivergencePointRequestMessage();

        client.Send(msg.GetMsgType(), msg);
    }

   
}
