﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Cirrus.Extensions;
using System.Linq;

namespace UdeS.Promoscience.Replays
{
    public class LabyrinthReplay
    {
        public Replay replay;

        public Labyrinths.Labyrinth labyrinth;

        public Labyrinths.Labyrinth emptyLabyrinth;

        protected Vector2Int labyrinthPosition;

        protected Vector3 worldPosition;

        private Dictionary<int, PlayerSequence> playerSequences;

        private List<PlayerSequence> activeSequences;

        private List<AlgorithmSequence> algorithmSequences;

        private System.Threading.Mutex mutex;

        private bool isAlgorithmToggled = false;

        private bool isPlaying = false;

        public LabyrinthReplay(
            Replay replay,
            Labyrinths.Labyrinth labyrinth)            
        {
            this.replay = replay;
            //this.courses = courses;
            this.labyrinth = labyrinth;

            mutex = new System.Threading.Mutex();

            playerSequences = new Dictionary<int, PlayerSequence>();
            activeSequences = new List<PlayerSequence>();
            algorithmSequences = new List<AlgorithmSequence>();

            //replay..gameStateChangedEvent += OnServerGameStateChanged;
            ServerGame.Instance.OnCourseAddedHandler += OnCourseAdded;

            replay.Controller.OnSequenceToggledHandler += OnSequenceToggled;
            replay.Controller.OnActionHandler += OnReplayAction;
                       
        }

        public virtual void Start()
        {
            //labyrinth.GenerateLabyrinthVisual();
            //emptyLabyrinth = labyrinth.Create();
            //emptyLabyrinth.gameObject.SetActive(false);

            labyrinthPosition = labyrinth.GetLabyrithStartPosition();

            worldPosition =
                labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

            labyrinth.gameObject.SetActive(true);
            labyrinth.Camera.Maximize();

            //TrySetMoveCount(algorithmSequences.Max(x => x.LocalMoveCount));

            foreach (Course course in replay.Controller.Courses)
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

            replay.Controller.GlobalMoveIndex++;
        }

        public virtual void Previous()
        {
            if (isPlaying)
            {
                Pause();
            }

            replay.Controller.GlobalMoveIndex--;

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
            replay.Controller.GlobalMoveIndex = target;

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

            Move(replay.Controller.GlobalMoveIndex);

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

        public virtual void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                // TODO: Handle play/ stop from replay object and not sequences
                // to prevent synch issues
                case ReplayAction.ToggleAlgorithm:
                    isAlgorithmToggled = !isAlgorithmToggled;
                    emptyLabyrinth.gameObject.SetActive(!isAlgorithmToggled);
                    labyrinth.gameObject.SetActive(isAlgorithmToggled);
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
            if (candidateMvcnt > replay.Controller.GlobalMoveCount)
                replay.Controller.GlobalMoveCount = candidateMvcnt;
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
                Move(Mathf.Clamp(replay.Controller.GlobalMoveIndex, 0, replay.Controller.GlobalMoveCount));

                AdjustOffsets();
            }
        }

        public virtual float GetOffsetAmount(float idx)
        {
            return
                // origin of segment at the center, move it to the left
                (-Promoscience.Utils.TILE_SIZE / 2) +
                // number of offsets (minimum 1 to align back at the center if no other sgms)
                (idx + 1) *
                // width of the level divided by the amount of sequences we are trying to fit
                // Number of player sequences + 1 (active sequences contains algorithm)
                (Promoscience.Utils.TILE_SIZE / (activeSequences.Count + 1));
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
            if (replay.Controller.OnSequenceFinishedHandler != null)
            {
                replay.Controller.OnSequenceFinishedHandler.Invoke();
            }
        }

        public virtual void Clear()
        {
            Object.Destroy(emptyLabyrinth);            

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
        }

        public void OnCourseAdded(Course course)
        {
            if (!playerSequences.ContainsKey(course.Id))
            {
                var sequence =
                    replay.Resources.PlayerSequence.Create(
                        replay,
                        course,
                        labyrinth,
                        labyrinthPosition);

                playerSequences.Add(course.Id, sequence);

                activeSequences.Add(sequence);

                TrySetMoveCount(sequence.LocalMoveCount);


                var algorithmSeq =
                    replay.Resources.AlgorithmSequence.Create(
                        replay,
                        labyrinth,
                        course.Algorithm,
                        labyrinthPosition
                        );

                algorithmSequences.Add(algorithmSeq);              

                TrySetMoveCount(algorithmSeq.LocalMoveCount);
                               
                AdjustOffsets();

                replay.Controller.SendAction(ReplayAction.AddCourse, true, course);
            }
        }

        public void OnCourseRemoved(Course course)
        {
            activeSequences.Remove(playerSequences[course.Id]);
            playerSequences.Remove(course.Id);

            TrySetMoveCount(activeSequences.Max(x => x.LocalMoveCount));

            AdjustOffsets();

            replay.Controller.SendAction(ReplayAction.AddCourse, false, course);
        }
    }
}