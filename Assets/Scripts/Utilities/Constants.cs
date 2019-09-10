using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Utils
{

    static class Constants
    {
        public const float TILE_SIZE = 5.0f;
        public const float MOVEMENT_SPEED = 2f;
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