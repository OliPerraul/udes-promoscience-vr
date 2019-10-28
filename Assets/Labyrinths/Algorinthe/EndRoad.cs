#if UNITY_EDITOR || UNITY_STANDALONE_WIN


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Generator
{
    public class EndRoad : Cell
    {
        public EndRoad(int x, int y) : base(x, y)
        {

        }

        public override bool isEnd()
        {
            return true;
        }

        public override int translate(int st)
        {
            switch (st)
            {
                case 1:
                    return Promoscience.Utils.TILE_ROME_END_ID;
                case 2:
                    return Promoscience.Utils.TILE_PTOL_END_ID;
                case 3:
                    return Promoscience.Utils.TILE_BRIT_END_ID;
                default:
                    return Promoscience.Utils.TILE_KART_END_ID;
            }
        }
    }
}

#endif