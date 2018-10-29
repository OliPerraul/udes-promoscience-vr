using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum GameState : int
{
    NotReady = 0,
    Pairing = 1,
    NoAssociatedPair = 2,
    Paired = 3,
    Ready = 4,
    ReadyTutorial = 5,
    LabyrithReady = 6,
    TutorialLabyrinthReady = 7,
    PlayingTutorial = 8,
    Playing = 9,
    WaitingForNextRound = 10
}

public enum DeviceType : int
{
    Tablet = 1,
    Headset = 2,
    NoType = 0
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
    Grey = 0,
    Yellow = 1,
    Red = 2,
    NoColor = 3
}
