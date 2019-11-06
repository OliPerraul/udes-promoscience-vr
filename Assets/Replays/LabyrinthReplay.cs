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

        protected Vector2Int lposition;

        protected Vector3 wposition;

        private Dictionary<int, PlayerSequence> playerSequences = new Dictionary<int, PlayerSequence>();

        private Dictionary<Algorithms.Id, AlgorithmSequence> algorithmSequences = new Dictionary<Algorithms.Id, AlgorithmSequence>();

        private List<PlayerSequence> activeSequences = new List<PlayerSequence>();

        private System.Threading.Mutex mutex = new System.Threading.Mutex();

        private bool isDirtyToggled = false;

        private bool isPlaying = false;

        public LabyrinthReplay(
            ScriptableController controller,
            Labyrinths.IData labyrinth)            
        {
            this.controller = controller;// replay;
            //this.courses = courses;
            this.labyrinthData = labyrinth;

            //replay..gameStateChangedEvent += OnServerGameStateChanged;
            Server.Instance.OnCourseAddedHandler += OnCourseAdded;

            controller.OnActionHandler += OnReplayAction;                       
        }

        public virtual void Start()
        {
            labyrinth = Labyrinths.Resources.Instance
                .GetLabyrinthTemplate(labyrinthData)
                .Create(labyrinthData);

            labyrinth.name += "DIRTY";

            labyrinth.Camera.Maximize();

            labyrinth.GenerateLabyrinthVisual();

            labyrinth.Init();

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

            foreach (var sq in algorithmSequences.Values)
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

            foreach (var sq in algorithmSequences.Values)
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

            foreach (var sq in algorithmSequences.Values)
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

            foreach (var sq in algorithmSequences.Values)
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

            foreach (var sq in algorithmSequences.Values)
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
            foreach (var sq in algorithmSequences.Values)
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

        bool isGreyboxToggled = false;

        public void EnableGreybox(bool enable)
        {
            isGreyboxToggled = enable;

            labyrinth.GenerateLabyrinthVisual(
                isGreyboxToggled ?
                Labyrinths.Resources.Instance.GreyboxSkin :
                null);
        }

        public void EnableDirty(bool enable)
        {
            isDirtyToggled = enable;

            foreach (var alg in algorithmSequences.Values)
            {
                alg.Show(!enable);
            }
        }

        public void EnableOptions(bool enable)
        {
            
        }

        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {
            Course course;

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


                case ReplayAction.ToggleGreyboxLabyrinth:
                    EnableGreybox(!isGreyboxToggled);
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

                case ReplayAction.SequenceToggled:
                    course = (Course)args[0];
                    bool enabled = (bool)args[1];

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
                    break;

                case ReplayAction.SequenceSelected:

                    course = (Course)args[0];

                    foreach (AlgorithmSequence sq in algorithmSequences.Values)
                    {
                        sq.Show(false);
                    }

                    AlgorithmSequence sequence;

                    if (algorithmSequences.TryGetValue(course.Algorithm.Id, out sequence))
                    {
                        sequence.Show(true);
                    }

                    break;
            }
        }

        public void TrySetMoveCount(int candidateMvcnt)
        {
            if (candidateMvcnt > controller.GlobalMoveCount)
                controller.GlobalMoveCount = candidateMvcnt;
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
            labyrinth.gameObject.Destroy();

            labyrinth.gameObject.Destroy();

            foreach (Sequence sq in algorithmSequences.Values)
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
        }

        public void OnCourseAdded(Course course)
        {
            if (!playerSequences.ContainsKey(course.Id))
            {
                var sequence =
                    Resources.Instance.PlayerSequence.Create(
                        controller,
                        course,
                        labyrinth,
                        lposition);

                playerSequences.Add(course.Id, sequence);

                activeSequences.Add(sequence);

                TrySetMoveCount(sequence.LocalMoveCount);

                var algorithmSeq =
                    Resources.Instance.AlgorithmSequence.Create(
                        controller,
                        labyrinth,
                        course.Algorithm,
                        lposition
                        );

                algorithmSequences.Add(course.Algorithm.Id, algorithmSeq);              
                
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