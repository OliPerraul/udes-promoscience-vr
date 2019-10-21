using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;

// Replay played on client device

namespace UdeS.Promoscience.Replays
{
    public class ClientReplay : Replay
    {
        public override ScriptableController Controller
        {
            get
            {
                return null;
            }
        }

        public override void OnServerGameStateChanged()
        {
            //return null;
            //throw new System.NotImplementedException();
        }
    }
}
