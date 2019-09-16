using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Network;
using UnityEngine;

namespace UdeS.Promoscience.Utils
{
    // 
    [Serializable]
    public struct Tile
    {
        public int x;
        public int y;
        public TileColor color;

        public Tile(int xPosition, int yPosition, TileColor tileColor)
        {
            x = xPosition;
            y = yPosition;
            color = tileColor;
        }

        public Vector2Int Position
        {
            get
            {
                return new Vector2Int(x, y);
            }
        }

    }
}
