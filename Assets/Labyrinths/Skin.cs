using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace UdeS.Promoscience.Labyrinths
{
    public interface ISkin
    {
        IEnumerable<Piece> Pieces { get; }
    }


    public static partial class Utils
    {
        public static Piece GetPiece(this ISkin skin, TileType type)
        {
            return skin.Pieces.Where(x => x.TileType == type).First();
        }
    }
}
