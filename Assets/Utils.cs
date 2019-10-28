using UnityEngine;
using System.Collections;
using System;
////using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience
{
    // 
    [Serializable]
    public struct Tile
    {
        public int x;
        public int y;
        public TileColor color;

        public Tile(int xPosition, int yPosition, TileColor tileColor)
        {
            x = xPosition;
            y = yPosition;
            color = tileColor;
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
        SimpleReplay = 1 << 4,
        LabyrinthSelect = 1 << 5,
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


    public static class Utils
    {
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

        public const float TILE_SIZE = 5.0f;
        public const float MOVEMENT_SPEED = 4f;
        public const float MOVEMENT_ACCELERATION = 1f;
        public const float MOVEMENT_MAX_SPEED = 0.5f;
        public const float TURNING_SPEED = 1.0f;
        public const float TURNING_ACCELERATION = 2f;
        public const float TURNING_MAX_SPEED = 0.5f;

        //Labyrinth tiles
        //0-49 Starts
        public const int TILE_START_START_ID = 0;
        public const int TILE_START_ID = 0;
        public const int TILE_ROME_START_ID = 1;
        public const int TILE_PTOL_START_ID = 2;
        public const int TILE_BRIT_START_ID = 3;
        public const int TILE_KART_START_ID = 4;
        public const int TILE_START_END_ID = 49;

        //50-99 Floors
        public const int TILE_FLOOR_START_ID = 50;
        public const int TILE_FLOOR_ID = 50;
        public const int TILE_ROME_FLOOR_ID = 51;
        public const int TILE_PTOL_FLOOR_ID = 52;
        public const int TILE_BRIT_FLOOR_ID = 53;
        public const int TILE_KART_FLOOR_ID = 54;
        public const int TILE_FLOOR_END_ID = 99;

        //100-149 Ends
        public const int TILE_END_START_ID = 100;
        public const int TILE_END_ID = 100;
        public const int TILE_ROME_END_ID = 101;
        public const int TILE_PTOL_END_ID = 102;
        public const int TILE_BRIT_END_ID = 103;
        public const int TILE_KART_END_ID = 104;
        public const int TILE_END_END_ID = 149;

        //150-199 Walls
        public const int TILE_WALL_START_ID = 150;
        public const int TILE_WALL_ID = 150;
        public const int TILE_ROME_HORIZONTAL_WALL_ID = 151;
        public const int TILE_ROME_HORIZONTAL_WALL_B_ID = 152;
        public const int TILE_ROME_VERTICAL_WALL_ID = 153;
        public const int TILE_ROME_VERTICAL_WALL_B_ID = 154;
        public const int TILE_ROME_TOWER_WALL_ID = 155;
        public const int TILE_PTOL_HORIZONTAL_WALL_ID = 156;
        public const int TILE_PTOL_HORIZONTAL_WALL_B_ID = 157;
        public const int TILE_PTOL_VERTICAL_WALL_ID = 158;
        public const int TILE_PTOL_VERTICAL_WALL_B_ID = 159;
        public const int TILE_PTOL_TOWER_WALL_ID = 160;
        public const int TILE_BRIT_HORIZONTAL_WALL_ID = 161;
        public const int TILE_BRIT_VERTICAL_WALL_ID = 162;
        public const int TILE_BRIT_TOWER_WALL_ID = 163;
        public const int TILE_BRIT_TOWER_WALL_2_ID = 164;
        public const int TILE_KART_HORIZONTAL_WALL_ID = 165;
        public const int TILE_KART_HORIZONTAL_WALL_B_ID = 166;
        public const int TILE_KART_VERTICAL_WALL_ID = 167;
        public const int TILE_KART_VERTICAL_WALL_B_ID = 168;
        public const int TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID = 169;
        public const int TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_B_ID = 170;
        public const int TILE_KART_VERTICAL_WALL_SCAFFOLDING_ID = 171;
        public const int TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID = 172;
        public const int TILE_KART_TOWER_WALL_ID = 173;
        public const int TILE_KART_TOWER_WALL_2_ID = 174;
        public const int TILE_KART_TOWER_WALL_SCAFFOLDING_ID = 175;

        public const int TILE_WALL_END_ID = 199;

    }
}