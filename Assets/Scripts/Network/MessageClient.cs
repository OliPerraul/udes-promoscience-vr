using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MessageClient : MonoBehaviour
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

    public int chatServerPort = 9999;

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
        client.RegisterHandler(CustomMsgType.Directive, OnDirective);

        client.Connect(pairedIpAdress.value, chatServerPort);
    }

    void StopClient()
    {
        client.Disconnect();
        client = null;
    }

    void OnConnect(NetworkMessage netMsg)
    {
        currentGameState.value = Constants.READY_TUTORIAL;

        action.valueChangedEvent += SendAction;
        movementTargetPosition.valueChangedEvent += SendMovementTargetPosition;
        headRotation.valueChangedEvent += SendHeadRotation;
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
        MovementTargetPositionMessage actionMsg = new MovementTargetPositionMessage();
        actionMsg.targetPosition = movementTargetPosition.value;

        client.Send(CustomMsgType.MovementTargetPosition, actionMsg);
    }

    public void SendAction()
    {
        ActionMessage actionMsg = new ActionMessage();
        actionMsg.actionId = action.value;

        client.Send(CustomMsgType.Action, actionMsg);
    }

    public void SendHeadRotation()
    {
        HeadRotationMessage headRotationMsg = new HeadRotationMessage();
        headRotationMsg.rotation = headRotation.value;

        client.Send(CustomMsgType.HeadRotation, headRotationMsg);
    }
}
