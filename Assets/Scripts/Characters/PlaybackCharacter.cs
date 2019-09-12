using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience
{
    public class PlaybackCharacter : MonoBehaviour
    {
        [SerializeField]
        private float speed = 0.6f;
        
        private Labyrinth labyrinth;

        private Vector2Int labyrinthPosition;

        private Vector3 targetPosition;

        public PlaybackCharacter Create(Labyrinth labyrinth, Vector2Int labpos, Vector3 worldPos)
        {
            PlaybackCharacter character = Instantiate(
                gameObject,
                worldPos, Quaternion.identity)
                .GetComponent<PlaybackCharacter>();

            character.labyrinth = labyrinth;
            character.labyrinthPosition = labpos;
            character.transform.position = worldPos;
            character.targetPosition = worldPos;

            return character;
        }


        public void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed);
        }

        public void Perform(GameAction gameAction, string info)
        {
            int forwardDirection = labyrinth.GetStartDirection();

            if (gameAction == GameAction.MoveUp)
            {
                labyrinthPosition.y -= 1;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
            }
            else if (gameAction == GameAction.MoveRight)
            {
                labyrinthPosition.x += 1;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
            }
            else if (gameAction == GameAction.MoveDown)
            {
                labyrinthPosition.y += 1;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
            }
            else if (gameAction == GameAction.MoveLeft)
            {
                labyrinthPosition.x -= 1;
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition);
            }
            else if (gameAction == GameAction.TurnRight)
            {
                forwardDirection = (forwardDirection + 1) % 4;
            }
            else if (gameAction == GameAction.TurnLeft)
            {
                forwardDirection = (forwardDirection - 1) < 0 ? 3 : (forwardDirection - 1);
            }
            else if (gameAction == GameAction.PaintFloorYellow)
            {
                labyrinth.SetTileColor(labyrinthPosition, TileColor.Yellow);
            }
            else if (gameAction == GameAction.PaintFloorRed)
            {
                labyrinth.SetTileColor(labyrinthPosition, TileColor.Red);
            }
            else if (gameAction == GameAction.UnpaintFloor)
            {
                labyrinth.SetTileColor(labyrinthPosition, TileColor.Grey);
            }
            else if (gameAction == GameAction.ReturnToDivergencePoint)
            {
                //errorCounter += 5;

                //var position = new Vector2Int(playerSteps[playerSteps.Count - 1].x, playerSteps[playerSteps.Count - 1].y);
                //forwardDirection = GetForwardDirectionWithRotation(rotationAtDivergence);

                //TileColor previousColor;

                //for (int j = wrongColorTilesWhenDiverging.Count - 1; j >= 0; j--)
                //{
                //    Tile tile = wrongColorTilesWhenDiverging[j];

                //    previousColor = tiles[tile.x, tile.y];
                //    tiles[tile.x, tile.y] = tile.color;

                //    EvaluateAlgorithmRespectOnPaintTile(position, tile.x, tile.y, tile.color, previousColor);
                //}

                //EvaluateAlgorithmRespectOnPositionChanged(position, tiles[currentLabyrinthPosition.x, currentLabyrinthPosition.y], GetRotationWithForwardDirection(forwardDirection));
            }
        }
            

    }
}