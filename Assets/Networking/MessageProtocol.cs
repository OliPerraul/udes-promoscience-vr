using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Network
{

    public enum CustomMsgType : short
    {
        Algorithm = 100,
        AlgorithmRespect = 101,
        Directive = 102,
        PairingRequest = 103,
        UnpairingRequest = 114,
        PairingResult = 104,
        PlayerInformation = 105,
        PlayerPaintTile = 106,
        PlayerPosition = 107,
        PlayerReachedTheEnd = 108,
        PlayerRotation = 109,
        PlayerTilesToPaint = 110,
        RequestForGameInformation = 111,
        ReturnToDivergencePointAnswer = 112,
        ReturnToDivergencePointRequest = 113,

        PaintingColor = 114

    }

    public class AlgorithmMessage : MessageBase
    {
        static CustomMsgType type = CustomMsgType.Algorithm;

        public static short GetCustomMsgType()
        {
            return (short)type;
        }

        public short GetMsgType()
        {
            return (short)type;
        }

        public Promoscience.Algorithms.Id algorithm;
    }


    public class CorrectingEnabledMessage : MessageBase
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


        public bool enabled;
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

    public class DirectiveMessage : MessageBase
    {
        static CustomMsgType type = CustomMsgType.Directive;

        public static short GetCustomMsgType()
        {
            return (short)type;
        }

        public short GetMsgType()
        {
            return (short)type;
        }

        public Directive directive;
    }

    public class PairingRequestMessage : MessageBase
    {
        static CustomMsgType type = CustomMsgType.PairingRequest;

        public static short GetCustomMsgType()
        {
            return (short)type;
        }

        public short GetMsgType()
        {
            return (short)type;
        }

        public Promoscience.DeviceType deviceType;
        public string deviceId;
    }

    public class UnpairingRequestMessage : MessageBase
    {
        static CustomMsgType type = CustomMsgType.PairingRequest;

        public static short GetCustomMsgType()
        {
            return (short)type;
        }

        public short GetMsgType()
        {
            return (short)type;
        }

        public Promoscience.DeviceType deviceType;
        public string deviceId;
    }

    public class PairingResultMessage : MessageBase
    {
        static CustomMsgType type = CustomMsgType.PairingResult;

        public static short GetCustomMsgType()
        {
            return (short)type;
        }

        public short GetMsgType()
        {
            return (short)type;
        }

        public bool isPairingSucess;
        public string message;
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

        public int teamId;

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


    public class PaintingColorMessage : MessageBase
    {
        static CustomMsgType type = CustomMsgType.PaintingColor;

        public static short GetCustomMsgType()
        {
            return (short)type;
        }

        public short GetMsgType()
        {
            return (short)type;
        }

        public TileColor tileColor;
    }


    public class PlayerPositionMessage : MessageBase
    {
        static CustomMsgType type = CustomMsgType.PlayerPosition;

        public static short GetCustomMsgType()
        {
            return (short)type;
        }

        public short GetMsgType()
        {
            return (short)type;
        }

        public Vector3 position;

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

    public class PlayerRotationMessage : MessageBase
    {
        static CustomMsgType type = CustomMsgType.PlayerRotation;

        public static short GetCustomMsgType()
        {
            return (short)type;
        }

        public short GetMsgType()
        {
            return (short)type;
        }

        public Quaternion rotation;
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

        public int gameRound;
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
}