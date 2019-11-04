using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Labyrinths
{

    public interface IData
    {
        int Id { get; set; }

        int[] Tiles { get; set; }
               
        TileType[] Tiles2 { get; set; }

        int SizeX { get; set; }

        int SizeY { get; set; }

        int GetLabyrithValueAt(int x, int y);

        int GetLabyrithXLenght();

        int GetLabyrithYLenght();

        Vector2Int StartPos { get; }

        Vector2Int EndPos { get; }

        bool GetIsTileWalkable(int x, int y);
        
        bool GetIsTileWalkable(Vector2Int tile);

        int StartDirection { get; }

        void SetLabyrithData(
            int[] labyrinthData, 
            int labyrinthSizeX, 
            int labyrinthSizeY, 
            int id);

        void SetLabyrithData(
            int[,] map, 
            int id);
    }


    // TODO from json (combine with ScriptableObect)
    [Serializable]
    public class Data : IData
    {
        [SerializeField]
        private int id;
        // TODO remove id is a member of the labyrinth
        public int Id { get { return id; } set { id = value; } }

        [SerializeField]
        private int[] tiles;

        public int[] Tiles { get { return tiles; } set { tiles = value; } }

        [SerializeField]
        private TileType[] tiles2;

        public TileType[] Tiles2 { get { return tiles2; } set { tiles2 = value; } }

        [SerializeField]
        private int sizeX;

        public int SizeX { get { return sizeX; } set { sizeX = value; } }

        [SerializeField]
        private int sizeY;

        public int SizeY { get { return sizeY; } set { sizeY = value; } }

        public Data() { }        

        public Data(int id, int[] tiles, int sizex, int sizey)
        {
            this.Id = id;
            this.Tiles = tiles;
            this.SizeX = sizex;
            this.SizeY = sizey;

            // TODO replace
            SetLabyrithData(
                tiles,
                SizeX,
                SizeY,
                Id);
        }

        [SerializeField]
        private Vector2Int startPos;

        public Vector2Int StartPos
        {
            get { return startPos; }
        }

        [SerializeField]
        private Vector2Int endPos;

        public Vector2Int EndPos
        {
            get { return endPos; }
        }

        public Action valueChangedEvent;

        void OnValueChanged()
        {
            if (valueChangedEvent != null)
            {
                valueChangedEvent();
            }
        }

        public int[] GetLabyrithDataWithId(int id)
        {
            if (id != Id)
            {
                Id = id;

                SQLiteUtilities.ReadLabyrinthDataFromId(id, this);

                OnValueChanged();
            }

            return Tiles;
        }

        public int GetLabyrithValueAt(int x, int y)
        {
            //Could add an out of bound check
            return Tiles[(x * SizeY) + y];
        }

        public int GetLabyrithXLenght()
        {
            return SizeX;
        }

        public int GetLabyrithYLenght()
        {
            return SizeY;
        }

        public void SetLabyrithData(
            int[] tiles, 
            int labyrinthSizeX, 
            int labyrinthSizeY, 
            int id)
        {
            Id = id;
            Tiles = tiles;
            SizeX = labyrinthSizeX;
            SizeY = labyrinthSizeY;

            for (int x = 0; x < SizeX; x++)
            {
                for (int y = 0; y < SizeY; y++)
                {                    
                    if (
                        GetLabyrithValueAt(x, y) >= Promoscience.Utils.TILE_START_START_ID &&
                        GetLabyrithValueAt(x, y) <= Promoscience.Utils.TILE_START_END_ID)
                    {
                        startPos = new Vector2Int(x, y);
                    }
                    else if (
                        GetLabyrithValueAt(x, y) >= Promoscience.Utils.TILE_END_START_ID &&
                        GetLabyrithValueAt(x, y) <= Promoscience.Utils.TILE_END_END_ID)
                    {
                        endPos = new Vector2Int(x, y);
                    }
                }
            }
        }


        public void SetLabyrithData(int[,] map, int id)
        {
            if (id != Id)
            {
                Id = id;
                SizeX = map.GetLength(0);
                SizeY = map.GetLength(1);
                Tiles = new int[SizeX * SizeY];

                for (int x = 0; x < SizeX; x++)
                {
                    for (int y = 0; y < SizeY; y++)
                    {
                        Tiles[(x * SizeY) + y] = map[x, y];

                        if (
                            GetLabyrithValueAt(x, y) >= Promoscience.Utils.TILE_START_START_ID &&
                            GetLabyrithValueAt(x, y) <= Promoscience.Utils.TILE_START_END_ID)
                        {
                            startPos = new Vector2Int(x, y);
                        }
                        else if (
                            GetLabyrithValueAt(x, y) >= Promoscience.Utils.TILE_END_START_ID &&
                            GetLabyrithValueAt(x, y) <= Promoscience.Utils.TILE_END_END_ID)
                        {
                            endPos = new Vector2Int(x, y);
                        }
                    }
                }
            }
        }

        public bool GetIsTileWalkable(int x, int y)
        {

            if (x >= 0 && x < GetLabyrithXLenght() && y >= 0 && y < GetLabyrithYLenght())
            {
                if ((GetLabyrithValueAt(x, y) >= Promoscience.Utils.TILE_FLOOR_START_ID && GetLabyrithValueAt(x, y) <= Promoscience.Utils.TILE_FLOOR_END_ID)
                    || (GetLabyrithValueAt(x, y) >= Promoscience.Utils.TILE_START_START_ID && GetLabyrithValueAt(x, y) <= Promoscience.Utils.TILE_START_END_ID)
                    || (GetLabyrithValueAt(x, y) >= Promoscience.Utils.TILE_END_START_ID && GetLabyrithValueAt(x, y) <= Promoscience.Utils.TILE_END_END_ID))
                {
                    return true;
                }
            }

            return false;
        }

        public bool GetIsTileWalkable(Vector2Int tile)
        {
            return GetIsTileWalkable(tile.x, tile.y);
        }

        //Labyrith start should always be in a dead end
        public int StartDirection
        {
            get
            {
                // up
                int direction = 0;

                // right
                if (GetIsTileWalkable(StartPos.x + 1, StartPos.y))
                {
                    direction = 1;
                }
                // down
                else if (GetIsTileWalkable(StartPos.x, StartPos.y + 1))
                {
                    direction = 2;
                }
                // Left
                else if (GetIsTileWalkable(StartPos.x - 1, StartPos.y))
                {
                    direction = 3;
                }

                return direction;
            }
        }


    }
}

