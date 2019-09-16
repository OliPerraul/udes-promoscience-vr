using UnityEngine;
using System.Collections;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience
{
    public class PlayerPlayback : MonoBehaviour
    {
        [SerializeField]
        private float speed = 0.6f;
        
        private Labyrinth labyrinth;

        private Vector2Int labyrinthPosition;

        private Vector3 targetPosition;

        private TileColor lastColor;

        public PlayerPlayback Create(Labyrinth labyrinth, Vector2Int labpos, Vector3 worldPos)
        {
            PlayerPlayback character = Instantiate(
                gameObject,
                worldPos, Quaternion.identity)
                .GetComponent<PlayerPlayback>();

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
                lastColor = labyrinth.GetTileColor(labyrinthPosition);
                labyrinth.SetTileColor(labyrinthPosition, TileColor.Yellow);
            }
            else if (gameAction == GameAction.PaintFloorRed)
            {
                lastColor = labyrinth.GetTileColor(labyrinthPosition);
                labyrinth.SetTileColor(labyrinthPosition, TileColor.Red);
            }
            else if (gameAction == GameAction.UnpaintFloor)
            {;
                lastColor = labyrinth.GetTileColor(labyrinthPosition);
                labyrinth.SetTileColor(labyrinthPosition, TileColor.Grey);
            }
            else if (gameAction == GameAction.ReturnToDivergencePoint)
            {
                ActionInfo actionInfo = JsonUtility.FromJson<ActionInfo>(info);
                
                targetPosition = labyrinth.GetLabyrinthPositionInWorldPosition(labyrinthPosition) ;

                transform.position = targetPosition;
                transform.rotation = actionInfo.rotation;

                // Undo wrong tiles (apply the correct colors)
                foreach (Tile tile in actionInfo.tiles)
                {
                    labyrinth.SetTileColor(tile.Position, tile.color);
                }

                // Undo last action
                labyrinth.SetTileColor(labyrinthPosition, lastColor);

                // Reset labyrinth position and mark current tile
                labyrinthPosition = actionInfo.position;
                labyrinth.SetTileColor(labyrinthPosition, TileColor.Yellow);
            }
        }
            

    }
}