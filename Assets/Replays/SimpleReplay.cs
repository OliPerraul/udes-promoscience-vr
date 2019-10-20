using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Replays
{
    // Playback for a single team
    public class SimpleReplay : Replay
    {
        [SerializeField]
        private ScriptableController replayController;

        protected override ScriptableController ReplayController
        {
            get
            {
                return replayController;
            }
        }


        [SerializeField]
        private bool isAlgorithmToggled = true;

        [SerializeField]
        private Labyrinths.Labyrinth labyrinth;

        // Empty labyrinth used for non algorithm view
        private Labyrinths.Labyrinth emptyLabyrinth;

        [SerializeField]
        private Algorithm algorithm;

        [SerializeField]
        ScriptableServerGameInformation serverGameState;

        [SerializeField]
        private AlgorithmSequence algorithmSequenceTemplate;

        [SerializeField]
        private PlayerSequence playerSequenceTemplate;

        private Dictionary<int, PlayerSequence> playerSequences;
        
        private AlgorithmSequence algorithmSequence;

        private List<Sequence> activeSequences;

        private Vector2Int labyrinthPosition;

        private Vector3 worldPosition;

        [SerializeField]
        private float tileWidth = 5f;

        private Mutex mutex;

        private bool isPlaying = false;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Begin();
            }
        }

        public void OnValidate()
        {
            if (algorithm == null)
            {
                algorithm = FindObjectOfType<Algorithm>();
            }

            if (labyrinth == null)
            {
                labyrinth = FindObjectOfType<Labyrinths.Labyrinth>();
            }
        }

        public void Awake()
        {
            mutex = new Mutex();

            playerSequences = new Dictionary<int, PlayerSequence>();
            activeSequences = new List<Sequence>();

            serverGameState.gameStateChangedEvent += OnServerGameStateChanged;
            serverGameState.OnCourseAddedHandler += OnCourseAdded;

            replayController.OnSequenceToggledHandler += OnSequenceToggled;
            replayController.OnActionHandler += OnReplayAction;

        }

        public void Resume()
        {
            isPlaying = true;

            algorithmSequence.Resume();

            foreach (var sq in activeSequences)
            {
                sq.Resume();
            }
        }

        public void Next()
        {
            if (isPlaying)
            {
                Pause();
            }

            algorithmSequence.Next();

            foreach (var sq in activeSequences)
            {
                if(sq.WithinBounds) sq.Next();
            }

            replayController.GlobalMoveIndex++;
        }

        public void Previous()
        {
            if (isPlaying)
            {
                Pause();
            }

            replayController.GlobalMoveIndex--;

            algorithmSequence.Previous();

            foreach (var sq in activeSequences)
            {
                if (sq.WithinBounds) sq.Previous();
            }
        }

        public void Move(int target)
        {
            replayController.GlobalMoveIndex = target;

            algorithmSequence.Move(target);

            foreach (var sq in activeSequences)
            {
                sq.Move(target);
            }
        }

        public virtual void Pause()
        {
            if (!isPlaying)
                return;

            algorithmSequence.Stop();// (target);

            foreach (var sq in activeSequences)
            {
                sq.Stop();
            }

            Move(replayController.GlobalMoveIndex);

            isPlaying = false;
        }


        public virtual void Stop()
        {
            algorithmSequence.Stop();

            foreach (var sq in activeSequences)
            {
                sq.Stop();
            }
            
            Move(0);

            isPlaying = false;
        }


        public void OnReplayAction(ReplayAction action, params object[] args)
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

        private void TrySetMoveCount(int candidateMvcnt)
        {
            if (candidateMvcnt > replayController.GlobalMoveCount)
                replayController.GlobalMoveCount = candidateMvcnt;
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
                Move(Mathf.Clamp(replayController.GlobalMoveIndex, 0, replayController.GlobalMoveCount));

                AdjustOffsets();
            }
        }

        public float GetOffsetAmount(float idx)
        {
            return 
                // origin of segment at the center, move it to the left
                (-tileWidth/2) + 
                // number of offsets (minimum 1 to align back at the center if no other sgms)
                (idx + 1) * 
                // width of the level divided by the amount of sequences we are trying to fit
                // Number of player sequences + 1 (active sequences contains algorithm)
                (tileWidth / (activeSequences.Count + 1));
        }

        public void AdjustOffsets()
        {
            for (int i = 0; i < activeSequences.Count; i++)
            {
                activeSequences[i].AdjustOffset(GetOffsetAmount(i));
            }
        }

        private void OnSequenceFinished()
        {
            if (replayController.OnSequenceFinishedHandler != null)
            {
                replayController.OnSequenceFinishedHandler.Invoke();
            }
        }

        public override void OnServerGameStateChanged()
        {
            if (serverGameState.GameState ==
                ServerGameState.Replay)
            {
                Begin();
            }
        }

        public void Clear()
        {
            Destroy(emptyLabyrinth);//.DestroyLabyrinth();

            if (algorithmSequence != null)
            {
                Destroy(algorithmSequence.gameObject);
                algorithmSequence = null;
            }

            foreach (Sequence sq in playerSequences.Values)
            {
                if (sq != null)
                {
                    Destroy(sq.gameObject);
                }
            }

            playerSequences.Clear();
            activeSequences.Clear();
        }

        public void Begin()
        {
            StopAllCoroutines();

            Clear();

            labyrinth.GenerateLabyrinthVisual();
            emptyLabyrinth = labyrinth.Create(transform);
            emptyLabyrinth.gameObject.SetActive(false);

            labyrinthPosition = labyrinth.GetLabyrithStartPosition();

            worldPosition =
                labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

            foreach (Course course in serverGameState.Courses)
            {
                OnCourseAdded(course);
            }

            algorithmSequence =
                algorithmSequenceTemplate.Create(
                    labyrinth,
                    algorithm,
                    labyrinthPosition);
            
            TrySetMoveCount(algorithmSequence.LocalMoveCount);

        }



        public void OnCourseAdded(Course course)
        {
            if (serverGameState.GameState == ServerGameState.Replay)
            {
                var sequence =
                    playerSequenceTemplate.Create(
                        course,
                        labyrinth,
                        labyrinthPosition);

                playerSequences.Add(course.Id, sequence);
                activeSequences.Add(sequence);

                TrySetMoveCount(sequence.LocalMoveCount);

                AdjustOffsets();
            }
        }

        public void OnCourseRemoved(Course course)
        {
            if (serverGameState.GameState == ServerGameState.Replay)
            {
                activeSequences.Remove(playerSequences[course.Id]);
                playerSequences.Remove(course.Id);

                TrySetMoveCount(activeSequences.Max(x => x.LocalMoveCount));

                AdjustOffsets();
            }
        }

        //public override void OnServerGameStateChanged()
        //{
        //    throw new NotImplementedException();
        //}

        //public override void OnServerGameStateChanged()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
