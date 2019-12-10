using UnityEngine;
using System.Collections;


namespace UdeS.Promoscience.Labyrinths.Editor
{
    public class Tile : UnityEngine.Tilemaps.Tile
    {
        [SerializeField]
        public TileType Type;
    }
}
