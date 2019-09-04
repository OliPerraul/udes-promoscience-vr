using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Tile", order = 1)]
    public class ScriptableTile : ScriptableObject
    {
        Vector2Int tilePosition;
        TileColor tileColor;
        TileColor tilePreviousColor;

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

        public TileColor TilePreviousColor
        {
            get
            {
                return tilePreviousColor;
            }
        }

        public void SetTile(Vector2Int position, TileColor color, TileColor previousColor)
        {
            tilePosition = position;
            tileColor = color;
            tilePreviousColor = previousColor;

            OnValueChanged();
        }

        public void SetTile(int positionX, int positionY, TileColor color, TileColor previousColor)
        {
            SetTile(new Vector2Int(positionX, positionY), color, previousColor);
        }

        public void SetTile(int positionX, int positionY, TileColor color)
        {
            SetTile(new Vector2Int(positionX, positionY), color, TileColor.NoColor);
        }

        void OnValueChanged()
        {
            if (valueChangedEvent != null)
            {
                valueChangedEvent();
            }
        }
    }
}
