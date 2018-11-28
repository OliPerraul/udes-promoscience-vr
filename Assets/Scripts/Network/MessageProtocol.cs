using UnityEngine;
using UnityEngine.Networking;


public enum CustomMsgType : short
{
    Directive = 101,
    PlayerInformation = 102,
    PlayerPosition = 103,
    PlayerRotation = 104,
    PlayerPaintTile = 105,
    PlayerReachedTheEnd = 106,
    PlayerTilesToPaint = 107,
    RequestForGameInformation = 113,
    ReturnToDivergencePointRequest = 108,
    ReturnToDivergencePointAnswer = 109,
    AlgorithmRespect = 110,
    PairingRequest = 111,
    PairingResult = 112
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

    public Directive directive;
}

public class PlayerInformationMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.PlayerInformation;

    public static short GetCustomMsgType()
    {
        return (short)type;
    }

    public short GetMsgType()
    {
        return (short)type;
    }

    public int teamInformationId;

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

    public Vector3 position;

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

public class PlayerTilesToPaintMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.PlayerTilesToPaint;

    public static short GetCustomMsgType()
    {
        return (short)type;
    }

    public short GetMsgType()
    {
        return (short)type;
    }

    public Tile[] tiles;
}

public class RequestForGameInformationMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.RequestForGameInformation;

    public static short GetCustomMsgType()
    {
        return (short)type;
    }

    public short GetMsgType()
    {
        return (short)type;
    }
}

public class ReturnToDivergencePointRequestMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.ReturnToDivergencePointRequest;

    public static short GetCustomMsgType()
    {
        return (short)type;
    }

    public short GetMsgType()
    {
        return (short)type;
    }
}

public class ReturnToDivergencePointAnswerMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.ReturnToDivergencePointAnswer;

    public static short GetCustomMsgType()
    {
        return (short)type;
    }

    public short GetMsgType()
    {
        return (short)type;
    }

    public bool answer;
}

public class AlgorithmRespectMessage : MessageBase
{
    static CustomMsgType type = CustomMsgType.AlgorithmRespect;

    public static short GetCustomMsgType()
    {
        return (short)type;
    }

    public short GetMsgType()
    {
        return (short)type;
    }


    public float algorithmRespect;
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