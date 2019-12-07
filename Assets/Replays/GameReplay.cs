using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays
{
    public class GameReplay : BaseReplay
    {
        public IList<Round> rounds;

        public IList<Round> Rounds => rounds;

        private List<AlgorithmReplay> replays = new List<AlgorithmReplay>();
    
        public int labyrinthIndex = 0;

        public int LabyrinthIndexWrap {
            get {
                return labyrinthIndex;
            }

            set {
                labyrinthIndex = value;
                labyrinthIndex = labyrinthIndex.Mod(Labyrinths.Resources.NumLabyrinths);
            }
        }

        public GameReplay(
            ReplayControlsAsset controls,
            IList<Round> rounds) : base(controls)
        {
            this.rounds = rounds;
        }

        public override void Start()
        {
            base.Start();

            Resume();

            Server.Instance.State.Set(ServerState.GameReplay);
        }

        public AlgorithmReplay AddReplay()
        {
            replays.Add(new AlgorithmReplay(
                controls,
                rounds[++LabyrinthIndexWrap]));
            return replays[replays.Count - 1];
        }

    }
}
