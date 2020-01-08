using UnityEngine;
using System.Collections;
using UdeS.Promoscience;

namespace UdeS.Promoscience
{
    /// <summary>
    /// Quick version of a game consiting of only one level/round
    /// </summary>
    public class Quickplay : Game
    {
        public override ServerState LevelState => ServerState.Quickplay;

        public Quickplay(int id) : base(id) { }

    }
}