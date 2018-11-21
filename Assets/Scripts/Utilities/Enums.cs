﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}

public enum GameAction : int
{
    MoveUp = 0,
    MoveRight = 1,
    MoveDown = 2,
    MoveLeft = 3,
    TurnRight = 4,
    TurnLeft = 5,
    PaintFloor = 6,
}

public enum DeviceType
{
    NoType,
    Tablet,
    Headset

}

public enum Directive
{
    TurnRight,
    TurnLeft,
    MoveFoward,
    UTurn,
    Stop
}

public enum TileColor : int
{
    NoColor = 0,
    Grey = 1,
    Yellow = 2,
    Red = 3
}
