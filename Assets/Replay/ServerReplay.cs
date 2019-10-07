using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;
using System;
using System.Linq;

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
        private float maxOffset = 5f;

        private int moveIndex = 0;

        private int moveCount = -999;

        private void SetMoveCount(int mvcnt)
        {
            moveCount = mvcnt;

            if (replayOptions.OnMoveCountSetHandler != null)
            {
                replayOptions.OnMoveCountSetHandler(moveCount);
            }
        }


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

                    if(!sequences[i].IsPlaying)
                    {
                        sequences.Remove(sequences[i]);
                    }
                }
            }
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

                case ReplayAction.Stop:

                    //mutex.WaitOne();

                    //Stop();

                    //mutex.ReleaseMutex();

                    break;
            }
        }

        public void OnProgress(int progress)
        {
            if (progress > moveIndex)
            {
                moveIndex = progress;

                if (replayOptions.OnProgressHandler != null)
                    replayOptions.OnProgressHandler.Invoke(moveIndex);
            }
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
                SetMoveCount(activeSequences.Max(x => x.MoveCount));
                AdjustSequences();
            }    
        }

        public void AdjustSequences()
        {
            for(int i = 0; i < activeSequences.Count; i++)
            {
                activeSequences[i].Adjust(i, activeSequences.Count, maxOffset);
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

        public void Begin()
        {
            StopAllCoroutines();

            labyrinth.GenerateLabyrinthVisual();

            labyrinthPosition = labyrinth.GetLabyrithStartPosition();

            worldPosition =
                labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

            foreach(Course course in serverGameState.Courses)
            {
                OnCourseAdded(course);
            }

            //algorithmSequence =
            //    algorithmSequenceTemplate.Create(
            //        labyrinth,
            //        algorithm,
            //        labyrinthPosition);

            //if (algorithmSequence.MoveCount > moveCount)
            //{
            //    SetMoveCount(algorithmSequence.MoveCount);
            //}
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

                SetMoveCount(sequence.MoveCount > moveCount ? sequence.MoveCount : moveCount);

                AdjustSequences();
            }
        }

        public void OnCourseRemoved(Course course)
        {
            if (serverGameState.GameState == ServerGameState.ViewingPlayback)
            {
                activeSequences.Remove(playerSequences[course.Id]);
                playerSequences.Remove(course.Id);

                SetMoveCount(activeSequences.Max(x => x.MoveCount));

                AdjustSequences();
            }
        }



    }
}
