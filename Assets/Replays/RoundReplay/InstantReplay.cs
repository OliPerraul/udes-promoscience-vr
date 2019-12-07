using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Labyrinths;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays
{
    public class InstantReplay : RoundReplay
    {
        public InstantReplay(
            ReplayControlsAsset controls,
            Round round) : base(controls, round) { }

        public override void Start()
        {
            base.Start();          

            Resume();

            Server.Instance.State.Set(ServerState.RoundReplay);
            
            IsToggleAlgorithm.Set(true);

            controls.PlaybackSpeed = 2f;
        }
    }
}