using UnityEngine;
using System.Collections;
using UdeS.Promoscience;

namespace UdeS.Promoscience
{
    public class Quickplay : Game
    {
        public override ServerState LevelState => ServerState.Quickplay;

        public Quickplay(int id) : base(id) { }

    }
}