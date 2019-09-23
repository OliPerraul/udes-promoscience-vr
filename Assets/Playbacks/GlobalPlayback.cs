using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;

namespace UdeS.Promoscience.Playbacks
{
    // Playback for a single team
    public class GlobalPlayback : MonoBehaviour
    {

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

        [SerializeField]
        private ScriptableIntegerArray recordedSteps;
        
        private List<PlayerSequence> playerSequences;
        
        private List<PlayerSequence> algorithmSequences;

        private Vector2Int labyrinthPosition;

        private Vector3 worldPosition;

        private bool playbackActive = false;

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

            if (serverGameState != null)
                serverGameState.gameStateChangedEvent += OnServerGameStateChanged;
        }


        public void OnServerGameStateChanged()
        {
            if (serverGameState.GameState == ServerGameState.ViewingPlayback)
            {
                Begin();
                playbackActive = true;
            }
            else if (playbackActive)
            {
                Stop();
                playbackActive = false;
            }
        }



        public void Begin()
        {
            StopAllCoroutines();

            labyrinth.GenerateLabyrinthVisual();
            labyrinthPosition = labyrinth.GetLabyrithStartPosition();

            worldPosition =
                labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

            foreach (PlayerSequenceData data in serverGameState.Sequences)
            {
                var sequence = playerSequenceTemplate.Create(
                    data, 
                    labyrinth, 
                    labyrinthPosition, 
                    worldPosition);

                playerSequences.Add(sequence);
            }

            StartCoroutine(PlayerSequenceCoroutine());
            //BeginPlayerSequence();
            //BeginAlgorithmSequence();
        }

        public IEnumerator PlayerSequenceCoroutine()
        {
            foreach (PlayerSequence sequence in playerSequences)
            {
                yield return sequence.StartCoroutine(sequence.BeginCoroutine());
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
