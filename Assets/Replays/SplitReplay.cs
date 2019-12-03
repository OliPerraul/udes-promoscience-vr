using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays
{
    public class SplitReplay : BaseReplay
    {
        private List<Round> rounds;

        public List<Round> Rounds => rounds;

        private List<LabyrinthReplay> replays = new List<LabyrinthReplay>();

        public SplitReplay(
            ReplayManagerAsset controller,
            List<Round> rounds) :
        base(
            controller)
        {
            this.rounds = rounds;
        }

        public override void Start()
        {
            base.Start();

            Resume();

            Server.Instance.State.Set(ServerState.ReplaySelect);
        }
    }
}
