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
        //private Vector2Int tilePosition;

        //private TileColor tileColor;

        private TileColor tilePreviousColor;

        private Tile tile;

        public Action valueChangedEvent;

        public void OnEnable()
        {
            tile.color = TileColor.Yellow;
        }


        public Vector2Int TilePosition
        {
            get
            {
                return tile.Position;
            }
        }

        public TileColor TileColor
        {
            get
            {
                return tile.color;
            }
        }

        public TileColor TilePreviousColor
        {
            get
            {
                return tilePreviousColor;
            }
        }

        public Tile Tile
        {
            get
            {
                return tile;
            }
        }

        public void SetTile(Vector2Int position, TileColor color, TileColor previousColor)
        {
            tile.Position = position;
            tile.color = color;
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
