using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.Labyrinths
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/Labyrinth", order = 1)]
    public class ScriptableLabyrinth : ScriptableObject, IData
    {
        public int currentId { get; set; }
        public int[] data { get; set; }
        public int sizeX { get; set; }
        public int sizeY { get; set; }

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
                    }
                }

                OnValueChanged();
            }
        }

        public void SetLabyrithData(int[] labyrinthData, int labyrinthSizeX, int labyrinthSizeY, int id)
        {
            if (id != currentId)
            {
                currentId = id;
                data = labyrinthData;
                sizeX = labyrinthSizeX;
                sizeY = labyrinthSizeY;

                OnValueChanged();
            }
        }
    }
}

