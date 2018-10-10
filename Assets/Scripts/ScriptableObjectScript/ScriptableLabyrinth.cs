using System;
using System.Collections;
using UnityEngine;

 [CreateAssetMenu(fileName = "Data", menuName = "Data/Labyrinth", order = 1)]
public class ScriptableLabyrinth : ScriptableObject
{
    int currentId = -1;
    int[] data;
    int sizeX;
    int sizeY;

    public Action valueChangedEvent;

    void OnValueChanged()
    {
        if (valueChangedEvent != null)
        {
            valueChangedEvent();
        }
    }

    public int[] GetLabyrithDataWitId(int id)
    {
        if(id!= currentId)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            currentId = id;
            SQLiteUtilities.SetLabyrintheDataWithId( id, ref sizeX, ref sizeY, ref data);
            
            OnValueChanged();
#endif
        }

        return data;
    }

    public int GetLabyrithValueAt(int x,int y)
    {
        //Could add an out of bound check
        return data[(x * sizeX) + y];
    }

    public int GetLabyrithXLenght()
    {
        return sizeX;
    }

    public int GetLabyrithYLenght()
    {
        return sizeY;
    }


    public void SetLabyrithDataWitId(int[,] map, int id)
    {
        if (id != currentId)
        {
            currentId = id;
            data = new int[map.GetLength(0) * map.GetLength(1)];

            for (int x = 0; x < sizeX; x++)
            {
                for (int y = 0; y < sizeY; y++)
                {
                    data[(x * sizeX) + y] = map[x, y];
                }
            }

            OnValueChanged();
        }
    }

    public void SetLabyrithDataWitId(int[] labyrinthData, int labyrinthSizeX, int labyrinthSizeY, int id)
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

