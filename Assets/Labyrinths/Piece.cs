using UnityEngine;
using System.Collections;


namespace UdeS.Promoscience.Labyrinths
{
    public class Piece : MonoBehaviour
    {
        [SerializeField]
        private TileType tileType;

        public TileType TileType => tileType;

    }
}