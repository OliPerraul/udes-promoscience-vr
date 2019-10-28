#if UNITY_EDITOR || UNITY_STANDALONE_WIN


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Generator
{
    public class StartRoad : Road
    {
        public StartRoad(int x, int y) : base(x, y)
        {

        }

        public override bool isStart()
        {
            return true;
        }

        public override int translate(int st)
        {
            switch (st)
            {
                case 1:
                    return Promoscience.Utils.TILE_ROME_START_ID;
                case 2:
                    return Promoscience.Utils.TILE_PTOL_START_ID;
                case 3:
                    return Promoscience.Utils.TILE_BRIT_START_ID;
                default:
                    return Promoscience.Utils.TILE_KART_START_ID;
            }
        }
    }
}


#endif