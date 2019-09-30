using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;

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

        [SerializeField]
        private AlgorithmSequence algorithmSequenceTemplate;
        
        private List<PlayerSequence> playerSequences;

        private PlayerSequence currentSequence;

        private List<PlayerSequence> algorithmSequences;

        private Vector2Int labyrinthPosition;

        private Vector3 worldPosition;

        private bool replayActive = false;

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
                algorithm = FindObjectOfType<Algorithm>();

            if (labyrinth == null)
                labyrinth = FindObjectOfType<Labyrinth>();
        
        }

        public void Awake()
        {
            //playerSequence = new PlayerSe
            playerSequences = new List<PlayerSequence>();
            algorithmSequences = new List<PlayerSequence>();
            serverGameState.gameStateChangedEvent += OnServerGameStateChanged;
            replayOptions.valueChangeEvent += OnPlaybackOptionsChanged;
            replayOptions.OnActionHandler += OnPlaybackAction;
        }

        public void OnProgress(float progress)
        {
            replayOptions.SendProgress(progress);
        }

        public void OnPlaybackAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.Previous:

                    if (currentSequence.HasPrevious)
                    {
                        currentSequence.Reverse();
                    }

                    break;

                case ReplayAction.Next:

                    if (currentSequence.HasNext)
                    {
                        currentSequence.Perform();
                    }

                    break;               

                case ReplayAction.Play:
                    currentSequence.Resume();
                    break;

                case ReplayAction.Resume:
                    currentSequence.Resume();
                    break;

                case ReplayAction.Pause:
                    currentSequence.Pause();
                    break;

                case ReplayAction.Stop:
                    currentSequence.Stop();
                    break;

                case ReplayAction.Slide:

                    float current = (float)args[0];
                    currentSequence.Move(current);

                    break;
            }
        }

        public void OnPlaybackOptionsChanged()
        {
            if (currentSequence != null)
            {
                currentSequence.OnProgressHandler -= OnProgress;
            }

            currentSequence = playerSequences[replayOptions.CourseIndex];
            currentSequence.OnProgressHandler += OnProgress;
        }
        
        public void OnServerGameStateChanged()
        {
            if (serverGameState.GameState == 
                ServerGameState.ViewingPlayback)
            {
                Begin();
                replayActive = true;
            }
            else if (replayActive)
            {
                Stop();
                replayActive = false;
            }
        }

        public void Begin()
        {
            StopAllCoroutines();

            labyrinth.GenerateLabyrinthVisual();
            labyrinthPosition = labyrinth.GetLabyrithStartPosition();

            worldPosition =
                labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

            for(int i = 0; i < replayOptions.Courses.Count; i++)
            {
                var sequence = 
                    playerSequenceTemplate.Create(
                        replayOptions.Courses[i],
                        labyrinth, 
                        labyrinthPosition, 
                        worldPosition);

                playerSequences.Add(sequence);
            }

            if (playerSequences.Count != 0)
            {
                OnPlaybackOptionsChanged();
            }
        }

        public IEnumerator PlayerSequenceCoroutine()
        {
            foreach (PlayerSequence sequence in playerSequences)
            {
                //yield return sequence.StartCoroutine(sequence.BeginCoroutine());
            }

            yield return null;
        }



        public void Stop()
        {
            StopAllCoroutines();
            labyrinth.DestroyLabyrinth();

            //if (playerSequence)
            //{
            //    Destroy(playerSequence.gameObject);
            //}

            //if (algorithmSequence)
            //{
            //    Destroy(algorithmSequence.gameObject);
            //}
        }

        private void BeginPlayerSequence()
        {
            //if (playerSequence != null)
            //    Destroy(playerSequence.gameObject);

            //playerSequence = playerSequenceTemplate.Create(labyrinth, labyrinthPosition, worldPosition);
            //StartCoroutine(PlayerSequenceCoroutine());
        }

        private void BeginAlgorithmSequence()
        {
            //if (algorithmSequence != null)
            //    Destroy(algorithmSequence.gameObject);

            //algorithmSequence = algorithmSequenceTemplate.Create(labyrinth, labyrinthPosition, worldPosition);

            StartCoroutine(AlgorithmSequenceCoroutine());
        }

        IEnumerator AlgorithmSequenceCoroutine()
        {
            //List<Tile> tiles = algorithm.GetAlgorithmSteps();

            //foreach (var tile in tiles)
            //{
            //    algorithmSequence.Perform(tile);
            //    yield return new WaitForSeconds(algorithmSequenceSpeed);
            //}

            yield return null;
        }


    }
}
