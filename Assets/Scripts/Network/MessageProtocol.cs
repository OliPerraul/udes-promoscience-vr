using UnityEngine;
using UnityEngine.Networking;


public enum CustomMsgType : short
{
    Action = 100,
    Directive = 101,
    PlayerPosition = 102,
    PlayerRotation = 103,
    PlayerPaintTile = 104,
    PlayerReachedTheEnd = 105,
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

public class PlayerPositionMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.PlayerPosition;

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

public class PlayerRotationMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.PlayerRotation;

    public static short GetCustomMsgType()
    {
        return (short) type;
    }

    public short GetMsgType()
    {
        return (short) type;
    }

    public Quaternion rotation;
}

public class PlayerPaintTileMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.PlayerPaintTile;

    public static short GetCustomMsgType()
    {
        return (short)type;
    }

    public short GetMsgType()
    {
        return (short)type;
    }

    public int tilePositionX;
    public int tilePositionY;
    public TileColor tileColor;
}

public class PlayerReachedTheEndMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.PlayerReachedTheEnd;

    public static short GetCustomMsgType()
    {
        return (short)type;
    }

    public short GetMsgType()
    {
        return (short)type;
    }
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