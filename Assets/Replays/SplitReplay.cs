using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays
{
    public class SplitReplay : BaseReplay
    {
        public IList<Round> rounds;

        public IList<Round> Rounds => rounds;

        private List<LabyrinthReplay> replays = new List<LabyrinthReplay>();

        public SplitReplay(
            IList<Round> rounds) : base()
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
