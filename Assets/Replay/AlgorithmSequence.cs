using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Replay
{
    public class AlgorithmSequence : MonoBehaviour
    {
        [SerializeField]
        private float speed = 0.6f;
        
        private Labyrinth labyrinth;

        private Vector2Int labyrinthPosition;

        private Vector3 targetPosition;

        private Tile lastTile;

        public AlgorithmSequence Create(Labyrinth labyrinth, Vector2Int labpos, Vector3 worldPos)
        {
            AlgorithmSequence character = Instantiate(
                gameObject,
                worldPos, Quaternion.identity)
                .GetComponent<AlgorithmSequence>();

            character.labyrinth = labyrinth;
            character.labyrinthPosition = labpos;
            character.transform.position = worldPos;
            character.targetPosition = worldPos;

            lastTile.x = -99999;
            lastTile.y = -99999;

            return character;
        }

        public void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
        }

        public void Perform(Tile tile)
        {
            int forwardDirection = labyrinth.GetStartDirection();

            // If tile is adjacent, lerp, otherwise teleport
            // left
            if (
                (tile.x == lastTile.x - 1 && tile.y == lastTile.y) ||
                (tile.x == lastTile.x + 1 && tile.y == lastTile.y) ||
                (tile.x == lastTile.x && tile.y == lastTile.y - 1) ||
                (tile.x == lastTile.x && tile.y == lastTile.y + 1) ||
                (tile.x == lastTile.x && tile.y == lastTile.y))
            {
                labyrinthPosition = tile.Position;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
            }
            else
            {
                labyrinthPosition = tile.Position;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
                transform.position = targetPosition;
            }

            lastTile = tile;
        }         
    }
}