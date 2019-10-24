using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replays.UI
{
    public abstract class MainDisplay : MonoBehaviour
    {
        [SerializeField]
        protected Resources resources;

        protected abstract ScriptableController ReplayController { get; }

        //[SerializeField]
        //protected ServerGame.InstancameInformation server;

        [SerializeField]
        protected LabyrinthDisplay labyrinthDisplay;

        protected bool init = false;

        public virtual void OnEnable()
        {
            if (init) return;

            init = false;

            ServerGame.Instance.gameStateChangedEvent += OnServerChangedState;
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