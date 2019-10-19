using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replays.UI
{
    public class SimpleDisplay : MainDisplay
    {
        [SerializeField]
        private ScriptableController replayController;

        protected override ScriptableController ReplayController
        {
            get
            {
                return replayController;
            }
        }

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