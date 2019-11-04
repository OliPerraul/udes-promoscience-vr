using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Labyrinths
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Labyrinth", order = 1)]
    public class Resource : ScriptableObject, IData
    {
        [SerializeField]
        private Skin skin;

        [SerializeField]
        private Data data;

        public IData Data
        {
            get
            {
                if (data == null)
                    data = new Data();
                return data;
            }
        }

        public int Id
        {
            get
            {
                return Data.Id;
            }

            set
            {
                Data.Id = value;
            }
        }

        public int SizeX { get { return Data.SizeX; } set { Data.SizeX = value; } }

        public int SizeY { get { return Data.SizeY; } set { Data.SizeY = value; } }

        public Vector2Int StartPos { get { return Data.StartPos; } }

        public Vector2Int EndPos { get { return Data.EndPos; } }

        public int StartDirection { get { return Data.StartDirection; } }

        public int[] Tiles { get { return Data.Tiles; } set { Data.Tiles = value; } }

        public TileType[] Tiles2 { get { return Data.Tiles2; } set { Data.Tiles2 = value; } }

        public bool GetIsTileWalkable(int x, int y)
        {
            return Data.GetIsTileWalkable(x, y);
        }

        public bool GetIsTileWalkable(Vector2Int tile)
        {
            return Data.GetIsTileWalkable(tile);
        }

        public int GetLabyrithValueAt(int x, int y)
        {
            return Data.GetLabyrithValueAt(x, y);
        }

        public int GetLabyrithXLenght()
        {
            return Data.GetLabyrithXLenght();
        }

        public int GetLabyrithYLenght()
        {
            return Data.GetLabyrithYLenght();
        }

        public void SetLabyrithData(int[] labyrinthData, int labyrinthSizeX, int labyrinthSizeY, int id)
        {
            Data.SetLabyrithData(labyrinthData, labyrinthSizeX, labyrinthSizeY, id);
        }        

        public void SetLabyrithData(int[,] map, int id)
        {
            Data.SetLabyrithData(map, id);
        }
    }
}

