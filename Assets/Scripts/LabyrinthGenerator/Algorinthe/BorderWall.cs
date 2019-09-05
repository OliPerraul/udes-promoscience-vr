using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Game;

namespace UdeS.Promoscience.Generator
{
    public class BorderWall : Wall
    {
        public BorderWall(int x, int y) : base(x, y)
        {

        }

        public override bool isBorderWall()
        {
            return true;
        }
    }
}
