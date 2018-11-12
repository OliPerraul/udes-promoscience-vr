﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class Constants
{
    public const float TILE_SIZE = 5.0f;
    public const float MOVEMENT_SPEED = 0.5f;
    public const float MOVEMENT_ACCELERATION = 1f;
    public const float MOVEMENT_MAX_SPEED = 0.5f;
    public const float TURNING_SPEED = 1.0f;
    public const float TURNING_ACCELERATION = 2f;
    public const float TURNING_MAX_SPEED = 0.5f;

    //Labyrinth tiles
    //0-49 Floors
    public const int TILE_FLOOR_START_ID = 0;
    public const int TILE_FLOOR_ID = 0;
    public const int TILE_ROME_FLOOR_ID = 1;
    public const int TILE_PTOL_FLOOR_ID = 2;
    public const int TILE_BRIT_FLOOR_ID = 3;
    public const int TILE_KARL_FLOOR_ID = 4;
    public const int TILE_FLOOR_END_ID = 49;

    //50-99 Walls
    public const int TILE_WALL_START_ID = 50;
    public const int TILE_WALL_ID = 50;
    public const int TILE_ROME_HORIZONTAL_WALL_ID = 51;
    public const int TILE_ROME_HORIZONTAL_WALL_B_ID = 52;
    public const int TILE_ROME_VERTICAL_WALL_ID = 53;
    public const int TILE_ROME_VERTICAL_WALL_B_ID = 54;
    public const int TILE_ROME_TOWER_WALL_ID = 55;
    public const int TILE_PTOL_HORIZONTAL_WALL_ID = 56;
    public const int TILE_PTOL_HORIZONTAL_WALL_B_ID = 57;
    public const int TILE_PTOL_VERTICAL_WALL_ID = 58;
    public const int TILE_PTOL_VERTICAL_WALL_B_ID = 59;
    public const int TILE_PTOL_TOWER_WALL_ID = 60;
    public const int TILE_WALL_END_ID = 99;

    //100-149 Starts
    public const int TILE_START_START_ID = 100;
    public const int TILE_START_ID = 100;
    public const int TILE_ROME_START_ID = 101;
    public const int TILE_PTOL_START_ID = 102;
    public const int TILE_BRIT_START_ID = 103;
    public const int TILE_KARL_START_ID = 104;
    public const int TILE_START_END_ID = 149;

    //150-199 Ends
    public const int TILE_END_START_ID = 150;
    public const int TILE_END_ID = 150;
    public const int TILE_ROME_END_ID = 151;
    public const int TILE_PTOL_END_ID = 152;
    public const int TILE_BRIT_END_ID = 153;
    public const int TILE_KARL_END_ID = 154;
    public const int TILE_END_END_ID = 199;
}
