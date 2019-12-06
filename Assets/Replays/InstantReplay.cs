using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays
{
    public class InstantReplay : LabyrinthReplay
    {
        public InstantReplay(
            List<Course> courses,
            IData labyrinth) : 
                base(
                    courses, 
                    labyrinth) { }

        public override void Start()
        {
            base.Start();          

            Resume();

            Server.Instance.State.Set(ServerState.LabyrinthReplay);
            
            IsToggleAlgorithm.Set(true);

            PlaybackSpeed = 2f;
        }
    }
}