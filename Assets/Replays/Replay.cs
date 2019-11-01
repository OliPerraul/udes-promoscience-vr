using UnityEngine;
using System.Collections;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays
{
    // Playback for a single team
    public abstract class Replay : MonoBehaviour
    {
        //[SerializeField]
        //public ScriptableServerGameInformation Server;

        public abstract ScriptableController Controller { get; }

        public abstract void OnServerGameStateChanged();

        public virtual void OnValidate()
        {

        }

        public virtual void Awake()
        {
            Server.Instance.gameStateChangedEvent += OnServerGameStateChanged;
        }
    }
}
