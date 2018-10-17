using UnityEngine;
using UnityEngine.Networking;


public enum CustomMsgType : short
{
    Action = 100,
    Directive = 101,
    MovementTargetPosition = 102,
    HeadRotation = 103,
    PairingRequest = 110,
    PairingResult = 111
}

public class ActionMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.Action;

    public static short GetCustomMsgType()
    {
        return (short) type;
    }

    public short GetMsgType()
    {
        return (short) type;
    }


    public int actionId;
}

public class DirectiveMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.Directive;

    public static short GetCustomMsgType()
    {
        return (short) type;
    }

    public short GetMsgType()
    {
        return (short) type;
    }

    public int directiveId;
}

public class MovementTargetPositionMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.MovementTargetPosition;

    public static short GetCustomMsgType()
    {
        return (short) type;
    }

    public short GetMsgType()
    {
        return (short) type;
    }

    public Vector3 targetPosition;

}

public class HeadRotationMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.HeadRotation;

    public static short GetCustomMsgType()
    {
        return (short) type;
    }

    public short GetMsgType()
    {
        return (short) type;
    }

    public Vector3 rotation;
}

public class PairingRequestMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.PairingRequest;

    public static short GetCustomMsgType()
    {
        return (short) type;
    }

    public short GetMsgType()
    {
        return (short) type;
    }

    public DeviceType deviceType;
    public string deviceId;
}

public class PairingResultMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.PairingResult;

    public static short GetCustomMsgType()
    {
        return (short) type;
    }

    public short GetMsgType()
    {
        return (short) type;
    }

    public bool isPairingSucess;
    public string message;
}