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
    ScriptableVector3 headRotation;

    [SerializeField]
    ScriptableVector3 movementTargetPosition;

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

        client.Connect(pairedIpAdress.value, serverPort);
    }

    void StopClient()
    {
        client.Disconnect();
        client = null;
    }

    void OnConnect(NetworkMessage netMsg)
    {
        gameState.value = GameState.ReadyTutorial;

        action.valueChangedEvent += SendAction;
        movementTargetPosition.valueChangedEvent += SendMovementTargetPosition;//to be removed
        headRotation.valueChangedEvent += SendHeadRotation;//to be removed
    }

    void OnDisconnect(NetworkMessage netMsg)
    {
        StopClient();//Might be changed when need reconnection?
    }

    void OnDirective(NetworkMessage netMsg)
    {
        DirectiveMessage msg = netMsg.ReadMessage<DirectiveMessage>();
        directive.value = msg.directiveId;
    }

    public void SendMovementTargetPosition()
    {
        MovementTargetPositionMessage movementTargetPositionMsg = new MovementTargetPositionMessage();
        movementTargetPositionMsg.targetPosition = movementTargetPosition.value;

        client.Send(movementTargetPositionMsg.GetMsgType(), movementTargetPositionMsg);
    }

    public void SendAction()
    {
        ActionMessage actionMsg = new ActionMessage();
        actionMsg.actionId = action.value;

        client.Send(actionMsg.GetMsgType(), actionMsg);
    }

    public void SendHeadRotation()
    {
        HeadRotationMessage headRotationMsg = new HeadRotationMessage();
        headRotationMsg.rotation = headRotation.value;

        client.Send(headRotationMsg.GetMsgType(), headRotationMsg);
    }
}
