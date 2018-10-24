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

        gameState.Value = GameState.ReadyTutorial;//Might need to be changed when doing reconnection
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        clientConnection = null;

        directive.valueChangedEvent -= SendDirective;

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
        playerPosition.Value = msg.targetPosition;
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

    public void SendDirective()
    {
        DirectiveMessage directiveMsg = new DirectiveMessage();
        directiveMsg.directiveId = directive.Value;

        clientConnection.Send(directiveMsg.GetMsgType(), directiveMsg);
    }

    public void SendEndReached()
    {
        PlayerReachedTheEndMessage playerReachedTheEndMessageMsg = new PlayerReachedTheEndMessage();

        clientConnection.Send(playerReachedTheEndMessageMsg.GetMsgType(), playerReachedTheEndMessageMsg);
    }
}
