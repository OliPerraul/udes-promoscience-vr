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
    public class GlobalReplay : MonoBehaviour
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

            replayOptions.OnActionHandler += OnReplayAction;
            
            replayOptions.OnSequenceToggledHandler += OnSequenceToggled;
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

        public void OnReplayAction(ReplayAction action, params object[] args)
        {
            foreach (PlayerSequence sequence in activeSequences)
            {
                sequence.HandleAction(action, args);
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

            //AdjustSequences();
        }

        public void OnCourseAdded(Course course)
        {
            if (serverGameState.GameState == ServerGameState.ViewingPlayback)
            {
                var sequence =
                    playerSequenceTemplate.Create(
                        course,
                        labyrinth,
                        labyrinthPosition,
                        worldPosition);

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
