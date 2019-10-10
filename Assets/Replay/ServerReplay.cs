using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace UdeS.Promoscience.Replay
{
    // Playback for a single team
    public class ServerReplay : MonoBehaviour
    {
        [SerializeField]
        private ScriptableReplayOptions replayOptions;

        [SerializeField]
        private Labyrinth labyrinth;

        [SerializeField]
        private Algorithm algorithm;

        [SerializeField]
        ScriptableServerGameInformation serverGameState;

        [SerializeField]
        private AlgorithmSequence algorithmSequenceTemplate;

        private AlgorithmSequence algorithmSequence;

        [SerializeField]
        private PlayerSequence playerSequenceTemplate;

        private Dictionary<int, PlayerSequence> playerSequences;

        private List<PlayerSequence> activeSequences;

        private Vector2Int labyrinthPosition;

        private Vector3 worldPosition;

        [SerializeField]
        private float tileWidth = 5f;

        private Mutex mutex;


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
                labyrinth = FindObjectOfType<Labyrinth>();
            }
        }

        public void Awake()
        {
            mutex = new Mutex();

            playerSequences = new Dictionary<int, PlayerSequence>();
            activeSequences = new List<PlayerSequence>();

            serverGameState.gameStateChangedEvent += OnServerGameStateChanged;
            serverGameState.OnCourseAddedHandler += OnCourseAdded;

            replayOptions.OnSequenceToggledHandler += OnSequenceToggled;
            replayOptions.OnActionHandler += OnReplayAction;

        }

        public void Resume()
        {
            List<Sequence> sequences = new List<Sequence>();
            sequences.AddRange(activeSequences.Cast<Sequence>());
            sequences.Add(algorithmSequence);

            StartCoroutine(ResumeCoroutine(sequences));

        }

        public IEnumerator ResumeCoroutine(List<Sequence> sequences)
        {
            while (sequences.Count != 0)
            {
                foreach (var sq in sequences)
                {
                    sq.Resume();
                }

                for (int i = sequences.Count - 1; i >= 0; i--) //  var sq in sequences)
                {
                    yield return sequences[i].ResumeCoroutineResult;

                    if (!sequences[i].IsPlaying)
                    {
                        sequences.Remove(sequences[i]);
                    }
                }
            }
        }

        public void Next()
        {
            List<Sequence> sequences = new List<Sequence>();
            sequences.AddRange(activeSequences.Cast<Sequence>());
            sequences.Add(algorithmSequence);

            foreach (var sq in sequences)
            {
                if(sq.WithinBounds) sq.Next();
            }

            replayOptions.GlobalMoveIndex++;
        }

        public void Previous()
        {
            List<Sequence> sequences = new List<Sequence>();
            sequences.AddRange(activeSequences.Cast<Sequence>());
            sequences.Add(algorithmSequence);

            replayOptions.GlobalMoveIndex--;

            foreach (var sq in sequences)
            {
                if (sq.WithinBounds) sq.Previous();
            }
        }


        public void Move(int target)
        {
            List<Sequence> sequences = new List<Sequence>();
            sequences.AddRange(activeSequences.Cast<Sequence>());
            sequences.Add(algorithmSequence);

            replayOptions.GlobalMoveIndex = target;

            foreach (var sq in sequences)
            {
                sq.Move(target);
            }
        }

        public virtual void Pause()
        {
            //isPlaying = false;
            StopAllCoroutines();
            Move(replayOptions.GlobalMoveIndex - 1);
        }


        public virtual void Stop()
        {
            //isPlaying = false;
            StopAllCoroutines();
            Move(0);
        }


        public void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                // TODO: Handle play/ stop from replay object and not sequences
                // to prevent synch issues

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
            if (candidateMvcnt > replayOptions.GlobalMoveCount)
                replayOptions.GlobalMoveCount = candidateMvcnt;
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
                Move(replayOptions.GlobalMoveIndex);

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
            if (replayOptions.OnSequenceFinishedHandler != null)
            {
                replayOptions.OnSequenceFinishedHandler.Invoke();
            }
        }

        public void OnServerGameStateChanged()
        {
            if (serverGameState.GameState ==
                ServerGameState.ViewingPlayback)
            {
                Begin();
            }
        }

        public void Clear()
        {
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

            labyrinth.GenerateLabyrinthVisual();

            labyrinthPosition = labyrinth.GetLabyrithStartPosition();

            worldPosition =
                labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

            Clear();

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
            if (serverGameState.GameState == ServerGameState.ViewingPlayback)
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
            if (serverGameState.GameState == ServerGameState.ViewingPlayback)
            {
                activeSequences.Remove(playerSequences[course.Id]);
                playerSequences.Remove(course.Id);

                TrySetMoveCount(activeSequences.Max(x => x.LocalMoveCount));

                AdjustOffsets();
            }
        }

    }
}
