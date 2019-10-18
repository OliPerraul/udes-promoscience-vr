using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replay.UI
{
    public abstract class ReplayDisplay : MonoBehaviour
    {
        [SerializeField]
        protected Resources resources;

        [SerializeField]
        protected ScriptableReplayController replayOptions;

        [SerializeField]
        protected ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        protected LabyrinthDisplay labyrinthDisplay;

        protected bool init = false;

        public virtual void OnEnable()
        {
            if (init) return;

            init = false;

            server.gameStateChangedEvent += OnServerChangedState;

        }

        public abstract void OnServerChangedState();

        public virtual bool Enabled
        {
            set
            {
                gameObject.SetActive(value); 
            }
        }
    }
}