using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;

namespace UdeS.Promoscience.Playbacks
{
    public class PlaybackManager : MonoBehaviour
    {
        //[SerializeField]
        //ScriptableServerGameInformation serverGameState;

        [SerializeField]
        ScriptableClientGameState gameState;

        [SerializeField]
        private ScriptableClientGameData gameData;

        [SerializeField]
        private PlayerPlayback playerPlaybackTemplate;

        private PlayerPlayback playerPlayback;

        [SerializeField]
        private AlgorithmPlayback algorithmPlaybackTemplate;

        private AlgorithmPlayback algorithmPlayback;

        [SerializeField]
        private Labyrinth labyrinth;

        [SerializeField]
        private Algorithm algorithm;

        [SerializeField]
        private ScriptableIntegerArray recordedSteps;

        [SerializeField]
        private float playerPlaybackSpeed = 0.5f;

        [SerializeField]
        private float algorithmPlaybackSpeed = 0.5f;

        private Vector2Int labyrinthPosition;

        private Vector3 worldPosition;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                BeginPlayback();
            }
        }

        public void Awake()
        {
            gameState.valueChangedEvent += OnGameStateChanged;
            //serverGameState.gameStateChangedEvent += OnServerGameStateChanged;
        }


        public void OnGameStateChanged()
        {
            if (gameState.Value == ClientGameState.ViewingPlayback)
            {                
                BeginPlayback();
            }
        }

        //public void OnServerGameStateChanged()
        //{
        //    if (serverGameState.GameState == ServerGameState.ViewingPlayback)
        //    {
        //        BeginPlayback();
        //    }
        //}


        public void BeginPlayback()
        {
            StopAllCoroutines();

            labyrinth.GenerateLabyrinthVisual();
            labyrinthPosition = labyrinth.GetLabyrithStartPosition();

            worldPosition =
                labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);

            BeginPlayerPlayback();
            BeginAlgorithmPlayback();
        }

        private void BeginPlayerPlayback()
        {
            if (playerPlayback != null)
                Destroy(playerPlayback.gameObject);

            playerPlayback = playerPlaybackTemplate.Create(labyrinth, labyrinthPosition, worldPosition);
            StartCoroutine(PlayerPlaybackCoroutine());
        }

        public IEnumerator PlayerPlaybackCoroutine()
        {
            for(int i = 0; i < recordedSteps.Value.Length; i++)
            {
                yield return StartCoroutine(
                    playerPlayback.Perform(
                        (GameAction)recordedSteps.Value[i], 
                        gameData.ActionValues[i]));                
            }

            yield return null;
        }

        private void BeginAlgorithmPlayback()
        {
            if (algorithmPlayback != null)
                Destroy(algorithmPlayback.gameObject);

            algorithmPlayback = algorithmPlaybackTemplate.Create(labyrinth, labyrinthPosition, worldPosition);

            StartCoroutine(AlgorithmPlaybackCoroutine());
        }

        IEnumerator AlgorithmPlaybackCoroutine()
        {
            List<Tile> tiles = algorithm.GetAlgorithmSteps();

            foreach (var tile in tiles)
            {
                algorithmPlayback.Perform(tile);
                yield return new WaitForSeconds(algorithmPlaybackSpeed);
            }

            yield return null;
        }


    }
}
