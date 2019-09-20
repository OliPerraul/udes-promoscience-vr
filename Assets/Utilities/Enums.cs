using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Utils
{
    public enum Algorithm : int
    {
        Tutorial = 0,
        RightHand = 0,
        ShortestFlightDistance = 1,
        LongestStraight = 2,
        Standard = 3
    }

    public enum ServerGameState : int
    {
        Lobby = 0,
        Tutorial = 1,
        GameRound = 2,
        Intermission = 3,
        ViewingPlayback = 4,
    }

    public enum ClientGameState : int
    {
        NotReady = 0,
        Connecting = 1,
        Pairing = 2,
        NoAssociatedPair = 3,
        Paired = 4,
        Ready = 5,
        LabyrithReady = 6,
        TutorialLabyrinthReady = 7,
        PlayingTutorial = 8,
        Playing = 9,
        WaitingForNextRound = 10,
        Reconnecting = 11,
        ReconnectingNoAssociatedPair = 12,
        WaitingForPairConnection = 13,
        GeneratingTutorialLabyrinthDataForTest = 14,

        ViewingPlayback = 15,
        WaitingPlayback = 16,
        ConnectingSupport = 17,
    }

    [Serializable]
    public enum GameAction : int
    {
        MoveUp = 0,
        MoveRight = 1,
        MoveDown = 2,
        MoveLeft = 3,
        TurnRight = 4,
        TurnLeft = 5,
        PaintFloorYellow = 6,
        PaintFloorRed = 7,
        UnpaintFloor = 8,
        ReturnToDivergencePoint = 9,
        CompletedRound = 10,
        ReceivedDirectiveMoveForward = 11,
        ReceivedDirectiveStop = 12,
        ReceivedDirectiveTurnLeft = 13,
        ReceivedDirectiveTurnRight = 14,
        ReceivedDirectiveUturn = 15
    }

    [Serializable]
    public enum DeviceType
    {
        NoType,
        Tablet,
        Headset,
        SupportDevice,
    }

    public enum Directive
    {
        MoveForward,
        Stop,
        TurnLeft,
        TurnRight,
        UTurn
    }

    [Serializable]
    public enum TileColor : int
    {
        Grey = 0,
        Yellow = 1,
        Red = 2,
        NoColor = 3
    }
}