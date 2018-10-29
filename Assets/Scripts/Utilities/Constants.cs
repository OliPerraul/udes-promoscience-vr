using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}

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

static class Constants
{

    //Algorith identifier
    public const int TUTORIAL_ALGORITHM = 0;
    public const int RIGHT_HAND_ALGORITHM = 0;
    public const int SHORTEST_FLIGHT_DISTANCE_ALGORITHM = 1;
    public const int LONGEST_STRAIGHT_ALGORITHM = 2;
    public const int STANDARD_ALGORITHM = 3;

    //Labyrinth tiles
    public const float TILE_SIZE = 5.0f;
    public const float MOVEMENT_SPEED = 0.5f;
    public const float MOVEMENT_ACCELERATION = 1f;
    public const float MOVEMENT_MAX_SPEED = 0.5f;
    public const float TURNING_SPEED = 1.0f;
    public const float TURNING_ACCELERATION = 2f;
    public const float TURNING_MAX_SPEED = 0.5f;

    //0-49 Floors
    public const int TILE_FLOOR_START_ID = 0;
    public const int TILE_FLOOR_1_ID = 0;
    public const int TILE_FLOOR_END_ID = 0;

    //50-99 Walls
    public const int TILE_WALL_START_ID = 50;
    public const int TILE_WALL_1_ID = 50;
    public const int TILE_WALL_END_ID = 99;

    //100-149 Starts
    public const int TILE_START_START_ID = 100;
    public const int TILE_START_1_ID = 100;
    public const int TILE_START_END_ID = 149;

    //150-199 Ends
    public const int TILE_END_START_ID = 150;
    public const int TILE_END_1_ID = 150;
    public const int TILE_END_END_ID = 199;

    //Directives

    public const int DIRECTIVE_TURN_RIGHT = 0;
    public const int DIRECTIVE_TURN_LEFT = 1;
    public const int DIRECTIVE_MOVE_FOWARD = 2;
    public const int DIRECTIVE_UTURN = 3;
    public const int DIRECTIVE_STOP = 4;

    //Actions
    public const int ACTION_MOVE_UP = 0;
    public const int ACTION_MOVE_RIGHT = 1;
    public const int ACTION_MOVE_DOWN = 2;
    public const int ACTION_MOVE_LEFT = 3;
    public const int ACTION_TURN_LEFT = 4;
    public const int ACTION_TURN_RIGHT = 5;
    public const int ACTION_PAINT_FLOOR = 6;
    public const int ACTION_DISTANCE_SCANNER = 7;

}
