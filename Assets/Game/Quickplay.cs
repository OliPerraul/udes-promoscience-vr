using UnityEngine;
using System.Collections;
using UdeS.Promoscience;

namespace UdeS.Promoscience
{
    public class Quickplay : Game
    {
        public Quickplay(GameAsset asset) : base(asset) { }
        
        protected override void DoStartRound()
        {
            roundState = ServerState.Quickplay;

            Server.Instance.State.Set(ServerState.Quickplay);
        }

    }
}