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
               
        TileType[] Tiles2 { get; set; }

        int SizeX { get; set; }

        int SizeY { get; set; }

        int GetLabyrithValueAt(int x, int y);

        int GetLabyrithXLenght();

        int GetLabyrithYLenght();

        Vector2Int StartPos { get; set; }

        Vector2Int EndPos { get; set; }

        bool GetIsTileWalkable(int x, int y);
        
        bool GetIsTileWalkable(Vector2Int tile);

        int StartDirection { get; }

        string Json { get; }

        SkinResource Skin { get; }
    }

    // TODO from json (combine with ScriptableObect)
    [Serializable]
    public class Data : IData
    {
        public string Json
        {
            get
            {
                return JsonUtility.ToJson(this);
            }
        }

        [SerializeField]
        private int id;

        // TODO remove id is a member of the labyrinth
        public int Id { get { return id; } set { id = value; } }
        
        [SerializeField]
        private int sizeX;

        [SerializeField]
        private int sizeY;

        public int SkinId;

        public SkinResource Skin
        {
            get {
                return Resources.Instance.Skins[SkinId];
            }
        }

        [SerializeField]
        private TileType[] tiles2;

        public TileType[] Tiles2 { get { return tiles2; } set { tiles2 = value; } }

        public int SizeX { get { return sizeX; } set { sizeX = value; } }

        public int SizeY { get { return sizeY; } set { sizeY = value; } }

        public Data() { }

        public Data(
            int id, 
            TileType[] tiles, 
            int sizex, 
            int sizey)            
        {
            this.Id = id;
            this.SizeX = sizex;
            this.SizeY = sizey;
        }

        [SerializeField]
        private Vector2Int startPos;

        public Vector2Int StartPos
        {
            get { return startPos; }

            set { startPos = value; }
        }

        [SerializeField]
        private Vector2Int endPos;

        public Vector2Int EndPos
        {
            get { return endPos; }

            set { endPos = value; }
        }

        public int GetLabyrithValueAt(int x, int y)
        {
            //Could add an out of bound check
            return (int) Tiles2[(x * SizeY) + y];
        }

        public int GetLabyrithXLenght()
        {
            return SizeX;
        }

        public int GetLabyrithYLenght()
        {
            return SizeY;
        }

        public bool GetIsTileWalkable(int x, int y)
        {
            if (GetLabyrithValueAt(x, y) == (int)TileType.Empty)
                    return false;

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

