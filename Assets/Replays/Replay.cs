using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays
{
    public abstract class Replay : MonoBehaviour
    {
        //[SerializeField]
        protected abstract ScriptableController ReplayController { get; }

        [SerializeField]
        protected Labyrinths.ScriptableResources labyrinthResources;
        
        [SerializeField]
        protected Resources resources;

        [SerializeField]
        protected ScriptableServerGameInformation server;

  
        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {

        }

        public abstract void OnServerGameStateChanged();


        public virtual void Awake()
        {
            server.gameStateChangedEvent += OnServerGameStateChanged;
            ReplayController.OnActionHandler += OnReplayAction;
        }

        // Create replay object on replay...
        public Replay Create(Labyrinths.Labyrinth labyrinth, IEnumerable<Course> courses)
        {
            var rpl = this.Create();
            //rpl.
            return null;
        }

        public virtual void AddCourse(Course course)
        {
            //var sequence =
            //    resources.PlayerSequence.Create(
            //        course,
            //        labyrinth,
            //        labyrinthPosition);

            //playerSequences.Add(course.Id, sequence);
            //activeSequences.Add(sequence);

            //TrySetMoveCount(sequence.LocalMoveCount);

            //AdjustOffsets();
        }

    }
}
