using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths;

namespace UdeS.Promoscience.Replays
{
    public class InstantReplay : Replay
    {
        public InstantReplay(ControllerAsset controller, IData labyrinth) : base(controller, labyrinth) { }

        public override void Start()
        {
            base.Start();          

            Resume();

            controller.SendAction(ReplayAction.ToggleOptions, false);
            controller.SendAction(ReplayAction.ToggleDirtyLabyrinth, true);
            controller.PlaybackSpeed = 2f;
        }
    }
}