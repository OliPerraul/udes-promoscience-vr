using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths;

namespace UdeS.Promoscience.Replays
{
    public class InstantReplay : SingleReplay
    {
        public InstantReplay(ReplayControllerAsset controller, IData labyrinth) : base(controller, labyrinth) { }

        public override void Start()
        {
            base.Start();          

            Resume();         
            
            controller.SendAction(ReplayAction.ToggleOptions, false);

            controller.SendAction(ReplayAction.ToggleAlgorithm, true);

            controller.PlaybackSpeed = 2f;
        }
    }
}