using UnityEngine;
using System.Collections;
using System;
////using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience
{
    [System.Serializable]
    public struct PositionRotationAndTile
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Tile[] Tiles;
    }


    // 
    [Serializable]
    public struct Tile
    {
        public int x;
        public int y;


        public TileColor Color
        {
            get
            {
                return color;
            }

            set
            {
                previousColor = color;
                color = value;
            }
        }            

        public TileColor color;

        // TODO private ??
        public TileColor previousColor;

        public TileColor PreviousColor { get { return previousColor; } }


        public Tile(int xPosition, int yPosition, TileColor tileColor)
        {
            x = xPosition;
            y = yPosition;
            color = tileColor;
            previousColor = TileColor.Grey;
        }

        public Vector2Int Position
        {
            get
            {
                return new Vector2Int(x, y);
            }

            set
            {
                x = value.x;
                y = value.y;
            }
        }

    }


    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3
    }

    [System.Serializable]
    public enum ServerGameState// : int
    {
        None = 0,//1 << 0,
        Lobby = 1 << 0,
        Tutorial = 1 << 1,
        GameRound = 1 << 2,
        Intermission = 1 << 3,
        InstantReplay = 1 << 4,        
        LevelSelect = 1 << 5,
        AdvancedReplay = 1 << 6,// TODO remove
        ReplaySelect = 1 << 7,
    }

    [Serializable]
    public enum ClientGameState : int
    {
        NotReady = 0,
        Connecting = 1,
        Pairing = 2,
        NoAssociatedPair = 3,
        Paired = 4,
        Ready = 5,
        LabyrinthReady = 6,
        TutorialLabyrinthReady = 7,
        PlayingTutorial = 8,
        Playing = 9,
        WaitingForNextRound = 10,
        Reconnecting = 11,
        ReconnectingNoAssociatedPair = 12,
        WaitingForPairConnection = 13,
        GeneratingTutorialLabyrinthDataForTest = 14,

        ViewingLocalReplay = 15,
        ViewingGlobalReplay = 16,
        WaitingReplay = 17,

        Finished = 18

    }

    [Serializable]
    public enum GameAction : int
    {
        //Start = -1,
        // Because the actions do not start with a 'Start' we add it after the facts (see playback code)...
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
        ReceivedDirectiveUturn = 15,

        // TODO combine?
        EndMovement = 16,
        //Finish = 17 // Sentinel value

        Unknown = 32
    }

    [Serializable]
    public enum DeviceType
    {
        NoType,
        Tablet,
        Headset,
        SupportDevice,
    }

    // THE ORDER IS IMPORTANT AND MUST MATCH THE UI LOL
    // TODO: Do not mix up UI and backend functionalities
    [Serializable]
    public enum Directive
    {
        Unknown = -1,
        TurnLeft = 0,
        MoveForward,        
        UTurn,
        TurnRight,
        Grey,
        Yellow,
        Red,
        Question,
        Compass,
        Stop,
        ReturnToDivergence
    }

    [Serializable]
    public enum TileColor : int
    {
        Grey = 0,
        Yellow = 1,
        Red = 2,
        NoColor = 3
    }


    public static class Utils
    {
        public const int NumColors = 3;

        public static Vector2Int GetMoveDestination(Vector2Int lpos, GameAction action)
        {
            // From ReturnToDivergent, EndAction, CompleteRound
            // Simply walk to the 'nextlpos', we do not increment nextlpos

            lpos.y += action == GameAction.MoveUp ? -1 : 0;
            lpos.y += action == GameAction.MoveDown ? 1 : 0;
            lpos.x += action == GameAction.MoveLeft ? -1 : 0;
            lpos.x += action == GameAction.MoveRight ? 1 : 0;
            return lpos;
        }

        public static Vector2Int GetMoveDestination(Vector2Int lpos, int action)
        {
            return GetMoveDestination(lpos, (Direction)action);
        }

        public static Vector2Int GetMoveDestination(Vector2Int lpos, Direction action)
        {
            // From ReturnToDivergent, EndAction, CompleteRound
            // Simply walk to the 'nextlpos', we do not increment nextlpos

            lpos.y += action == Direction.Up ? -1 : 0;
            lpos.y += action == Direction.Down ? 1 : 0;
            lpos.x += action == Direction.Left ? -1 : 0;
            lpos.x += action == Direction.Right ? 1 : 0;
            return lpos;
        }

        public static Vector3 GetDirectionVector(Direction action)
        {
            // From ReturnToDivergent, EndAction, CompleteRound
            // Simply walk to the 'nextlpos', we do not increment nextlpos

            switch (action)
            {
                case Direction.Down:
                    return Vector3.back;

                case Direction.Up:
                    return Vector3.forward;


                case Direction.Left:
                    return Vector3.left;


                case Direction.Right:
                    return Vector3.right;

                default:
                    return Vector3.zero;
            }
        }

        public static bool IsSameDirection(Vector3 dir1, Vector3 dir2, float angleThresholdToCheck=90)
        {
            return Vector3.Angle(dir1, dir2) < angleThresholdToCheck;
        }

        //returns -1 when to the left, 1 to the right, and 0 for forward/backward
        public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);

            if (dir > 0.0f)
            {
                return 1.0f;
            }
            else if (dir < 0.0f)
            {
                return -1.0f;
            }
            else
            {
                return 0.0f;
            }
        }


        public const float MovementSpeed = 2f;
        public const float MovementAcceleration = 1f;
        public const float MovementMaxSpeed = 0.5f;
        public const float TurningSpeed = 1.0f;
        public const float TurningAcceleration = 2f;
        public const float TurningMaxSpeed = 0.5f;
    }
}