using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace UdeS.Promoscience.Labyrinths
{
    public interface ISkin
    {
        IEnumerable<Piece> Pieces { get; }

        string Name { get; }
    }


    public static partial class Utils
    {
        public static Piece GetPiece(this ISkin skin, TileType type)
        {
            if (type == TileType.Empty)
                return null;

            var res = skin.Pieces.FirstOrDefault(x => x != null && x.TileTypes.Contains(type));//?.First();
            Debug.Assert(res != null, Enum.GetName(typeof(TileType), type) + " does not exist in " + skin.Name);
            return res;
        }
    }
}
