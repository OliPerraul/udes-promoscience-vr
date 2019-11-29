using UnityEngine;
using System.Collections;
using UdeS.Promoscience;

namespace UdeS.Promoscience
{
    public class Quickplay : Game
    {
        public Quickplay(LevelSelectionMode order) : base(order) { }
        

        protected override void DoStartRound()
        {
            Server.Instance.State.Set(ServerState.Quickplay);
        }

    }
}