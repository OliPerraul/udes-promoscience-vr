﻿using System;
using System.Collections;
using UnityEngine;

 [CreateAssetMenu(fileName = "Data", menuName = "Data/Labyrinth", order = 1)]
public class ScriptableLabyrinth : ScriptableObject
{
    int currentId = -1;
    int[,] data;

    public Action valueChangedEvent;

    void OnValueChanged()
    {
        if (valueChangedEvent != null)
        {
            valueChangedEvent();
        }
    }

    public int[,] GetLabyrithDataWitId(int id)
    {
        if(id!= currentId)
        {
            currentId = id;
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            data = SQLiteUtilities.GetLabyrintheDataWithId(id);

            OnValueChanged();
#endif
        }
        return data;
    }

    public int GetLabyrithValueAt(int x,int y)
    {
        //Could add an out of bound check
        return data[x,y];
    }

    public int GetLabyrithXLenght()
    {
        return data.GetLength(0);
    }

    public int GetLabyrithYLenght()
    {
        return data.GetLength(1);
    }

    //Use to fill labyrinth wit code instead of getting it in the database
    public void SetLabyrithDataWitId(int[,] map, int id)
    {
        if (id != currentId)
        {
            currentId = id;
            data = new int[map.GetLength(0), map.GetLength(1)];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    data[i, j] = map[i, j];
                }
            }

            OnValueChanged();
        }
    }

    public void SetLabyrithDataWitId(int[] map, int sizeX, int sizeY, int id)
    {
        if (id != currentId)
        {
            currentId = id;
            data = new int[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    data[i, j] = map[(i*sizeX)+j];
                }
            }

            OnValueChanged();
        }
    }
}
