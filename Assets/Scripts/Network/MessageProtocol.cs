using UnityEngine;
using UnityEngine.Networking;


public class CustomMsgType
{
    public const short Action = 100;
    public const short Directive = 101;
    public const short MovementTargetPosition = 102;
    public const short HeadRotation = 103;
    public const short PairingRequest = 110;
    public const short PairingResult = 111;
}

public class ActionMessage : MessageBase
{
    public int actionId;
}

public class DirectiveMessage : MessageBase
{
    public int directiveId;
}

public class MovementTargetPositionMessage : MessageBase
{
    public Vector3 targetPosition;
}

public class HeadRotationMessage : MessageBase
{
    public Vector3 rotation;
}

public class PairingRequestMessage : MessageBase
{
    public int deviceType;
    public string deviceId;
}

public class PairingResultMessage : MessageBase
{
    public bool isPairingSucess;
    public string message;
}