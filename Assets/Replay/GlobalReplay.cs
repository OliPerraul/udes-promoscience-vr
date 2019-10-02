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
        private PlayerSequence playerSequenceTemplate;
        
        private Dictionary<int, PlayerSequence> playerSequences;

        private List<PlayerSequence> activeSequences;

        private Vector2Int labyrinthPosition;

        private Vector3 worldPosition;

        private float maxOffset = 5f;

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

            replayOptions.OnActionHandler += OnPlaybackAction;            
        }

        public void OnProgress(int progress)
        {
            if(replayOptions.OnProgressHandler != null)
                replayOptions.OnProgressHandler.Invoke(progress);
        }

        public void OnPlaybackAction(ReplayAction action, params object[] args)
        {
            foreach (PlayerSequence sequence in activeSequences)
            {
                sequence.HandleAction(action, args);
            }
        }

        public void OnSequenceToggled(CourseData course, bool enabled)
        {
            playerSequences[course.Id].gameObject.SetActive(enabled);

            if (!enabled)
            {
                activeSequences.Remove(playerSequences[course.Id]);
            }

            AdjustOffsets();      
        }

        public void AdjustOffsets()
        {
            for(int i = 0; i < activeSequences.Count; i++)
            {
                activeSequences[i].AdjustOffset(i, activeSequences.Count, maxOffset);
            }
        }
        
        //public void OnPlaybackOptionsChanged()
        //{
        //    if (currentSequence != null)
        //    {
        //        currentSequence.OnProgressHandler -= OnProgress;
        //    }

        //    currentSequence = playerSequences[replayOptions.CourseIndex];

        //    currentSequence.OnProgressHandler += OnProgress;
        //    currentSequence.OnSequenceFinishedHandler += OnSequenceFinished;

        //    if (replayOptions.OnSequenceChangedHandler != null)
        //    {
        //        replayOptions.OnSequenceChangedHandler.Invoke(currentSequence);
        //    }
        //}

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

            int idx = 0;

            foreach(CourseData course in serverGameState.Courses)
            {
                var sequence =
                    playerSequenceTemplate.Create(
                        course,
                        labyrinth,
                        labyrinthPosition,
                        worldPosition);

                playerSequences.Add(course.Id, sequence);
                activeSequences.Add(sequence);
            }

            AdjustOffsets();
        }

        public void OnCourseAdded(CourseData course)
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
            }
        }

        public void OnCourseRemoved(CourseData course)
        {
            if (serverGameState.GameState == ServerGameState.ViewingPlayback)
            {

                activeSequences.Remove(playerSequences[course.Id]);
                playerSequences.Remove(course.Id);
                AdjustOffsets();
            }
        }



    }
}
