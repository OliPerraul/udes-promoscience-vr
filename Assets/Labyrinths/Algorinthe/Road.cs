#if UNITY_EDITOR || UNITY_STANDALONE_WIN


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Generator
{
    public class Road : Cell
    {
        private int count;
        private bool visited;
        private bool current;

        public Road(int x, int y) : base(x, y)
        {

        }

        public override bool isRoad()
        {
            return true;
        }

        public override int translate(int st)
        {
            switch (st)
            {
                case 1:
                    return Utils.TILE_ROME_FLOOR_ID;
                case 2:
                    return Utils.TILE_PTOL_FLOOR_ID;
                case 3:
                    return Utils.TILE_BRIT_FLOOR_ID;
                default:
                    return Utils.TILE_KART_FLOOR_ID;
            }
        }
    }
}


#endif