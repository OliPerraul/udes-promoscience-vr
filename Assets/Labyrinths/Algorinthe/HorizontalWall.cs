#if UNITY_EDITOR || UNITY_STANDALONE_WIN


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Generator
{
    public class HorizontalWall : Wall
    {
        private bool longueur;

        public HorizontalWall(int x, int y) : base(x, y)
        {
            if (x % 2 == 0) longueur = false;
            else longueur = true;
        }

        public override bool isHorizontalWall()
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
                        return Promoscience.Utils.TILE_ROME_HORIZONTAL_WALL_ID;
                    case 2:
                        return Promoscience.Utils.TILE_PTOL_HORIZONTAL_WALL_ID;
                    case 3:
                        return Promoscience.Utils.TILE_BRIT_HORIZONTAL_WALL_ID;
                    default:
                        return Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_ID;
                }
            }
            else
            {
                switch (st)
                {
                    case 1:
                        return Promoscience.Utils.TILE_ROME_HORIZONTAL_WALL_B_ID;
                    case 2:
                        return Promoscience.Utils.TILE_PTOL_HORIZONTAL_WALL_B_ID;
                    case 3:
                        return Promoscience.Utils.TILE_BRIT_HORIZONTAL_WALL_ID;
                    default:
                        return Promoscience.Utils.TILE_KART_HORIZONTAL_WALL_B_ID;
                }
            }
        }
    }
}

#endif