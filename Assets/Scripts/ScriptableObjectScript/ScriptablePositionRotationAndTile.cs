using System;
using System.Collections;
using UnityEngine;

 [CreateAssetMenu(fileName = "Data", menuName = "Data/PositionRotationAndTile", order = 1)]
 public class ScriptablePositionRotationAndTile : ScriptableObject
 {
    Vector3 position;
    Quaternion rotation;
    Tile[] tiles;

    public Action valueChangedEvent;
   
    public Vector3 GetPosition()
    {
        return position;
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }

    public Tile[] GetTiles()
    {
        return tiles;
    }

    public void SetPositionRotationAndTiles(Vector3 pos, Quaternion rot, Tile[] t)
    {
        position = pos;
        rotation = rot;
        tiles = t;

        OnValueChanged();
    }

    void OnValueChanged()
    {
        if (valueChangedEvent != null)
        {
            valueChangedEvent();
        }
    }
}

