using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replay.UI
{
    public class IntermissionDisplay : ReplayDisplay
    {
        public override void OnEnable()
        {
            if (init) return;

            base.OnEnable();

            labyrinthDisplay.Enabled = true;
        }

        public override void OnServerChangedState()
        {
            //throw new NotImplementedException();
        }
    }
}