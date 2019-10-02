using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replay
{
    // Playback for a single team
    public class LocalReplay : MonoBehaviour
    {
        [SerializeField]
        ScriptableClientGameState gameState;

        [SerializeField]
        private ScriptableClientGameData gameData;

        [SerializeField]
        private PlayerSequence playerSequenceTemplate;

        private PlayerSequence playerSequence;

        [SerializeField]
        private AlgorithmSequence algorithmSequenceTemplate;

        private AlgorithmSequence algorithmSequence;

        [SerializeField]
        private Labyrinth labyrinth;

        [SerializeField]
        private Algorithm algorithm;


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

        public void Awake()
        {
            gameState.valueChangedEvent += OnGameStateChanged;

            if(gameState != null)
                gameState.valueChangedEvent += OnServerGameStateChanged;
        }

        public void OnGameStateChanged()
        {
            if (gameState.Value == ClientGameState.ViewingLocalReplay)
            {                
                Begin();
            }
        }

        public void OnServerGameStateChanged()
        {
            if (gameState.Value == ClientGameState.ViewingLocalReplay)
            {                 
                Begin();
                playbackActive = true;
            }
            else if(playbackActive)
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

            BeginPlayerSequence();
            BeginAlgorithmSequence();
        }

        public void Stop()
        {
            StopAllCoroutines();
            labyrinth.DestroyLabyrinth();

            if (playerSequence)
            {
                Destroy(playerSequence.gameObject);
            }

            if (algorithmSequence)
            {
                Destroy(algorithmSequence.gameObject);
            }
        }

        private void BeginPlayerSequence()
        {
            //if (playerSequence != null)
            //    Destroy(playerSequence.gameObject);

            //playerSequence = playerSequenceTemplate.Create(labyrinth, labyrinthPosition, worldPosition);
            //StartCoroutine(PlayerSequenceCoroutine());
        }

        public IEnumerator PlayerSequenceCoroutine()
        {
            for(int i = 0; i < gameData.ActionSteps.Length; i++)
            {
                //yield return StartCoroutine(
                //    playerSequence.DoPerformCoroutine(
                //        (GameAction)gameData.ActionSteps[i], 
                //        gameData.ActionValues[i]));                
            }

            yield return null;
        }

        private void BeginAlgorithmSequence()
        {
            //if (algorithmSequence != null)
                //Destroy(algorithmSequence.gameObject);

            //algorithmSequence = algorithmSequenceTemplate.Create(labyrinth, labyrinthPosition, worldPosition);

            //StartCoroutine(AlgorithmSequenceCoroutine());
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
