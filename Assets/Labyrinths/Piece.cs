using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Labyrinths
{
    public class Piece : MonoBehaviour
    {
        [SerializeField]
        private TileType[] tileTypes;

        public IEnumerable<TileType> TileTypes => tileTypes;

    }
}