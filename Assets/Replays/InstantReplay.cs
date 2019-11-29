using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays
{
    public class InstantReplay : LabyrinthReplay
    {
        public InstantReplay(
            ReplayManagerAsset controller, 
            List<Course> courses,
            IData labyrinth) : 
                base(
                    controller, 
                    courses, 
                    labyrinth) { }

        public override void Start()
        {
            base.Start();          

            Resume();

            Server.Instance.State.Set(ServerState.InstantReplay);
            
            controller.SendAction(ReplayAction.ToggleOptions, false);

            controller.SendAction(ReplayAction.ToggleAlgorithm, true);

            controller.PlaybackSpeed = 2f;
        }
    }
}