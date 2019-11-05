using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replays
{
    public class LabyrinthReplay
    {
        //public Replay replay;
        protected ScriptableController controller;

        public Labyrinths.IData labyrinthData;

        public Labyrinths.Labyrinth labyrinth;

        public Labyrinths.Labyrinth dirtyLabyrinth;

        protected Vector2Int lposition;

        protected Vector3 wposition;

        private Dictionary<int, PlayerSequence> playerSequences;

        private List<PlayerSequence> activeSequences;

        private List<AlgorithmSequence> algorithmSequences;

        private System.Threading.Mutex mutex;

        private bool isDirtyToggled = false;

        private bool isPlaying = false;

        public LabyrinthReplay(
            ScriptableController controller,
            Labyrinths.IData labyrinth)            
        {
            this.controller = controller;// replay;
            //this.courses = courses;
            this.labyrinthData = labyrinth;

            mutex = new System.Threading.Mutex();

            playerSequences = new Dictionary<int, PlayerSequence>();

            activeSequences = new List<PlayerSequence>();

            algorithmSequences = new List<AlgorithmSequence>();

            //replay..gameStateChangedEvent += OnServerGameStateChanged;
            Server.Instance.OnCourseAddedHandler += OnCourseAdded;

            controller.OnSequenceToggledHandler += OnSequenceToggled;

            controller.OnActionHandler += OnReplayAction;                       
        }

        public virtual void Start()
        {
            labyrinth = Labyrinths.Resources.Instance
                .GetLabyrinthTemplate(labyrinthData)
                .Create(labyrinthData);

            labyrinth.Camera.Maximize();

            labyrinth.GenerateLabyrinthVisual();

            dirtyLabyrinth = Labyrinths.Resources.Instance
                .GetLabyrinthTemplate(labyrinthData)
                .Create(labyrinthData);

            dirtyLabyrinth.name += "DIRTY";

            dirtyLabyrinth.Camera.Maximize();

            dirtyLabyrinth.transform.position = labyrinth.transform.position;

            dirtyLabyrinth.GenerateLabyrinthVisual();

            lposition = labyrinth.GetLabyrithStartPosition();

            wposition =
                labyrinth.GetLabyrinthPositionInWorldPosition(lposition);

            EnableDirty(true);

            foreach (Course course in Server.Instance.Courses)
            {
                OnCourseAdded(course);
            }
        }

        public virtual void Resume()
        {
            isPlaying = true;

            foreach (var sq in algorithmSequences)
            {
                sq.Resume();
            }

            foreach (var sq in activeSequences)
            {
                sq.Resume();
            }
        }

        public virtual void Next()
        {
            if (isPlaying)
            {
                Pause();
            }

            foreach (var sq in algorithmSequences)
            {
                if (sq.WithinBounds) sq.Next();
            }

            foreach (var sq in activeSequences)
            {
                if (sq.WithinBounds) sq.Next();
            }

            controller.GlobalMoveIndex++;
        }

        public virtual void Previous()
        {
            if (isPlaying)
            {
                Pause();
            }

            controller.GlobalMoveIndex--;

            foreach (var sq in algorithmSequences)
            {
                if (sq.WithinBounds) sq.Previous();
            }

            foreach (var sq in activeSequences)
            {
                if (sq.WithinBounds) sq.Previous();
            }
        }

        public virtual void Move(int target)
        {
            controller.GlobalMoveIndex = target;

            foreach (var sq in algorithmSequences)
            {
                sq.Move(target);
            }


            foreach (var sq in activeSequences)
            {
                sq.Move(target);
            }
        }

        public virtual void Pause()
        {
            if (!isPlaying)
                return;

            foreach (var sq in algorithmSequences)
            {
                sq.Stop();// (target);
            }

            foreach (var sq in activeSequences)
            {
                sq.Stop();
            }

            Move(controller.GlobalMoveIndex);

            isPlaying = false;
        }

        public virtual void Stop()
        {
            foreach (var sq in algorithmSequences)
            {
                sq.Stop();// (target);
            }

            foreach (var sq in activeSequences)
            {
                sq.Stop();
            }

            Move(0);

            isPlaying = false;
        }

        public void EnableDirty(bool enable)
        {
            isDirtyToggled = enable;
            labyrinth.gameObject.SetActive(!isDirtyToggled);
            dirtyLabyrinth.gameObject.SetActive(isDirtyToggled);
        }

        public void EnableOptions(bool enable)
        {
            
        }

        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ToggleOptions:                    
                    break;

                case ReplayAction.ExitReplay:
                    Clear();
                    break;

                // TODO: Handle play/ stop from replay object and not sequences
                // to prevent synch issues
                case ReplayAction.ToggleDirtyLabyrinth:
                    EnableDirty(!isDirtyToggled);
                    break;

                case ReplayAction.Play:

                    Resume();

                    break;

                case ReplayAction.Resume:
                    Resume();
                    break;

                case ReplayAction.Pause:

                    //mutex.WaitOne();

                    //Pause();

                    //mutex.ReleaseMutex();

                    break;

                case ReplayAction.Slide:

                    mutex.WaitOne();

                    int current = (int)args[0];
                    Move(current);

                    mutex.ReleaseMutex();

                    break;


                case ReplayAction.Next:

                    mutex.WaitOne();
                                       
                    Next();

                    mutex.ReleaseMutex();

                    break;

                case ReplayAction.Previous:

                    mutex.WaitOne();

                    Previous();

                    mutex.ReleaseMutex();

                    break;

                case ReplayAction.Stop:

                    //mutex.WaitOne();

                    //Stop();

                    //mutex.ReleaseMutex();

                    break;
            }
        }

        public void TrySetMoveCount(int candidateMvcnt)
        {
            if (candidateMvcnt > controller.GlobalMoveCount)
                controller.GlobalMoveCount = candidateMvcnt;
        }

        public void OnSequenceToggled(Course course, bool enabled)
        {
            playerSequences[course.Id].gameObject.SetActive(enabled);

            if (!enabled)
            {
                activeSequences.Remove(playerSequences[course.Id]);
            }
            else
            {
                activeSequences.Add(playerSequences[course.Id]);
            }

            if (activeSequences.Count != 0)
            {
                // Adjust move count to biggest sequence
                TrySetMoveCount(activeSequences.Max(x => x.LocalMoveCount));

                // Let all sqnces catch up     
                Move(Mathf.Clamp(controller.GlobalMoveIndex, 0, controller.GlobalMoveCount));

                AdjustOffsets();
            }
        }

        public virtual float GetOffsetAmount(float idx)
        {
            return
                // origin of segment at the center, move it to the left
                (-Labyrinths.Utils.TILE_SIZE / 2) +
                // number of offsets (minimum 1 to align back at the center if no other sgms)
                (idx + 1) *
                // width of the level divided by the amount of sequences we are trying to fit
                // Number of player sequences + 1 (active sequences contains algorithm)
                (Labyrinths.Utils.TILE_SIZE / (activeSequences.Count + 1));
        }

        public virtual void AdjustOffsets()
        {
            for (int i = 0; i < activeSequences.Count; i++)
            {
                activeSequences[i].AdjustOffset(GetOffsetAmount(i));
            }
        }

        protected virtual void OnSequenceFinished()
        {
            if (controller.OnSequenceFinishedHandler != null)
            {
                controller.OnSequenceFinishedHandler.Invoke();
            }
        }

        public virtual void Clear()
        {            
            dirtyLabyrinth.gameObject.Destroy();

            labyrinth.gameObject.Destroy();

            foreach (Sequence sq in algorithmSequences)
            {
                if (sq != null)
                {
                    Object.Destroy(sq.gameObject);
                }
            }

            foreach (Sequence sq in playerSequences.Values)
            {
                if (sq != null)
                {
                    Object.Destroy(sq.gameObject);
                }
            }

            playerSequences.Clear();

            activeSequences.Clear();

            algorithmSequences.Clear();

            labyrinth = null;

            dirtyLabyrinth = null;            
        }

        public void OnCourseAdded(Course course)
        {
            if (!playerSequences.ContainsKey(course.Id))
            {
                var sequence =
                    Resources.Instance.PlayerSequence.Create(
                        controller,
                        course,
                        dirtyLabyrinth,
                        lposition);

                playerSequences.Add(course.Id, sequence);

                activeSequences.Add(sequence);

                TrySetMoveCount(sequence.LocalMoveCount);

                var algorithmSeq =
                    Resources.Instance.AlgorithmSequence.Create(
                        controller,
                        dirtyLabyrinth,
                        course.Algorithm,
                        lposition
                        );

                algorithmSequences.Add(algorithmSeq);              
                
                TrySetMoveCount(algorithmSeq.LocalMoveCount);
                               
                AdjustOffsets();

                controller.SendAction(ReplayAction.AddCourse, true, course);
            }
        }

        public void OnCourseRemoved(Course course)
        {
            activeSequences.Remove(playerSequences[course.Id]);
            playerSequences.Remove(course.Id);

            TrySetMoveCount(activeSequences.Max(x => x.LocalMoveCount));

            AdjustOffsets();

            controller.SendAction(ReplayAction.AddCourse, false, course);
        }
    }
}