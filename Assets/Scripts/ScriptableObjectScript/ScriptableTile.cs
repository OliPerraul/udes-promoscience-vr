using System;
using System.Collections;
using UnityEngine;

 [CreateAssetMenu(fileName = "Data", menuName = "Data/Tile", order = 1)]
 public class ScriptableTile : ScriptableObject
 {
    Vector2Int tilePosition;
    TileColor tileColor;

    public Action valueChangedEvent;
   
    public Vector2Int TilePosition
    {
        get
        {
            return tilePosition;
        }
    }

    public TileColor TileColor
    {
        get
        {
            return tileColor;
        }
    }

    public void SetTile(Vector2Int position, TileColor color)
    {
        tilePosition = position;
        tileColor = color;

        OnValueChanged();
    }

    public void SetTile(int positionX, int positionY, TileColor color)
    {
        SetTile(new Vector2Int(positionX, positionY), color);
    }

    void OnValueChanged()
    {
        if (valueChangedEvent != null)
        {
            valueChangedEvent();
        }
    }
}

