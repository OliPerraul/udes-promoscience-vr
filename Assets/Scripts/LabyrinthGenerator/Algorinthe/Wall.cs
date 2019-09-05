﻿#if UNITY_EDITOR || UNITY_STANDALONE_WIN


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.Generator
{
    public class Wall : Cell
    {
        public Wall(int x, int y) : base(x, y)
        {

        }

        public override bool isWall()
        {
            return true;
        }
    }
}

#endif