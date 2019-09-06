#if UNITY_EDITOR || UNITY_STANDALONE_WIN


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Generator
{
    public class VerticalWall : Wall
    {
        public bool longueur;

        public VerticalWall(int x, int y) : base(x, y)
        {
            if (y % 2 == 0) longueur = false;
            else longueur = true;
        }

        public override bool isVerticalWall()
        {
            return true;
        }

        public override int translate(int st)
        {
            if (longueur)
            {
                switch (st)
                {
                    case 1:
                        return Constants.TILE_ROME_VERTICAL_WALL_ID;
                    case 2:
                        return Constants.TILE_PTOL_VERTICAL_WALL_ID;
                    case 3:
                        return Constants.TILE_BRIT_VERTICAL_WALL_ID;
                    default:
                        return Constants.TILE_KART_VERTICAL_WALL_ID;
                }
            }
            switch (st)
            {
                case 1:
                    return Constants.TILE_ROME_VERTICAL_WALL_B_ID;
                case 2:
                    return Constants.TILE_PTOL_VERTICAL_WALL_B_ID;
                case 3:
                    return Constants.TILE_BRIT_VERTICAL_WALL_ID;
                default:
                    return Constants.TILE_KART_VERTICAL_WALL_B_ID;
            }
        }
    }
}


#endif