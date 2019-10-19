using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Replays
{
    public abstract class Replay : MonoBehaviour
    {
        //[SerializeField]
        protected abstract ScriptableController ReplayController { get; }

        [SerializeField]
        protected Labyrinths.ScriptableResources resources;

        [SerializeField]
        protected ScriptableServerGameInformation server;

        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {

        }

        public abstract void OnServerGameStateChanged();

        protected bool init = false;

        public virtual void OnEnable()
        {
            if (init) return;

            init = true;

            server.gameStateChangedEvent += OnServerGameStateChanged;

            ReplayController.OnActionHandler += OnReplayAction;
        }

    }
}
