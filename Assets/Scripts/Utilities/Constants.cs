using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class Constants
{
    //Game states and player status
    public const int NOT_READY = 0;
    public const int PAIRING = 1;
    public const int NO_ASSOCIATED_PAIR = 2;
    public const int PAIRED = 3;
    public const int READY = 4;
    public const int READY_TUTORIAL = 5;
    public const int LABYRITH_READY = 6;
    public const int TUTORIAL_LABYRITH_READY = 7;
    public const int PLAYING_TUTORIAL = 8;
    public const int PLAYING = 9;
    public const int WAITING_FOR_NEXT_ROUND = 10;

    //Device types
    public const int DEVICE_TABLET = 0;
    public const int DEVICE_HEADSET = 1;

    //Algorith identifier
    public const int TUTORIAL_ALGORITH = 0;
    public const int RIGHT_HAND_ALGORITH = 0;
    public const int SHORTEST_FLIGHT_DISTANCE_ALGORITH = 1;
    public const int LONGEST_STRAIGHT_ALGORITH = 2;
    public const int STANDARD_RECURSIVE_ALGORITH = 3;

    //Labyrinth tiles
    public const float TILE_SIZE = 5.0f;
    public const float MOVEMENT_SPEED = 0.5f;
    public const float TURNIN_SPEED = 0.5f;

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

}
