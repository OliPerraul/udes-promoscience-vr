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
        int currentId { get; set; }

        int[] data { get; set; }

        int sizeX { get; set; }

        int sizeY { get; set; }

        int GetLabyrithValueAt(int x, int y);

        int GetLabyrithXLenght();

        int GetLabyrithYLenght();

        Vector2Int StartPos { get; }

        Vector2Int EndPos { get; }

        bool GetIsTileWalkable(int x, int y);
        
        bool GetIsTileWalkable(Vector2Int tile);

        int StartDirection { get; }

        void SetLabyrithData(int[] labyrinthData, int labyrinthSizeX, int labyrinthSizeY, int id);

        void SetLabyrithData(int[,] map, int id);
    }


    // TODO from json (combine with ScriptableObect)
    [Serializable]
    public class Data : IData
    {
        public int currentId { get; set; }
        public int[] data { get; set; }
        public int sizeX { get; set; }
        public int sizeY { get; set; }

        public Data()
        {
        }

        public Data(int id, int[] data, int sizex, int sizey)
        {
            this.currentId = id;
            this.data = data;
            this.sizeX = sizex;
            this.sizeY = sizey;

            // TODO replace
            SetLabyrithData(
                data,
                sizeX,
                sizeY,
                currentId);
        }

        private Vector2Int startPos;

        public Vector2Int StartPos
        {
            get { return startPos; }
        }

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
            if (id != currentId)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                currentId = id;
                SQLiteUtilities.ReadLabyrinthDataFromId(id, this);

                OnValueChanged();
#endif
            }

            return data;
        }

        public int GetLabyrithValueAt(int x, int y)
        {
            //Could add an out of bound check
            return data[(x * sizeY) + y];
        }

        public int GetLabyrithXLenght()
        {
            return sizeX;
        }

        public int GetLabyrithYLenght()
        {
            return sizeY;
        }

        public void SetLabyrithData(
            int[] labyrinthData, 
            int labyrinthSizeX, 
            int labyrinthSizeY, 
            int id)
        {
            currentId = id;
            data = labyrinthData;
            sizeX = labyrinthSizeX;
            sizeY = labyrinthSizeY;

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    if (
                        GetLabyrithValueAt(x, y) >= Utils.TILE_START_START_ID &&
                        GetLabyrithValueAt(x, y) <= Utils.TILE_START_END_ID)
                    {
                        startPos = new Vector2Int(x, y);
                    }
                    else if (
                        GetLabyrithValueAt(x, y) >= Utils.TILE_END_START_ID &&
                        GetLabyrithValueAt(x, y) <= Utils.TILE_END_END_ID)
                    {
                        endPos = new Vector2Int(x, y);
                    }
                }
            }
        }

        public void SetLabyrithData(int[,] map, int id)
        {
            if (id != currentId)
            {
                currentId = id;
                sizeX = map.GetLength(0);
                sizeY = map.GetLength(1);
                data = new int[sizeX * sizeY];

                for (int x = 0; x < sizeX; x++)
                {
                    for (int y = 0; y < sizeY; y++)
                    {
                        data[(x * sizeY) + y] = map[x, y];

                        if (
                            GetLabyrithValueAt(x, y) >= Utils.TILE_START_START_ID &&
                            GetLabyrithValueAt(x, y) <= Utils.TILE_START_END_ID)
                        {
                            startPos = new Vector2Int(x, y);
                        }
                        else if (
                            GetLabyrithValueAt(x, y) >= Utils.TILE_END_START_ID &&
                            GetLabyrithValueAt(x, y) <= Utils.TILE_END_END_ID)
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
                if ((GetLabyrithValueAt(x, y) >= Utils.TILE_FLOOR_START_ID && GetLabyrithValueAt(x, y) <= Utils.TILE_FLOOR_END_ID)
                    || (GetLabyrithValueAt(x, y) >= Utils.TILE_START_START_ID && GetLabyrithValueAt(x, y) <= Utils.TILE_START_END_ID)
                    || (GetLabyrithValueAt(x, y) >= Utils.TILE_END_START_ID && GetLabyrithValueAt(x, y) <= Utils.TILE_END_END_ID))
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

