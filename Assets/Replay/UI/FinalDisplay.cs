using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replay.UI
{
    public class FinalDisplay : ReplayDisplay
    {
        private List<LabyrinthButton> labyrinthButtons;

        [SerializeField]
        private FinalReplayResource resource;

        public override void OnEnable()
        {
            if (init) return;

            base.OnEnable();

            labyrinthButtons = new List<LabyrinthButton>();

            foreach (var btn in labyrinthButtons)
            {
                btn.OnClickedHandler += OnClicked;
            }         
        }       

        public void OnClicked(int id)
        {
            labyrinthDisplay.Enabled = true;
            Enabled = false;

            replayOptions.SendAction(ReplayAction.ToggleLabyrinth, id);
        }

        public override void OnServerChangedState()
        {
            
        }        
    }
}
