using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Labyrinths
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Labyrinth", order = 1)]
    public class ScriptableLabyrinth : ScriptableObject, IData
    {
        private IData data;

        public int currentId
        {
            get
            {
                return data.currentId;
            }

            set
            {
                data.currentId = value;
            }
        }


        public int sizeX { get { return data.sizeX; } set { data.sizeX = value; } }
        public int sizeY { get { return data.sizeY; } set { data.sizeY = value; } }

        public Vector2Int StartPos { get { return data.StartPos; } }

        public Vector2Int EndPos { get { return data.EndPos; } }

        public int StartDirection { get { return data.StartDirection; } }

        int[] IData.data { get { return data.data; } set { data.data = value; } }

        public bool GetIsTileWalkable(int x, int y)
        {
            return data.GetIsTileWalkable(x, y);
        }

        public bool GetIsTileWalkable(Vector2Int tile)
        {
            return data.GetIsTileWalkable(tile);
        }

        public int GetLabyrithValueAt(int x, int y)
        {
            return data.GetLabyrithValueAt(x, y);
        }

        public int GetLabyrithXLenght()
        {
            return data.GetLabyrithXLenght();
        }

        public int GetLabyrithYLenght()
        {
            return data.GetLabyrithYLenght();
        }

        public void OnEnable()
        {
            //data = new Data();
        }

        public void SetLabyrithData(int[] labyrinthData, int labyrinthSizeX, int labyrinthSizeY, int id)
        {
            data.SetLabyrithData(labyrinthData, labyrinthSizeX, labyrinthSizeY, id);
        }        

        public void SetLabyrithData(int[,] map, int id)
        {
            data.SetLabyrithData(map, id);
        }
    }
}

