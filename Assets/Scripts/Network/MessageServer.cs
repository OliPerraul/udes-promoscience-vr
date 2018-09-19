using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageServer : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger currentGameState;

    [SerializeField]
    ScriptableString pairedIpAdress;

    [SerializeField]
    ScriptableInteger directive;

    [SerializeField]
    ScriptableInteger action;

    [SerializeField]
    ScriptableVector3 movementTargetPosition;

    [SerializeField]
    ScriptableVector3 headRotation;

    NetworkServerSimple server = null;
    NetworkConnection clientConnection = null;

    public int serverPort = 9999;

    private void Start()
    {
        pairedIpAdress.valueChangedEvent += StartServer;
        directive.valueChangedEvent += SendDirective;
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
        server.RegisterHandler(CustomMsgType.Action, OnAction);
        server.RegisterHandler(CustomMsgType.MovementTargetPosition, OnMovementTargetPosition);
        server.RegisterHandler(CustomMsgType.HeadRotation, OnHeadRotation);

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
        currentGameState.value = Constants.READY;
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        clientConnection = null;
        StopServer();//Might be changed when need reconnection?
    }

    void OnAction(NetworkMessage netMsg)
    {
        ActionMessage msg = netMsg.ReadMessage<ActionMessage>();
        action.value = msg.actionId;
    }

    void OnMovementTargetPosition(NetworkMessage netMsg)
    {
        MovementTargetPositionMessage msg = netMsg.ReadMessage<MovementTargetPositionMessage>();
        movementTargetPosition.value = msg.targetPosition;
    }

    void OnHeadRotation(NetworkMessage netMsg)
    {
        HeadRotationMessage msg = netMsg.ReadMessage<HeadRotationMessage>();
        headRotation.value = msg.rotation;
    }

    public void SendDirective()
    {
        DirectiveMessage directiveMsg = new DirectiveMessage();
        directiveMsg.directiveId = directive.value;

        clientConnection.Send(CustomMsgType.Directive, directiveMsg);
    }

}
