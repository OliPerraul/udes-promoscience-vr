using UnityEngine;
using System.Collections;
using System;
using System.IO;

namespace UdeS.Promoscience.Labyrinths
{
    [Serializable]
    public enum TileType
    {
        Empty = 0,

        StartStart = Utils.TILE_START_START_ID,
        Start,
        StartEnd = Utils.TILE_START_END_ID,

        EndStart = Utils.TILE_END_START_ID,
        End,
        EndEnd = Utils.TILE_END_END_ID,

        FloorStart = Utils.TILE_FLOOR_START_ID,
        Floor1,
        Floor2,
        Floor3,
        Floor4,
        Floor5,
        Floor6,
        Floor7,
        Floor8,
        Floor9,
        Floor10,
        Floor11,
        Floor12,
        Floor13,
        Floor14,
        FloorEnd = Utils.TILE_FLOOR_END_ID,

        WallStart = Utils.TILE_WALL_START_ID,
        Horizontal1,
        Horizontal2,
        Horizontal3,
        Horizontal4,
        Vertical1,
        Vertical2,
        Vertical3,
        Vertical4,
        Corner1,
        Corner2,
        Corner3,
        WallEnd = Utils.TILE_WALL_END_ID,



        DebugStart = 1800,
        DebugRed,
        DebugYellow,
        DebugGrey,

    }

    public enum Type
    {
        Unknown,
        Small,
        Medium,
        Large
    }

    public static partial class Utils
    {
        // I think this would be smarter than saving to DB
        //public static void Save(ILabyrinth labyrinth)
        //{
        //    // Write to disk
        //    StreamWriter writer = new StreamWriter(labyrinth.Name + ".json", true);
        //    writer.Write(labyrinth.Json);
        //    writer.Flush();
        //    writer.Close();
        //}

        //public static void Save(ILabyrinth labyrinth)
        //{
        //    // Write to disk
        //    StreamWriter writer = new StreamWriter(labyrinth.Name + ".json", true);
        //    writer.Write(labyrinth.Json);
        //    writer.Flush();
        //    writer.Close();
        //}

        public static ILabyrinth LoadFromJson(string data)
        {
            // Write to disk
            return JsonUtility.FromJson<Labyrinth>(data);
        }


        /// <summary>
        /// TODO replace with Resources.NumLabyrinth
        /// </summary>
        public static int NumLabyrinth => Server.Instance.Labyrinths.Count;

        public const int SelectMaxHorizontal = 2;

        public const float SelectionOffset = 280;

        public const int SizeSmall = 9;

        public const int SizeMedium = 11;

        public const int SizeLarge = 13;

        public const int TILE_EMPTY_ID = 0;

        public const float TileSize = 5.0f;
        //Labyrinth tiles
        //0-49 Starts
        public const int TILE_START_START_ID = 1;
        public const int TILE_START_ID = 1;
        public const int TILE_ROME_START_ID = 2;
        public const int TILE_PTOL_START_ID = 3;
        public const int TILE_BRIT_START_ID = 4;
        public const int TILE_KART_START_ID = 5;
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


        public static Type GetType(ILabyrinth data)
        {
            if (data.SizeX <= SizeSmall && data.SizeY <= SizeSmall)
            {
                return Type.Small;
            }
            else if (data.SizeX <= SizeMedium && data.SizeY <= SizeMedium)
            {
                return Type.Medium;
            }
            else
            {
                return Type.Large;
            }
        }

        public static TileType[] ConvertToTiles(int[] tiles)
        {
            TileType[] res = new TileType[tiles.Length];

            for (int i = 0; i < tiles.Length; i++)
            {
                res[i] = TileType.Empty;

                if (tiles[i] == 0)
                    continue;

                if (tiles[i] >= Utils.TILE_START_START_ID && tiles[i] <= Utils.TILE_START_END_ID)
                {
                    res[i] = TileType.Start;
                }

                if (tiles[i] >= Utils.TILE_FLOOR_START_ID && tiles[i] <= Utils.TILE_FLOOR_END_ID)
                {
                    if (tiles[i] == (int) TileType.Floor1)
                    {
                        res[i] = TileType.Floor1;
                    }
                    else if (tiles[i] == (int) TileType.Floor2)
                    {
                        res[i] = TileType.Floor2;
                    }
                    else if (tiles[i] == (int) TileType.Floor3)
                    {
                        res[i] = TileType.Floor3;
                    }
                    else if (tiles[i] == (int) TileType.Floor4)
                    {
                        res[i] = TileType.Floor4;
                    }
                    else
                    {
                        res[i] = TileType.Floor1;
                    }
                     
                }

                if (tiles[i] >= Utils.TILE_END_START_ID && tiles[i] <= Utils.TILE_END_END_ID)
                {
                    res[i] = TileType.End;
                }

                if (tiles[i] >= Utils.TILE_WALL_START_ID && tiles[i] <= Utils.TILE_WALL_END_ID)
                {
                    switch (tiles[i])
                    {
                        //public const int TILE_ROME_HORIZONTAL_WALL_ID = 151;
                        //public const int TILE_ROME_HORIZONTAL_WALL_B_ID = 152;
                        //public const int TILE_ROME_VERTICAL_WALL_ID = 153;
                        //public const int TILE_ROME_VERTICAL_WALL_B_ID = 154;
                        //public const int TILE_ROME_TOWER_WALL_ID = 155;

                        //public const int TILE_PTOL_HORIZONTAL_WALL_ID = 156;
                        //public const int TILE_PTOL_HORIZONTAL_WALL_B_ID = 157;
                        //public const int TILE_PTOL_VERTICAL_WALL_ID = 158;
                        //public const int TILE_PTOL_VERTICAL_WALL_B_ID = 159;
                        //public const int TILE_PTOL_TOWER_WALL_ID = 160;

                        //public const int TILE_BRIT_HORIZONTAL_WALL_ID = 161;
                        //public const int TILE_BRIT_VERTICAL_WALL_ID = 162;
                        //public const int TILE_BRIT_TOWER_WALL_ID = 163;
                        //public const int TILE_BRIT_TOWER_WALL_2_ID = 164;

                        //public const int TILE_KART_HORIZONTAL_WALL_ID = 165;
                        //public const int TILE_KART_HORIZONTAL_WALL_B_ID = 166;
                        //public const int TILE_KART_VERTICAL_WALL_ID = 167;
                        //public const int TILE_KART_VERTICAL_WALL_B_ID = 168;
                        //public const int TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID = 169;
                        //public const int TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_B_ID = 170;
                        //public const int TILE_KART_VERTICAL_WALL_SCAFFOLDING_ID = 171;
                        //public const int TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID = 172;
                        //public const int TILE_KART_TOWER_WALL_ID = 173;
                        //public const int TILE_KART_TOWER_WALL_2_ID = 174;
                        //public const int TILE_KART_TOWER_WALL_SCAFFOLDING_ID = 175;

                        // Horizontal

                        case Utils.TILE_ROME_HORIZONTAL_WALL_ID:
                        case Utils.TILE_PTOL_HORIZONTAL_WALL_ID:
                        case Utils.TILE_BRIT_HORIZONTAL_WALL_ID:
                        case Utils.TILE_KART_HORIZONTAL_WALL_ID:
                            res[i] = TileType.Horizontal1;
                            break;

                        case Utils.TILE_ROME_HORIZONTAL_WALL_B_ID:
                        case Utils.TILE_PTOL_HORIZONTAL_WALL_B_ID:
                        case Utils.TILE_KART_HORIZONTAL_WALL_B_ID:
                            res[i] = TileType.Horizontal2;
                            break;

                        case Utils.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_ID:
                            res[i] = TileType.Horizontal3;
                            break;

                        case Utils.TILE_KART_HORIZONTAL_WALL_SCAFFOLDING_B_ID:
                            res[i] = TileType.Horizontal4;
                            break;

                        // Vertical

                        case Utils.TILE_ROME_VERTICAL_WALL_ID:
                        case Utils.TILE_PTOL_VERTICAL_WALL_ID:
                        case Utils.TILE_BRIT_VERTICAL_WALL_ID:
                        case Utils.TILE_KART_VERTICAL_WALL_ID:
                            res[i] = TileType.Vertical1;
                            break;

                        case Utils.TILE_ROME_VERTICAL_WALL_B_ID:
                        case Utils.TILE_PTOL_VERTICAL_WALL_B_ID:
                        case Utils.TILE_KART_VERTICAL_WALL_B_ID:
                            res[i] = TileType.Vertical2;
                            break;

                        case Utils.TILE_KART_VERTICAL_WALL_SCAFFOLDING_ID:
                            res[i] = TileType.Vertical3;
                            break;

                        case Utils.TILE_KART_VERTICAL_WALL_SCAFFOLDING_B_ID:
                            res[i] = TileType.Vertical4;
                            break;

                        // Corners

                        case Utils.TILE_ROME_TOWER_WALL_ID:
                        case Utils.TILE_PTOL_TOWER_WALL_ID:
                        case Utils.TILE_BRIT_TOWER_WALL_ID:
                        case Utils.TILE_KART_TOWER_WALL_ID:
                            res[i] = TileType.Corner1;
                            break;

                        case Utils.TILE_BRIT_TOWER_WALL_2_ID:
                        case Utils.TILE_KART_TOWER_WALL_2_ID:
                            res[i] = TileType.Corner2;
                            break;

                        case Utils.TILE_KART_TOWER_WALL_SCAFFOLDING_ID:
                            res[i] = TileType.Corner3;
                            break;

                        default:
                            res[i] = TileType.Horizontal1;
                            break;

                    }

                }

            }

            return res;
        }
    }


}