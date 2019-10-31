#if UNITY_EDITOR || UNITY_STANDALONE_WIN


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Generator
{
    public class Tower : Wall
    {
        public Tower(int x, int y) : base(x, y)
        {

        }

        public override bool isTower()
        {
            return true;
        }
        public override int translate(int st)
        {
            switch (st)
            {
                case 1:
                    return Utils.TILE_ROME_TOWER_WALL_ID;
                case 2:
                    return Utils.TILE_PTOL_TOWER_WALL_ID;
                case 3:
                    return Utils.TILE_BRIT_TOWER_WALL_ID;
                default:
                    return Utils.TILE_KART_TOWER_WALL_ID;
            }
        }
    }
}


#endif