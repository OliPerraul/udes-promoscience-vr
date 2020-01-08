using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System;

namespace UdeS.Promoscience.Labyrinths
{
    /// <summary>
    /// Represents a skin or theme used by a labyrinth (the visual aspect independent of the layout)
    /// </summary>
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
            if(res == null)
            Debug.LogWarning(Enum.GetName(typeof(TileType), type) + " does not exist in " + skin.Name);
            return res;
        }
    }
}
