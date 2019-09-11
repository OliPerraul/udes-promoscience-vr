using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;
using System.Collections.Generic;

namespace UdeS.Promoscience
{
    public class PlaybackManager : MonoBehaviour
    {
        [SerializeField]
        private PlaybackCharacter playbackCharacterTemplate;

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


        public void BeginPlayback()
        {
            Vector2Int labPos = labyrinth.GetLabyrithStartPosition();

            Vector3 worldPosition =
                labyrinth.GetLabyrinthPositionInWorldPosition(labPos);

            BeginPlayerPlayback(labPos, worldPosition);
            BeginAlgorithmPlayback(labPos, worldPosition);
        }


        private void BeginPlayerPlayback(Vector2Int labPos, Vector3 worldPos)
        {
            PlaybackCharacter character = playbackCharacterTemplate.Create(labyrinth, labPos, worldPos);
            StartCoroutine(PlayerPlaybackCoroutine(character)); 
        }

        IEnumerator PlayerPlaybackCoroutine(PlaybackCharacter character)
        {
            foreach (int step in recordedSteps.Value)
            {
                character.Perform((GameAction)step);
                yield return new WaitForSeconds(playerPlaybackSpeed);
            }

            // TODO destroy..
            yield return null;
        }

        private void BeginAlgorithmPlayback(Vector2Int labPos, Vector3 worldPos)
        {
            //PlaybackCharacter character = Instantiate(
            //    playbackCharacterTemplate.gameObject,
            //    worldPos, Quaternion.identity)
            //    .GetComponent<PlaybackCharacter>();

            //List<Tile> tiles = algorithm.GetAlgorithmSteps();
        }

        IEnumerator AlgorithmPlaybackCoroutine()
        {
            yield return new WaitForSeconds(algorithmPlaybackSpeed);

            yield return null;
        }


    }
}
